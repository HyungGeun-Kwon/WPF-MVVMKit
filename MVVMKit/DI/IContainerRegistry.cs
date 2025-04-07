using System.Windows;
using MVVMKit.Regions;

namespace MVVMKit.DI
{
    public interface IContainerRegistry
    {
        void RegisterSingleton<TImplementation>(string key) where TImplementation : class;
        
        void RegisterSingleton<TInterface, TImplementation>(string key) where TImplementation : class, TInterface;

        void RegisterTransient<TInterface, TImplementation>(string key) where TImplementation : class, TInterface;

        void RegisterSingleton<TInterface, TImplementation>()
            where TImplementation : class, TInterface;

        void RegisterSingleton<TImplementation>() where TImplementation : class;

        void RegisterInstance<TInterface>(TInterface instance);

        void RegisterTransient<TInterface, TImplementation>()
            where TImplementation : class, TInterface;

        void RegisterForNavigation<TView, TViewModel>(string viewKey = null, string viewModelKey = null)
            where TView : FrameworkElement
            where TViewModel : class, INavigationAware;
    }
}
