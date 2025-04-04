using MVVMKit.Dialogs;
using MVVMKit.MVVM;

namespace DITest.ViewModels
{
    public class UserControlViewModel : NotifyBase, IDialogAware
    {
        private string _text = "UserControlView";
        public string Text { get => _text; set => SetProperty(ref _text, value); }

        private int _showCount;
        public int ShowCount { get => _showCount; set => SetProperty(ref _showCount, value); }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(DialogParameters parameters)
        {
            ShowCount = parameters.GetValue<int>("ShowCount");
        }
    }
}
