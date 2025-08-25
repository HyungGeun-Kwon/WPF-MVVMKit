using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MVVMKit.DI;
using MVVMKit.MVVM;

namespace MVVMKit.Regions
{
    public class RegionManager : IRegionManager
    {
        private readonly Dictionary<string, ContentControl> _regions = new Dictionary<string, ContentControl>();
        private readonly Container _container;

        public RegionManager(Container container)
        {
            _container = container;
        }

        internal void RegisterRegionName(string regionName, ContentControl regionControl)
        {
            if (_regions.ContainsKey(regionName))
            {
                _regions.Remove(regionName);
            }
            _regions[regionName] = regionControl;
        }
        public void RequestNavigate(string regionName, string viewKey)
        {
            Type viewType = _container.GetNavigationViewType(viewKey);
            RequestNavigate(regionName, viewType);
        }
        public void RequestNavigate(string regionName, Type viewType)
        {
            if (!_regions.TryGetValue(regionName, out var target))
                throw new ArgumentException($"Region '{regionName}' not found.");


            // 현재 View/ViewModel이 INavigationAware면 OnNavigatedFrom 호출
            if (target.Content is FrameworkElement currentElement)
            {
                if (currentElement.DataContext != null && currentElement.DataContext is INavigationAware currentAware)
                {
                    currentAware.OnNavigatedFrom();
                }
            }

            var view = _container.Resolve(viewType);

            if (!(view is FrameworkElement fe))
            {
                throw new InvalidOperationException($"Resolved view '{viewType.FullName}' is not a FrameworkElement.");
            }

            if (ViewModelLocator.IsMapping(viewType))
            {
                Type viewModelType = ViewModelLocator.GetViewModelTypeForView(viewType);
                object viewModel = _container.Resolve(viewModelType);
                fe.DataContext = viewModel;

                if (viewModel is INavigationAware nowAware)
                {
                    nowAware.OnNavigatedTo();
                }
            }

            target.Content = view;
        }
    }
}
