using System.Windows.Input;
using MVVMKit.Commands;
using MVVMKit.Dialogs;
using MVVMKit.MVVM;
using MVVMKit.Regions;
using MVVMKitSample.UI.Core.Names;

namespace MVVMKitSample.ViewModels
{
    public class MainViewModel : NotifyBase
    {
        private int _aDialogShowCount = 0;
        private bool _leftRegionIsAView = true;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;

        public ICommand LoadedCommand { get; }
        public ICommand ShowADialogCommand { get; }
        public ICommand SwitchViewCommand { get; }

        public MainViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _dialogService = dialogService;

            LoadedCommand = new BindingCommand(OnLoaded);
            ShowADialogCommand = new BindingCommand(OnShowADialog);
            SwitchViewCommand = new BindingCommand(OnSwitchViewCommand);

        }

        private void OnSwitchViewCommand()
        {
            if (_leftRegionIsAView)
            {
                RegionSetting(ViewNames.BView, ViewNames.AView);
            }
            else
            {
                RegionSetting(ViewNames.AView, ViewNames.BView);
            }
            _leftRegionIsAView = !_leftRegionIsAView;
        }

        private void OnLoaded()
        {
            RegionSetting(ViewNames.AView, ViewNames.BView);
            _leftRegionIsAView = true;
        }
        private void RegionSetting(string leftRegionName, string rightRegionName)
        {
            _regionManager.RequestNavigate(RegionNames.LeftRegion, leftRegionName);
            _regionManager.RequestNavigate(RegionNames.RightRegion, rightRegionName);
        }
        private void OnShowADialog()
        {
            _aDialogShowCount++;
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("ShowCount", _aDialogShowCount);
            _dialogService.Show(ViewNames.ADialog, dialogParameters);
        }
    }
}
