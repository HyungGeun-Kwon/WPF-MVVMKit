using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DITest.DI
{
    /// <summary>
    /// 의존성 주입(DI) 컨테이너
    /// - Singleton, Transient, Instance 등록 지원
    /// - 생성자 주입 기반 자동 생성 지원
    /// </summary>
    public class Container
    {
        // 타입별 생성자 함수 등록
        private readonly Dictionary<Type, Func<object>> _registrations = new Dictionary<Type, Func<object>>();

        // 싱글톤 인스턴스 보관소 (한 번만 생성된 객체 저장)
        private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();

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
        /// 구현체만 주어졌을 때 자기 자신을 싱글톤으로 등록
        /// </summary>
        public void RegisterSingleton<TImplementation>() where TImplementation : class
            => RegisterSingleton<TImplementation, TImplementation>();

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
        public object Resolve(Type type)
        {
            // RegisterInstance 또는 Singleton으로 등록된 경우
            if (_singletons.ContainsKey(type))
            {
                return _singletons[type];
            }

            // 등록된 생성자 팩토리가 있는 경우
            if (_registrations.ContainsKey(type))
            {
                return _registrations[type]();
            }

            // 등록되지 않은 타입을 자동으로 생성하려면 아래 코드 주석 해제
            // return CreateWithConstructor(type);

            throw new InvalidOperationException($"Type {type.FullName} is not registered.");
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

            // 생성자 파라미터들을 모두 Resolve (재귀 호출)
            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Resolve(parameters[i].ParameterType);
            }

            // 생성자 호출로 인스턴스 생성
            return constructor.Invoke(args);
        }
    }
}
