using System.Windows;
using DITest.DI;
using DITest.Services;
using DITest.ViewModels;
using DITest.Views;

namespace DITest
{
    public class App : Application
    {
        public static Container Container;
        protected override void OnStartup(StartupEventArgs e)
        {

            Container = new Container();
            Container.RegisterSingleton<ICameraService, CameraService>();
            Container.RegisterSingleton<IIOService, IOService>();
            Container.RegisterSingleton<InspectionManager>();
            Container.RegisterSingleton<MainViewModel>();

            MainWindow = new MainWindow();
            MainWindow.DataContext = Container.Resolve<MainViewModel>();
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
