using MVVMKit.DI;

namespace MVVMKit.Modules
{
    public interface IModule
    {
        void OnInitialized(IContainerProvider containerProvider);
        void RegisterTypes(IContainerRegistry containerRegistry);
    }
}
