using MVVMKitSample.Application.ServiceA;

namespace MVVMKitSample.Infrastructure.AService
{
    public class ServiceA : IServiceA
    {
        public string ServiceAText { get; set; } = "Shared Text";
    }
}
