using System.Windows;
using MVVMKit.Dialogs;
using MVVMKit.Regions;

namespace MVVMKit.DI
{
    public interface IContainerRegistry
    {
        /// <summary>
        /// 지정한 인터페이스/구현체 타입을 타입 기반 싱글톤으로 등록.
        /// </summary>
        void RegisterSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface;
        /// <summary>
        /// 지정한 인터페이스/구현체 타입을 키 기반 싱글톤으로 등록.
        /// </summary>
        void RegisterSingleton<TInterface, TImplementation>(string key) where TImplementation : class, TInterface;
        /// <summary>
        /// 지정한 구현체 타입을 타입 기반 싱글톤으로 등록.
        /// </summary>
        void RegisterSingleton<TImplementation>() where TImplementation : class;
        /// <summary>
        /// 지정한 구현체 타입을 키 기반 싱글톤으로 등록.
        /// </summary>
        void RegisterSingleton<TImplementation>(string key) where TImplementation : class;

        /// <summary>
        /// 외부에서 생성한 인스턴스를 직접 타입 기반 싱글톤으로 등록
        /// </summary>
        void RegisterInstance<T>(T instance) where T : class;
        /// <summary>
        /// 외부에서 생성한 인스턴스를 직접 타입 + 키 기반 싱글톤으로 등록
        /// </summary>
        void RegisterInstance<T>(T instance, string key) where T : class;

        /// <summary>
        /// 지정한 인터페이스/구현체를 타입 기반 트랜지언트(매번 새로 생성) 방식으로 등록.
        /// </summary>
        void RegisterTransient<TInterface, TImplementation>() where TImplementation : class, TInterface;
        /// <summary>
        /// 지정한 인터페이스/구현체를 타입 + 키 기반 트랜지언트(매번 새로 생성) 방식으로 등록.
        /// </summary>
        void RegisterTransient<TInterface, TImplementation>(string key) where TImplementation : class, TInterface;
        /// <summary>
        /// 지정한 구현체를 타입 기반 트랜지언트(매번 새로 생성) 방식으로 등록.
        /// </summary>
        void RegisterTransient<T>() where T : class;
        /// <summary>
        /// 지정한 구현체를 타입 + 키 기반 트랜지언트(매번 새로 생성) 방식으로 등록.
        /// </summary>
        void RegisterTransient<T>(string key) where T : class;

        /// <summary>
        /// 지정한 View를 Navigation용으로 Transient 방식 등록
        /// key 기본값 = 클래스명 (typeof(TView).Name)
        /// </summary>
        void RegisterForNavigation<TView>(string key = null) where TView : FrameworkElement;
        /// <summary>
        /// 지정한 View를 Navigation용으로 등록하고 ViewModelLocator에게 View와 ViewModel을 Transient 방식으로 등록함
        /// 만약 ViewModel을 SingleTon으로 사용을 희망한다면 RegisterSingleton()으로 따로 등록
        /// key 기본값 = 클래스명 (typeof(TView).Name)
        /// </summary>
        void RegisterForNavigation<TView, TViewModel>(string key = null) where TView : FrameworkElement where TViewModel : INavigationAware;

        /// <summary>
        /// 지정한 View를 Dialog용으로 Transient 방식 등록
        /// key 기본값 = 클래스명 (typeof(TView).Name)
        /// </summary>
        void RegisterDialog<TView>(string key = null) where TView : FrameworkElement;
        /// <summary>
        /// 지정한 View를 Dialog용으로 등록하고 ViewModelLocator에게 View와 ViewModel을 Transient 방식으로 등록함
        /// 만약 ViewModel을 SingleTon으로 사용을 희망한다면 RegisterSingleton()으로 따로 등록
        /// key 기본값 = 클래스명 (typeof(TView).Name)
        /// </summary>
        void RegisterDialog<TView, TViewModel>(string key = null) where TView : FrameworkElement where TViewModel : IDialogAware;
    }
}
