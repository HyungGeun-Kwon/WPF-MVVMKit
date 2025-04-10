using MVVMKitSample.Application.ServiceA;

namespace MVVMKitSample.Infrastructure.ServiceA
{
    public class ServiceA : IServiceA
    {
        public string ServiceAText { get; set; } = "Shared Text";
    }
}
