using MVVMKit.DI;
using MVVMKit.Modules;
using MVVMKitSample.UI.Core.Names;
using MVVMKitSample.UI.ViewB.ViewModels;
using MVVMKitSample.UI.ViewB.Views;

namespace MVVMKitSample.UI.ViewB.Modules
{
    public class ViewBModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BView, BViewModel>(ViewNames.BView);
        }
    }
}
