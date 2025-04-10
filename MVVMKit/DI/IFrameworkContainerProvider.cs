using System.Windows;

namespace MVVMKit.DI
{
    public interface IFrameworkContainerProvider
    {
        /// <summary>
        /// View와 ViewModel을 함께 Resolve (튜플로 반환)
        /// View - ViewModel이 ViewModelLocator에 정의되어있지 않으면 ViewModel을 연결하지 않고 viewModel을 null로 반환
        /// </summary>
        (TFrameworkElement frameworkElement, object viewModel) ResolveWithViewModel<TFrameworkElement>()
            where TFrameworkElement : FrameworkElement;

        /// <summary>
        /// View만 Resolve하고 ViewModel을 자동 연결 (DataContext에만 주입)
        /// View - ViewModel이 ViewModelLocator에 정의되어있지 않으면 ViewModel을 연결하지 않음
        /// </summary>
        TFrameworkElement ResolveFrameworkElement<TFrameworkElement>()
            where TFrameworkElement : FrameworkElement;
    }
}
