namespace MVVMKit.Regions
{
    public interface INavigationAware
    {
        /// <summary>
        /// View가 해당 Region으로 전환되었을 때 호출
        /// </summary>
        void OnNavigatedTo();

        /// <summary>
        /// View가 해당 Region에서 다른 View로 전환될 때 호출
        /// </summary>
        void OnNavigatedFrom();
    }
}
