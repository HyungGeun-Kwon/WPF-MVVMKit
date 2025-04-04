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

        public void Register<TView>() where TView : FrameworkElement
        {
            Register<TView>(typeof(TView).Name);
        }

        public void Register<TView>(string viewName) where TView : FrameworkElement
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

            var view = _container.Resolve(viewType); // Resolve 할 때 ViewModel이 연결되어있다면 자동 연결됨
            if (view is Window window)
            {
                return window;
            }

            if (view is FrameworkElement fe) // UserControl, Page 등
            {
                var wrapper = new Window
                {
                    Content = fe,
                    Width = fe.Width > 0 ? fe.Width : 400,
                    Height = fe.Height > 0 ? fe.Height : 300,
                    Title = viewName,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                return wrapper;
            }

            throw new InvalidOperationException($"Resolved view '{viewName}' is not a FrameworkElement. Actual type: {view.GetType().FullName}");
        }
    }
}
