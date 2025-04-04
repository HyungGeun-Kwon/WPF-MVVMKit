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
            IDialogService dialogService = new DialogService(Container);
            Container.RegisterInstance(dialogService);

            // 싱글톤 등록
            Container.RegisterSingleton<ICameraService, CameraService>();
            Container.RegisterSingleton<IIOService, IOService>();
            Container.RegisterSingleton<InspectionManager>();
            Container.RegisterSingleton<MainViewModel>();

            // View ViewModel 연결
            ViewModelLocator.WireViewViewModel<MainWindow, MainViewModel>();
            ViewModelLocator.WireViewViewModel<DialogView, DialogViewModel>();
            ViewModelLocator.WireViewViewModel<UserControlView, UserControlViewModel>();

            // DialogView
            dialogService.Register<DialogView>();
            dialogService.Register<UserControlView>();

            base.OnStartup(e);

            // 시작.
            MainWindow = Container.Resolve<MainWindow>();
            MainWindow.Show();
        }
    }
}
