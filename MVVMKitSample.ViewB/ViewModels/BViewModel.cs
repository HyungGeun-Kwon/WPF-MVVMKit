using MVVMKit.MVVM;
using MVVMKit.Regions;
using MVVMKitSample.Application.ServiceA;

namespace MVVMKitSample.ViewB.ViewModels
{
    public class BViewModel : NotifyBase, INavigationAware
    {
        public IServiceA ServiceA { get; }

        public BViewModel(IServiceA serviceA)
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
