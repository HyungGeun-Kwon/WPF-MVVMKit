using System.Windows;

namespace MVVMKit.Dialogs
{
    public interface IDialogRegistry
    {
        void Register<TView>() where TView : FrameworkElement;
        void Register<TView>(string viewName) where TView : FrameworkElement;
    }
}
