using System.Windows;

namespace MVVMKit.Dialogs
{
    public interface IDialogService
    {
        void Register<TView>() where TView : Window;
        bool ShowDialog(string viewName);
        void Show(string viewName);
    }
}
