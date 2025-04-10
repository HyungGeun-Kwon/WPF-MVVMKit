using System;

namespace MVVMKit.Regions
{
    public interface IRegionManager
    {
        void RequestNavigate(string regionName, Type viewType);
        void RequestNavigate(string regionName, string viewKey);
    }
}
