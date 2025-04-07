using System.Windows.Input;
using MVVMKit.Commands;
using MVVMKit.Dialogs;
using MVVMKit.MVVM;
using MVVMKit.Regions;

namespace MVVMKitSample.ViewModels
{
    public class MainViewModel : NotifyBase
    {
        private int _aDialogShowCount = 0;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;

        public ICommand LoadedCommand { get; }
        public ICommand ShowADialogCommand { get; }

        public MainViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _dialogService = dialogService;

            LoadedCommand = new BindingCommand(OnLoaded);
            ShowADialogCommand = new BindingCommand(OnShowADialog);
        }

        private void OnLoaded()
        {
            _regionManager.RequestNavigate("RegionA", "AView");
            _regionManager.RequestNavigate("RegionB", "BView");
        }
        private void OnShowADialog()
        {
            _aDialogShowCount++;
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("ShowCount", _aDialogShowCount);
            _dialogService.Show("ADialog", dialogParameters);
        }
    }
}
