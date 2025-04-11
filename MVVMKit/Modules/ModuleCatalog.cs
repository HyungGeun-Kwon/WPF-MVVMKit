using System.Collections.Generic;
using MVVMKit.DI;

namespace MVVMKit.Modules
{
    public class ModuleCatalog : IModuleCatalog
    {
        private readonly List<IModule> _modules = new List<IModule>();

        public void AddModule<T>() where T : IModule, new()
        {
            _modules.Add(new T());
        }

        public void InitializeModules(IContainerRegistry registry, IContainerProvider provider)
        {
            foreach (var module in _modules)
            {
                module.RegisterTypes(registry);
            }

            foreach (var module in _modules)
            {
                module.OnInitialized(provider);
            }
        }
    }
}
