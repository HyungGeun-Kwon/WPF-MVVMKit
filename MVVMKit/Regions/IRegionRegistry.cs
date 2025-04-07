using System.Windows.Controls;

namespace MVVMKit.Regions
{
    public interface IRegionRegistry
    {
        void Register(string regionName, ContentControl regionControl);
    }
}
