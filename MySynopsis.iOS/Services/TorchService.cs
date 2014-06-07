using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
using MySynopsis.BusinessLogic.Services;
using MySynopsis.iOS.Services;
using System;
using System.Linq;
using Xamarin.Forms;


[assembly: Dependency(typeof(TorchService))]
namespace MySynopsis.iOS.Services
{

    public class TorchService : ITorchService
    {
        public TorchService()
        {
            _torch = AVCaptureDevice.Devices.FirstOrDefault(d => d.TorchAvailable);
        }

        private AVCaptureDevice _torch;
        private bool _disposed;
        public bool IsTorchAvailable
        {
            get { return _torch != null; }
        }

        public TorchStatus Status
        {
            get {
                if (!IsTorchAvailable)
                {
                    return TorchStatus.Unavailable;
                }
                return _torch.TorchActive ? TorchStatus.On : TorchStatus.Off;
            }
        }

        public bool TrySetTorchStatus(TorchStatus status)
        {
            if (!IsTorchAvailable)
            {
                System.Diagnostics.Debug.WriteLine("Unable to Set Torch Status, Torch unavailable");
                return false;
            }
            NSError error;
            if (!_torch.LockForConfiguration(out error))
            {
                System.Diagnostics.Debug.WriteLine("Unable to Set Torch Status, Torch appears locked : {0}", error.LocalizedDescription);
                return false;
            }
            if (status == TorchStatus.Unavailable)
            {
                System.Diagnostics.Debug.WriteLine("Unable to Set Torch Status, Invalid status provided");
                return false;
            }
            try
            {
                _torch.TorchMode = status == TorchStatus.On ? AVCaptureTorchMode.On : AVCaptureTorchMode.Off;
                return true;
            }
            finally
            {
                _torch.UnlockForConfiguration();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
            {
                return;
            }
            if (_torch != null)
            {
                _torch.Dispose();
                _torch = null;
            }
            _disposed = true;
        }
    }
}