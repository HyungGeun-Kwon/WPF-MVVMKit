using System;
using System.Collections.Generic;
using System.Windows;
using MVVMKit.DI;
using MVVMKit.Navigation;

namespace MVVMKit.Dialogs
{
    public class DialogService : IDialogService
    {
        private readonly Dictionary<string, Type> _viewMap = new Dictionary<string, Type>();
        private readonly Container _container;

        public DialogService(Container container)
        {
            _container = container;
        }

        public void Register<TView>() where TView : Window
        {
            Register<TView>(typeof(TView).Name);
        }

        public void Register<TView>(string viewName) where TView : Window
        {
            _viewMap[viewName] = typeof(TView);
        }

        public bool ShowDialog(string viewName)
        {
            var view = CreateWindow(viewName);
            bool? result = view.ShowDialog();
            return result ?? false;
        }

        public void Show(string viewName)
        {
            var view = CreateWindow(viewName);
            view.Show(); // 비모달
        }

        private Window CreateWindow(string viewName)
        {
            if (!_viewMap.TryGetValue(viewName, out Type viewType))
                throw new ArgumentException($"'{viewName}' is not registered.");

            var view = _container.Resolve(viewType);
            if (view is Window window)
            {
                return window;
            }

            throw new InvalidOperationException($"Resolved view '{viewName}' is not a Window. Actual type: {view.GetType().FullName}");
        }
    }
}
