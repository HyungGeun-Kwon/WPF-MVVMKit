using MVVMKit.MVVM;
using MVVMKit.Regions;
using MVVMKitSample.Application.ServiceA;

namespace MVVMKitSample.ViewA.ViewModels
{
    public class AViewModel : NotifyBase, INavigationAware
    {
        public IServiceA ServiceA { get; }

        public AViewModel(IServiceA serviceA)
        {
            ServiceA = serviceA;
        }

        public void OnNavigatedTo()
        {
        }
        public void OnNavigatedFrom()
        {
        }
    }
}
