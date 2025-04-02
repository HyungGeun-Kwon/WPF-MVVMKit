namespace DITest.Services
{

    public class InspectionManager
    {
        private IIOService _ioService;
        private ICameraService _cameraService;
        public InspectionManager(IIOService ioService, ICameraService cameraService)
        {
            _ioService = ioService;
            _cameraService = cameraService;
        }

        public void Start()
        {
            _ioService.IOServiceTestText = "Start IO";
            _cameraService.CameraServiceTestText = "Start Camera";
        }

        public void Stop()
        {
            _ioService.IOServiceTestText = "Stop IO";
            _cameraService.CameraServiceTestText = "Stop Camera";
        }
    }
}
