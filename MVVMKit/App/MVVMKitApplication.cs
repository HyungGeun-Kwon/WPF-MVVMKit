using System.Windows;
using MVVMKit.DI;
using MVVMKit.Dialogs;
using MVVMKit.Event;
using MVVMKit.Modules;
using MVVMKit.Regions;

namespace MVVMKit.App
{
    public abstract class MVVMKitApplication : Application
    {
        public IContainerProvider Container;
        private IContainerRegistry _containerRegistry;
        private IFrameworkContainerProvider _frameworkContainerProvider;

        protected abstract Window CreateShell(IFrameworkContainerProvider frameworkContainerProvider);

        protected abstract void RegisterTypes(IContainerRegistry containerRegistry);

        protected abstract void AddModule(IModuleCatalog moduleCatalog);

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
            ContainerDefaultSetting();

            RegisterTypes(_containerRegistry);

            RegisterModules();

            SettingMainWindow();
        }

        private void ContainerDefaultSetting()
        {
            var container = new Container();
            Container = container;
            _containerRegistry = container;
            _frameworkContainerProvider = container;

            var dialogService = new DialogService(container);
            var regionManager = new RegionManager(container);
            _containerRegistry.RegisterInstance<IDialogService>(dialogService);
            _containerRegistry.RegisterInstance<IRegionManager>(regionManager);
            _containerRegistry.RegisterInstance<IContainerProvider>(container);
            _containerRegistry.RegisterInstance<IFrameworkContainerProvider>(container);

            _containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
        }
        private void RegisterModules()
        {
            IModuleCatalog moduleCatalog = new ModuleCatalog();
            AddModule(moduleCatalog);
            moduleCatalog.InitializeModules(_containerRegistry, Container);
        }
        private void SettingMainWindow()
        {
            var shell = CreateShell(_frameworkContainerProvider);
            if (shell != null)
            {
                MainWindow = shell;
            }
        }
        private void OnInitialized()
        {
            MainWindow?.Show();
        }
    }
}
