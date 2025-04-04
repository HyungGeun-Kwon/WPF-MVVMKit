using System.Windows;
using DITest.Services;
using DITest.ViewModels;
using DITest.Views;
using MVVMKit.DI;
using MVVMKit.Dialogs;
using MVVMKit.Navigation;

namespace DITest
{
    public class App : Application
    {
        public static Container Container;
        protected override void OnStartup(StartupEventArgs e)
        {
            // 기본
            Container = new Container();

            ConfigureContainerDefault();
            ConfigureContainer();
            ConfigureViewModelLocator();
            ConfigureDialogs();

            base.OnStartup(e);

            ShowMainWindow();
        }

        private void ConfigureContainerDefault()
        {
            IDialogService dialogService = new DialogService(Container);
            Container.RegisterInstance(dialogService);
        }

        private void ConfigureContainer()
        {
            Container.RegisterSingleton<ICameraService, CameraService>();
            Container.RegisterSingleton<IIOService, IOService>();
            Container.RegisterSingleton<InspectionManager>();
            Container.RegisterSingleton<MainViewModel>();
            Container.RegisterSingleton<UserControlView>();
        }

        private void ConfigureViewModelLocator()
        {
            ViewModelLocator.WireViewViewModel<MainWindow, MainViewModel>();
            ViewModelLocator.WireViewViewModel<DialogView, DialogViewModel>();
            ViewModelLocator.WireViewViewModel<UserControlView, UserControlViewModel>();
        }

        private void ConfigureDialogs()
        {
            var dialogService = Container.Resolve<IDialogService>();
            dialogService.Register<DialogView>();
            dialogService.Register<UserControlView>();
        }

        private void ShowMainWindow()
        {
            MainWindow = Container.Resolve<MainWindow>();
            MainWindow.Show();
        }
    }
}
