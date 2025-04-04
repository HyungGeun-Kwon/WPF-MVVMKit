using MVVMKit.Dialogs;
using MVVMKit.MVVM;

namespace DITest.ViewModels
{
    public class DialogViewModel : NotifyBase, IDialogAware
    {
        private string _text = "Dialog";
        public string Text { get => _text; set => SetProperty(ref _text, value); }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(DialogParameters parameters)
        {
        }
    }
}
