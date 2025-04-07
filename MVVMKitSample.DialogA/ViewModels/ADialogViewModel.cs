using MVVMKit.Dialogs;
using MVVMKit.MVVM;

namespace MVVMKitSample.DialogA.ViewModels
{
    public class ADialogViewModel : NotifyBase, IDialogAware
    {
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
