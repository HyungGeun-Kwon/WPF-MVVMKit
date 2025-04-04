using System;
using System.Windows.Input;
using DITest.Services;
using MVVMKit.Commands;
using MVVMKit.Dialogs;

namespace DITest.ViewModels
{
    public class MainViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly InspectionManager _inspectionManager;

        public IIOService IOService { get; }
        public ICameraService CameraService { get; }

        public ICommand BtnStartClickCommand { get; }
        public ICommand BtnStopClickCommand { get; }
        public ICommand BtnDialogViewShowClickCommand { get; }
        public ICommand BtnDialogViewShowDialogClickCommand { get; }
        public ICommand BtnUserControlViewShowClickCommand { get; }
        public MainViewModel(IDialogService dialogService, IIOService ioService, ICameraService cameraService, InspectionManager inspectionManager)
        {
            _dialogService = dialogService;
            IOService = ioService;
            CameraService = cameraService;
            _inspectionManager = inspectionManager;

            BtnStartClickCommand = new BindingCommand(OnBtnStartClick);
            BtnStopClickCommand = new BindingCommand(OnBtnStopClick);
            BtnDialogViewShowClickCommand = new BindingCommand(OnBtnDialogViewShowClick);
            BtnDialogViewShowDialogClickCommand = new BindingCommand(OnBtnDialogViewShowDialogClick);
            BtnUserControlViewShowClickCommand = new BindingCommand(OnBtnUserControlViewShowClick);
        }

        private void OnBtnStartClick()
        {
            _inspectionManager.Start();
        }
        private void OnBtnStopClick()
        {
            _inspectionManager.Stop();
        }
        private void OnBtnDialogViewShowClick()
        {
            _dialogService.Show("DialogView");
        }
        private void OnBtnDialogViewShowDialogClick()
        {
            _dialogService.ShowDialog("DialogView");
        }
        private void OnBtnUserControlViewShowClick()
        {
            _dialogService.Show("UserControlView");
        }
    }
}
