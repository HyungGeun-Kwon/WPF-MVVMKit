using MVVMKit.DI;
using MVVMKit.Modules;
using MVVMKitSample.UI.Core.Names;
using MVVMKitSample.UI.ViewA.ViewModels;
using MVVMKitSample.UI.ViewA.Views;

namespace MVVMKitSample.UI.ViewA.Modules
{
    public class ViewAModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AView, AViewModel>(ViewNames.AView);
            //containerRegistry.RegisterSingleton<AViewModel>(); // ViewModel을 한번만 생성
        }
    }
}
