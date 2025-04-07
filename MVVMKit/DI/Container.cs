using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using MVVMKit.MVVM;
using MVVMKit.Regions;

namespace MVVMKit.DI
{
    /// <summary>
    /// 의존성 주입(DI) 컨테이너
    /// - Singleton, Transient, Instance 등록 지원
    /// - 생성자 주입 기반 자동 생성 지원
    /// </summary>
    public class Container : IContainerProvider, IContainerRegistry
    {
        // 타입별 생성자 함수 등록
        private readonly Dictionary<Type, Func<object>> _registrations = new Dictionary<Type, Func<object>>();

        // 싱글톤 인스턴스 보관소 (한 번만 생성된 객체 저장)
        private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();

        // Key - Type 매핑
        private readonly Dictionary<string, Type> _namedTypes = new Dictionary<string, Type>();

        /// <summary>
        /// 현재는 단순 View와 ViewModel을 싱글톤으로 등록해줌.
        /// Navigation 관련 기능은 구현 전
        /// </summary>
        public void RegisterForNavigation<TView, TViewModel>(string viewKey = null, string viewModelKey = null)
            where TView : FrameworkElement
            where TViewModel : class, INavigationAware
        {
            if (viewKey == null) viewKey = typeof(TView).Name;

            RegisterSingleton<TView>(viewKey);

            if (viewModelKey == null) RegisterSingleton<TViewModel>();
            else RegisterSingleton<TViewModel>(viewModelKey);

            ViewModelLocator.WireViewViewModel<TView, TViewModel>();
        }

        public void RegisterSingleton<TImplementation>(string key) where TImplementation : class
        {
            _namedTypes[key] = typeof(TImplementation);
            RegisterSingleton<TImplementation>();
        }

        public void RegisterSingleton<TInterface, TImplementation>(string key) where TImplementation : class, TInterface
        {
            _namedTypes[key] = typeof(TInterface);
            RegisterSingleton<TInterface, TImplementation>();
        }

        public void RegisterTransient<TInterface, TImplementation>(string key) where TImplementation : class, TInterface
        {
            _namedTypes[key] = typeof(TInterface);
            RegisterTransient<TInterface, TImplementation>();
        }

        /// <summary>
        /// 구현체만 주어졌을 때 자기 자신을 싱글톤으로 등록
        /// </summary>
        public void RegisterSingleton<TImplementation>() where TImplementation : class
            => RegisterSingleton<TImplementation, TImplementation>();

        /// <summary>
        /// 지정한 인터페이스/구현체 타입을 싱글톤으로 등록.
        /// </summary>
        public void RegisterSingleton<TInterface, TImplementation>()
            where TImplementation : class, TInterface
        {
            _registrations[typeof(TInterface)] = () =>
            {
                // 아직 생성되지 않았으면 생성해서 저장
                if (!_singletons.ContainsKey(typeof(TInterface)))
                {
                    object instance = CreateWithConstructor(typeof(TImplementation));
                    _singletons[typeof(TInterface)] = instance;
                }

                // 항상 동일 인스턴스 반환
                return _singletons[typeof(TInterface)];
            };
        }

        /// <summary>
        /// 외부에서 생성한 인스턴스를 직접 싱글톤으로 등록
        /// </summary>
        public void RegisterInstance<TInterface>(TInterface instance)
        {
            _singletons[typeof(TInterface)] = instance;
        }

        /// <summary>
        /// 지정한 인터페이스/구현체를 트랜지언트(매번 새로 생성) 방식으로 등록.
        /// </summary>
        public void RegisterTransient<TInterface, TImplementation>()
            where TImplementation : class, TInterface
        {
            _registrations[typeof(TInterface)] = () =>
            {
                // 매번 Resolve할 때마다 새 인스턴스 생성
                return CreateWithConstructor(typeof(TImplementation));
            };
        }

        public object Resolve(string key)
        {
            if (!_namedTypes.TryGetValue(key, out var type))
                throw new InvalidOperationException($"Type with key '{key}' not registered.");

            return Resolve(type);
        }

        public T Resolve<T>(string key)
        {
            return (T)Resolve(key);
        }

        /// <summary>
        /// 제네릭 타입으로 Resolve (등록된 인스턴스를 반환)
        /// </summary>
        public TInterface Resolve<TInterface>()
        {
            return (TInterface)Resolve(typeof(TInterface));
        }

        /// <summary>
        /// 타입 기반으로 등록된 인스턴스를 반환하거나 예외 발생
        /// </summary>
        public object Resolve(Type type, bool autoWireRegisteredViewModel = true)
        {
            object returnValue;

            // RegisterInstance 또는 Singleton으로 등록된 경우
            if (_singletons.ContainsKey(type))
            {
                returnValue = _singletons[type];
            }
            // 등록된 생성자 팩토리가 있는 경우
            else if (_registrations.ContainsKey(type))
            {
                returnValue = _registrations[type]();
            }
            // 등록되지 않은 타입을 자동으로 생성
            else
            {
                returnValue = CreateWithConstructor(type);
            }

            // FrameworkElement 이면서 ViewModelLocator에 매핑되어있다면 DataContext 등록
            if (autoWireRegisteredViewModel)
            {
                if (returnValue is FrameworkElement fe)
                {
                    if (ViewModelLocator.IsMapping(type))
                    {
                        Type viewModelType = ViewModelLocator.GetViewModelTypeForView(type);
                        object viewModel = Resolve(viewModelType);
                        fe.DataContext = viewModel;
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 생성자 분석을 통해 의존성을 재귀적으로 Resolve하고 인스턴스를 생성.
        /// </summary>
        private object CreateWithConstructor(Type type)
        {
            // public 생성자만 가져옴
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length == 0)
                throw new InvalidOperationException("Public constructor not found for type: " + type.FullName);

            // 파라미터 수가 가장 많은 생성자 선택 (보통 DI 용도로 설계된 생성자)
            ConstructorInfo constructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            ParameterInfo[] parameters = constructor.GetParameters();
            object[] args = new object[parameters.Length];

            // 생성자 파라미터들을 모두 Resolve
            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Resolve(parameters[i].ParameterType);
            }

            // 생성자 호출로 인스턴스 생성
            return constructor.Invoke(args);
        }
    }
}
