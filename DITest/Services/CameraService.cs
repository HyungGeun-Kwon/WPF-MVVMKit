using MVVMKit.MVVM;

namespace DITest.Services
{
    public interface ICameraService
    {
        string CameraServiceTestText { get; set; }
    }
    public class CameraService : NotifyBase, ICameraService
    {
        private string _cameraServiceTestText = "Camera";
        public string CameraServiceTestText { get => _cameraServiceTestText; set => SetProperty(ref _cameraServiceTestText, value); }
    }
}
