using System.Configuration;
using MVVMKit.MVVM;

namespace DITest.ViewModels
{
    public class DialogViewModel : NotifyBase
    {
        private string _text = "Dialog";
        public string Text { get => _text; set => SetProperty(ref _text, value); }
    }
}
