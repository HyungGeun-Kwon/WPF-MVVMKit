using System;

namespace MVVMKit.DI
{
    public interface IContainerProvider
    {

        /// <summary>
        /// 타입 기반으로 인스턴스를 Resolve (등록 안되어 있으면 생성자 기반 생성)
        /// </summary>
        T Resolve<T>();
        /// <summary>
        /// 타입 + 키 기반으로 인스턴스를 Resolve (등록 안되어 있으면 예외)
        /// </summary>
        T Resolve<T>(string key);
        /// <summary>
        /// 타입 기반으로 인스턴스를 Resolve (등록 안되어 있으면 생성자 기반 생성)
        /// </summary>
        object Resolve(Type type);
        /// <summary>
        /// 타입 + 키 기반으로 인스턴스를 Resolve (등록 안되어 있으면 예외)
        /// </summary>
        object Resolve(Type type, string key);
    }
}
