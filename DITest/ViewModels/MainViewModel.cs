using System.Windows.Input;
using DITest.MVVMHelper;
using DITest.Services;

namespace DITest.ViewModels
{
    public class MainViewModel
    {
        private readonly InspectionManager _inspectionManager;

        public IIOService IOService { get; }
        public ICameraService CameraService { get; }

        public ICommand BtnStartClickCommand { get; }
        public ICommand BtnStopClickCommand { get; }


        public MainViewModel(IIOService ioService, ICameraService cameraService, InspectionManager inspectionManager)
        {
            IOService = ioService;
            CameraService = cameraService;
            _inspectionManager = inspectionManager;

            BtnStartClickCommand = new BindingCommand(OnBtnStartClick);
            BtnStopClickCommand = new BindingCommand(OnBtnStopClick);
        }

        private void OnBtnStartClick()
        {
            _inspectionManager.Start();
        }
        private void OnBtnStopClick()
        {
            _inspectionManager.Stop();
        }
    }
}
