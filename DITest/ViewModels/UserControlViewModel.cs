using MVVMKit.MVVM;

namespace DITest.ViewModels
{
    public class UserControlViewModel : NotifyBase
    {
        private string _text = "UserControlView";
        public string Text { get => _text; set => SetProperty(ref _text, value); }
    }
}
