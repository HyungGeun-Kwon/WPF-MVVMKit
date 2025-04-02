using DITest.MVVMHelper;

namespace DITest.Services
{
    public interface IIOService
    {
        string IOServiceTestText { get; set; }
    }
    public class IOService : NotifyBase, IIOService
    {
        private string _ioServiceTestText = "IO";
        public string IOServiceTestText { get => _ioServiceTestText; set => SetProperty(ref _ioServiceTestText, value); }
    }
}
