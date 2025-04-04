using System.Windows;

namespace MVVMKit.Dialogs
{
    public interface IDialogService
    {
        void Register<TView>() where TView : FrameworkElement;
        void Register<TView>(string viewName) where TView : FrameworkElement;
        bool ShowDialog(string viewName);
        void Show(string viewName);
    }
}
