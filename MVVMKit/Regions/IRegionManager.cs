using System;
using System.Windows.Controls;

namespace MVVMKit.Regions
{
    public interface IRegionManager
    {
        void Register(string regionName, ContentControl regionControl);
        void RequestNavigate(string regionName, Type viewType);
        void RequestNavigate(string regionName, string viewKey);
    }
}
