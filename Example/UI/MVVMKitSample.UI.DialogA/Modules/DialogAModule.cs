using MVVMKit.DI;
using MVVMKit.Modules;
using MVVMKitSample.UI.Core.Names;
using MVVMKitSample.UI.DialogA.ViewModels;
using MVVMKitSample.UI.DialogA.Views;

namespace MVVMKitSample.UI.DialogA.Modules
{
    public class DialogAModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<ADialog, ADialogViewModel>(ViewNames.ADialog);
        }
    }
}
