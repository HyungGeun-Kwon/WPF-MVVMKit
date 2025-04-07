using System;

namespace MVVMKit.DI
{
    public interface IContainerProvider
    {
        object Resolve(string key);
        T Resolve<T>(string key);
        TInterface Resolve<TInterface>();
        object Resolve(Type type, bool autoWireRegisteredViewModel = true);
    }
}
