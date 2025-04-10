using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using MVVMKit.Dialogs;
using MVVMKit.MVVM;
using MVVMKit.Regions;

namespace MVVMKit.DI
{
    /// <summary>
    /// 의존성 주입(DI) 컨테이너
    /// - Singleton, Transient, Instance 등록 지원
    /// - 생성자 주입 기반 자동 생성 지원
    /// </summary>
    public class Container : IContainerProvider, IContainerRegistry, IFrameworkContainerProvider
    {
        // 타입별 생성자 함수 등록
        private readonly Dictionary<Type, Func<object>> _registrations = new Dictionary<Type, Func<object>>();

        // 싱글톤 인스턴스 보관소 (한 번만 생성된 객체 저장)
        private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();

        // string key 기반 타입별 생성자 함수 등록
        private readonly Dictionary<(Type, string), Func<object>> _namedRegistrations = new Dictionary<(Type, string), Func<object>>();

        // strign key 기반 싱글톤 인스턴스 보관소 (한 번만 생성된 객체 저장)
        private readonly Dictionary<(Type, string), object> _namedSingletons = new Dictionary<(Type, string), object>();


        // RegionManager에서 사용하는 NavigationView 매핑
        private readonly Dictionary<string, Type> _namedNavigationViews = new Dictionary<string, Type>();

        // DialogService에서 사용하는 DialogView 매핑
        private readonly Dictionary<string, Type> _namedDialogs = new Dictionary<string, Type>();

        /// <summary>
        /// 지정한 인터페이스/구현체 타입을 타입 기반 싱글톤으로 등록.
        /// </summary>
        public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            _registrations[typeof(TInterface)] = () =>
            {
                if (!_singletons.ContainsKey(typeof(TInterface)))
                {
                    object instance = CreateWithConstructor(typeof(TImplementation));
                    _singletons[typeof(TInterface)] = instance;
                }
                return _singletons[typeof(TInterface)];
            };
        }

        /// <summary>
        /// 지정한 인터페이스/구현체 타입을 타입 + 키 기반 싱글톤으로 등록.
        /// </summary>
        public void RegisterSingleton<TInterface, TImplementation>(string key) where TImplementation : class, TInterface
        {
            _namedRegistrations[(typeof(TInterface), key)] = () =>
            {
                if (!_namedSingletons.ContainsKey((typeof(TInterface), key)))
                {
                    _namedSingletons[(typeof(TInterface), key)] = CreateWithConstructor(typeof(TImplementation));
                }
                return _namedSingletons[(typeof(TInterface), key)];
            };
        }

        /// <summary>
        /// 지정한 구현체 타입을 타입 기반 싱글톤으로 등록.
        /// </summary>
        public void RegisterSingleton<TImplementation>() where TImplementation : class
        {
            RegisterSingleton<TImplementation, TImplementation>();
        }

        /// <summary>
        /// 지정한 구현체 타입을 타입 + 키 기반 싱글톤으로 등록.
        /// </summary>
        public void RegisterSingleton<TImplementation>(string key) where TImplementation : class
        {
            RegisterSingleton<TImplementation, TImplementation>(key);
        }

        /// <summary>
        /// 외부에서 생성한 인스턴스를 직접 타입 기반 싱글톤으로 등록
        /// </summary>
        public void RegisterInstance<T>(T instance) where T : class
        {
            _singletons[typeof(T)] = instance;
        }

        /// <summary>
        /// 외부에서 생성한 인스턴스를 직접 타입 + 키 기반 싱글톤으로 등록
        /// </summary>
        public void RegisterInstance<T>(T instance, string key) where T : class
        {
            _namedSingletons[(typeof(T), key)] = instance;
        }

        /// <summary>
        /// 지정한 인터페이스/구현체를 타입 기반 트랜지언트(매번 새로 생성) 방식으로 등록.
        /// </summary>
        public void RegisterTransient<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            _registrations[typeof(TInterface)] = () =>
            {
                // 매번 Resolve할 때마다 새 인스턴스 생성
                return CreateWithConstructor(typeof(TImplementation));
            };
        }

        /// <summary>
        /// 지정한 인터페이스/구현체를 타입 + 키 기반 트랜지언트(매번 새로 생성) 방식으로 등록.
        /// </summary>
        public void RegisterTransient<TInterface, TImplementation>(string key) where TImplementation : class, TInterface
        {
            _namedRegistrations[(typeof(TInterface), key)] = () =>
            {
                return CreateWithConstructor(typeof(TImplementation));
            };
        }

        /// <summary>
        /// 지정한 구현체를 타입 기반 트랜지언트(매번 새로 생성) 방식으로 등록.
        /// </summary>
        public void RegisterTransient<T>() where T : class
        {
            RegisterTransient<T, T>();
        }

        /// <summary>
        /// 지정한 구현체를 타입 + 키 기반 트랜지언트(매번 새로 생성) 방식으로 등록.
        /// </summary>
        public void RegisterTransient<T>(string key) where T : class
        {
            RegisterTransient<T, T>(key);
        }

        /// <summary>
        /// 지정한 View를 Navigation용으로 Transient 방식 등록
        /// key 기본값 = 클래스명 (typeof(TView).Name)
        /// </summary>
        public void RegisterForNavigation<TView>(string key = null) where TView : FrameworkElement
        {
            var viewType = typeof(TView);
            if (key == null) key = viewType.Name;
            _namedNavigationViews[key] = viewType;
            RegisterTransient<TView>();
        }

        /// <summary>
        /// 지정한 View를 Navigation용으로 등록하고 ViewModelLocator에게 View와 ViewModel을 Transient 방식으로 등록함
        /// key 기본값 = 클래스명 (typeof(TView).Name)
        /// </summary>
        public void RegisterForNavigation<TView, TViewModel>(string key = null)
            where TView : FrameworkElement
            where TViewModel : INavigationAware
        {
            RegisterForNavigation<TView>(key);
            ViewModelLocator.WireViewViewModel<TView, TViewModel>();
        }

        /// <summary>
        /// 지정한 View를 Dialog용으로 Transient 방식 등록
        /// key 기본값 = 클래스명 (typeof(TView).Name)
        /// </summary>
        public void RegisterDialog<TView>(string key = null) where TView : FrameworkElement
        {
            var viewType = typeof(TView);
            if (key == null) key = viewType.Name;
            _namedDialogs[key] = viewType;
            RegisterTransient<TView>();
        }

        /// <summary>
        /// 지정한 View를 Dialog용으로 등록하고 ViewModelLocator에게 View와 ViewModel을 Transient 방식으로 등록함
        /// 만약 ViewModel을 SingleTon으로 사용을 희망한다면 RegisterSingleton()으로 따로 등록
        /// key 기본값 = 클래스명 (typeof(TView).Name)
        /// </summary>
        public void RegisterDialog<TView, TViewModel>(string key = null)
            where TView : FrameworkElement
            where TViewModel : IDialogAware
        {
            RegisterDialog<TView>(key);
            ViewModelLocator.WireViewViewModel<TView, TViewModel>();
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
        /// <summary>
        /// 타입 기반으로 인스턴스를 Resolve (등록 안되어 있으면 생성자 기반 생성)
        /// </summary>
        public object Resolve(Type type)
        {
            if (_singletons.ContainsKey(type)) return _singletons[type];
            if (_registrations.ContainsKey(type)) return _registrations[type]();
            return CreateWithConstructor(type);
        }
        /// <summary>
        /// 타입 + 키 기반으로 인스턴스를 Resolve (등록 안되어 있으면 예외)
        /// </summary>
        public object Resolve(Type type, string key)
        {
            if (_namedSingletons.ContainsKey((type, key))) return _namedSingletons[(type, key)];
            if (_namedRegistrations.ContainsKey((type, key))) return _namedRegistrations[(type, key)]();
            throw new InvalidOperationException($"Type {type.FullName} with key '{key}' is not registered.");
        }

        /// <summary>
        /// 타입 기반으로 인스턴스를 Resolve (등록 안되어 있으면 생성자 기반 생성)
        /// </summary>
        public T Resolve<T>() => (T)Resolve(typeof(T));

        /// <summary>
        /// 타입 + 키 기반으로 인스턴스를 Resolve (등록 안되어 있으면 예외)
        /// </summary>
        public T Resolve<T>(string key) => (T)Resolve(typeof(T), key);

        internal Type GetNavigationViewType(string key)
        {
            if (_namedNavigationViews.TryGetValue(key, out Type navType))
                return navType;

            throw new InvalidOperationException($"'{key}' is not registered as navigation view.");
        }

        internal Type GetDialogViewType(string key)
        {
            if (_namedDialogs.TryGetValue(key, out Type dlgType))
                return dlgType;

            throw new InvalidOperationException($"'{key}' is not registered as dialog view.");
        }

        /// <summary>
        /// View와 ViewModel을 함께 Resolve (튜플로 반환)
        /// View - ViewModel이 ViewModelLocator에 정의되어있지 않으면 ViewModel을 연결하지 않고 viewModel을 null로 반환
        /// </summary>
        public (TFrameworkElement frameworkElement, object viewModel) ResolveWithViewModel<TFrameworkElement>()
            where TFrameworkElement : FrameworkElement
        {
            var type = typeof(TFrameworkElement);
            (FrameworkElement frameworkElement, object viewModel) reVal = ResolveWithViewModel(type);

            return ((TFrameworkElement)reVal.frameworkElement, reVal.viewModel);
        }

        /// <summary>
        /// type은 반드시 FrameworkElement여야함.
        /// View와 ViewModel을 함께 Resolve (튜플로 반환)
        /// View - ViewModel이 ViewModelLocator에 정의되어있지 않으면 ViewModel을 연결하지 않고 viewModel을 null로 반환
        /// </summary>
        public (FrameworkElement frameworkElement, object viewModel) ResolveWithViewModel(Type type)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(type))
                throw new InvalidOperationException($"Type '{type.FullName}' must inherit from FrameworkElement.");

            var resolved = Resolve(type);

            if (!(resolved is FrameworkElement fe))
                throw new InvalidOperationException($"Resolved instance is not a FrameworkElement: {resolved.GetType().FullName}");

            object vm = null;

            if (ViewModelLocator.IsMapping(type))
            {
                Type vmType = ViewModelLocator.GetViewModelTypeForView(type);
                vm = Resolve(vmType);
                fe.DataContext = vm;
            }

            return (fe, vm); // ViewModel이 없으면 null 반환
        }

        /// <summary>
        /// View만 Resolve하고 ViewModel을 자동 연결 (DataContext에만 주입)
        /// View - ViewModel이 ViewModelLocator에 정의되어있지 않으면 ViewModel을 연결하지 않음
        /// </summary>
        public TFrameworkElement ResolveFrameworkElement<TFrameworkElement>()
            where TFrameworkElement : FrameworkElement
        {
            var type = typeof(TFrameworkElement);
            var fe = Resolve<TFrameworkElement>();

            if (ViewModelLocator.IsMapping(type))
            {
                Type vmType = ViewModelLocator.GetViewModelTypeForView(type);
                object vm = Resolve(vmType);
                fe.DataContext = vm;
            }

            return fe;
        }

        /// <summary>
        /// View만 Resolve하고 ViewModel을 자동 연결 (DataContext에만 주입)
        /// View - ViewModel이 ViewModelLocator에 정의되어있지 않으면 ViewModel을 연결하지 않음
        /// </summary>
        public FrameworkElement ResolveFrameworkElement(Type type)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(type))
                throw new InvalidOperationException($"Type '{type.FullName}' must inherit from FrameworkElement.");

            var resolved = Resolve(type);

            if (!(resolved is FrameworkElement fe))
                throw new InvalidOperationException($"Resolved instance is not a FrameworkElement: {resolved.GetType().FullName}");

            if (ViewModelLocator.IsMapping(type))
            {
                Type vmType = ViewModelLocator.GetViewModelTypeForView(type);
                object vm = Resolve(vmType);
                fe.DataContext = vm;
            }

            return fe;
        }
    }
}
