using System;
using System.Windows;
using MVVMKit.DI;
using MVVMKit.MVVM;

namespace MVVMKit.Dialogs
{
    public class DialogService : IDialogService
    {
        private readonly Container _container;

        public DialogService(Container container)
        {
            _container = container;
        }

        public bool ShowDialog(string viewKey, DialogParameters parameters = null)
        {
            var window = CreateWindow(viewKey, parameters);
            if (window.DataContext is IDialogAware aware)
            {
                AttachCloseHandler(window, aware);
            }
            bool? result = window.ShowDialog();
            return result ?? false;
        }

        public void Show(string viewKey, DialogParameters parameters = null)
        {
            var window = CreateWindow(viewKey, parameters);
            if (window.DataContext is IDialogAware aware)
            {
                AttachCloseHandler(window, aware);
            }
            window.Show(); // 비모달
        }

        private void AttachCloseHandler(Window window, IDialogAware aware)
        {
            void Handler(object s, EventArgs e)
            {
                window.Closed -= Handler;
                aware.OnDialogClosed();
            }

            window.Closed += Handler;
        }

        private Window CreateWindow(string viewKey, DialogParameters parameters)
        {
            // ViewModel 자동 연결 Off로 View Resolve
            // ViewModel이 IDialogAware인 경우 parameters를 전달해주기 위함
            Type viewType = _container.GetDialogViewType(viewKey);
            var view = _container.Resolve(viewType);

            Window window;
            if (view is Window w)
            {
                window = w;
            }
            else if (view is FrameworkElement fe) // UserControl, Page 등
            {
                var wrapper = new Window
                {
                    Content = fe,
                    Width = fe.Width > 0 ? fe.Width : 400,
                    Height = fe.Height > 0 ? fe.Height : 300,
                    Title = viewKey,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                window = wrapper;
            }
            else
            {
                throw new InvalidOperationException($"Resolved view '{viewKey}' is not a FrameworkElement. Actual type: {view.GetType().FullName}");
            }

            // ViewModel이 ViewModelLocator에 연결되어있다면 DataContext 연결
            if (ViewModelLocator.IsMapping(viewType))
            {
                Type viewModelType = ViewModelLocator.GetViewModelTypeForView(viewType);
                object viewModel = _container.Resolve(viewModelType);
                window.DataContext = viewModel;

                // ViewModel이 IDialogAware라면 parameters 전달
                if (viewModel is IDialogAware dialogAware)
                {
                    dialogAware.OnDialogOpened(parameters ?? new DialogParameters());
                }
            }

            return window;
        }
    }
}
