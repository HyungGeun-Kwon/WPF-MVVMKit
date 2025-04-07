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
        private readonly IContainerProvider _container;

        public RegionManager(IContainerProvider container)
        {
            _container = container;
        }

        public void Register(string regionName, ContentControl regionControl)
        {
            if (!_regions.ContainsKey(regionName))
            {
                _regions[regionName] = regionControl;
            }
        }
        public void RequestNavigate(string regionName, string viewKey)
        {
            RequestNavigate(regionName, _container.Resolve(viewKey));
        }
        public void RequestNavigate(string regionName, Type viewType)
        {
            RequestNavigate(regionName, _container.Resolve(viewType));
        }
        private void RequestNavigate(string regionName, object view)
        {
            var viewType = view.GetType();
            if (!_regions.TryGetValue(regionName, out var target))
                throw new ArgumentException($"Region '{regionName}' not found.");

            // 현재 View/ViewModel이 INavigationAware면 OnNavigatedFrom 호출
            if (target.Content is FrameworkElement currentElement)
            {
                if (currentElement.DataContext is INavigationAware currentAware)
                {
                    currentAware.OnNavigatedFrom();
                }
            }

            if (!(view is FrameworkElement fe))
            {
                throw new InvalidOperationException($"Resolved view '{viewType.FullName}' is not a FrameworkElement.");
            }

            if (ViewModelLocator.IsMapping(viewType))
            {
                var vmType = ViewModelLocator.GetViewModelTypeForView(viewType);
                object viewModel = _container.Resolve(vmType);
                fe.DataContext = viewModel;

                if (viewModel is INavigationAware aware)
                {
                    aware.OnNavigatedTo();
                }
            }

            target.Content = view;
        }
    }
}
