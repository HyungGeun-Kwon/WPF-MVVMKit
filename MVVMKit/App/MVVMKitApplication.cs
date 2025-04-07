using System.Windows;
using MVVMKit.DI;
using MVVMKit.Dialogs;
using MVVMKit.Regions;

namespace MVVMKit.App
{
    public abstract class MVVMKitApplication : Application
    {
        public IContainerProvider Container;
        private IContainerRegistry _containerRegistry;
        private IDialogRegistry _dialogRegistry;

        protected abstract Window CreateShell();

        protected abstract void RegisterTypes(IContainerRegistry containerRegistry);

        protected abstract void RegisterDialogs(IDialogRegistry dialogRegistry);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeInternal();
        }

        private void InitializeInternal()
        {
            Initialize();
            OnInitialized();
        }
        private void Initialize()
        {
            var container = new Container();
            Container = container;
            _containerRegistry = container;
            ConfigureContainerDefault();
            RegisterTypes(_containerRegistry);
            RegisterDialogs(_dialogRegistry);

            var shell = CreateShell();
            if (shell != null)
            {
                MainWindow = shell;
            }
        }
        private void OnInitialized()
        {
            MainWindow?.Show();
        }

        private void ConfigureContainerDefault()
        {
            var dialogService = new DialogService(Container);
            var regionManager = new RegionManager(Container);

            _containerRegistry.RegisterInstance<IDialogService>(dialogService);
            _containerRegistry.RegisterInstance<IRegionManager>(regionManager);
            _dialogRegistry = dialogService;
        }
    }
}
