using System.Windows;
using MVVMKit.App;
using MVVMKit.DI;
using MVVMKit.Dialogs;
using MVVMKit.MVVM;
using MVVMKitSample.Application.ServiceA;
using MVVMKitSample.DialogA.ViewModels;
using MVVMKitSample.DialogA.Views;
using MVVMKitSample.Infrastructure.AService;
using MVVMKitSample.ViewA.ViewModels;
using MVVMKitSample.ViewA.Views;
using MVVMKitSample.ViewB.ViewModels;
using MVVMKitSample.ViewB.Views;
using MVVMKitSample.ViewModels;
using MVVMKitSample.Views;

namespace MVVMKitSample
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : MVVMKitApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            WireDataContext();
            base.OnStartup(e);
        }
        private void WireDataContext()
        {
            ViewModelLocator.WireViewViewModel<MainWindow, MainViewModel>();
            ViewModelLocator.WireViewViewModel<ADialog, ADialogViewModel>();
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IServiceA, ServiceA>();

            containerRegistry.RegisterForNavigation<AView, AViewModel>();
            containerRegistry.RegisterForNavigation<BView, BViewModel>();
        }
        protected override void RegisterDialogs(IDialogRegistry dialogRegistry)
        {
            dialogRegistry.Register<ADialog>("ADialog");
        }
    }
}
