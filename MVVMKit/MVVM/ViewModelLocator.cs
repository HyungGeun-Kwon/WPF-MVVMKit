using System;
using System.Collections.Generic;
using System.Windows;

namespace MVVMKit.MVVM
{
    public static class ViewModelLocator
    {
        private static readonly Dictionary<Type, Type> _map = new Dictionary<Type, Type>();

        /// <summary>
        /// View와 ViewModel을 연결 등록
        /// </summary>
        public static void WireViewViewModel<TView, TViewModel>()
            where TView : FrameworkElement
        {
            _map[typeof(TView)] = typeof(TViewModel);
        }

        public static bool IsMapping(Type viewType)
        {
            return _map.ContainsKey(viewType);
        }

        public static Type GetViewModelTypeForView(Type viewType)
        {
            if (!_map.ContainsKey(viewType))
            {
                throw new InvalidOperationException($"No ViewModel registered for {viewType.FullName}");
            }

            return _map[viewType];
        }
    }
}
