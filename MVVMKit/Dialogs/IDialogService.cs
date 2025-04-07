namespace MVVMKit.Dialogs
{
    public interface IDialogService
    {
        bool ShowDialog(string viewName, DialogParameters parameters = null);
        void Show(string viewName, DialogParameters parameters = null);
    }
}
