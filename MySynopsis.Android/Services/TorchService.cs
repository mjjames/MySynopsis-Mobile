using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySynopsis.BusinessLogic.Services;
using Xamarin.Forms;
using Android.Content.PM;
using Android.Hardware;
using Graphics = Android.Graphics;

[assembly: Dependency(typeof(MySynopsis.Android.Services.TorchService))]
namespace MySynopsis.Android.Services
{

    public class TorchService : Java.Lang.Object, ITorchService
    {
        private Camera _torch;
        private bool _disposed;
        private IList<string> _supportedFlashModes;
        private Camera.Parameters _params;
        public TorchService()
        {
            if (!Forms.Context.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFlash))
            {
                _torch = null;
            }

            if (TryTorchReconnect())
            {
                _params = _torch.GetParameters();
            }
        }
        public bool IsTorchAvailable
        {
            get { return TryTorchReconnect(); }
        }

        public TorchStatus Status
        {
            get
            {
                if (!IsTorchAvailable || !TryTorchReconnect())
                {
                    return TorchStatus.Unavailable;
                }
                string flashMode;
                try
                {
                    flashMode = _torch.GetParameters().FlashMode;
                }
                catch
                {
                    flashMode = "";
                }
                return (string.IsNullOrWhiteSpace(flashMode) || flashMode == Camera.Parameters.FlashModeOff) ? TorchStatus.Off : TorchStatus.On;
            }
        }

        public bool TrySetTorchStatus(TorchStatus status)
        {
            if (status == BusinessLogic.Services.TorchStatus.Unavailable || !IsTorchAvailable)
            {
                return false;
            }
            try
            {
                if (_supportedFlashModes == null)
                {
                    UpdateFlashModes(_params);
                }
                if (!_supportedFlashModes.Any())
                {
                    return false;
                }
                if (status == TorchStatus.On)
                {
                    if (!TryTorchReconnect())
                    {
                        return false;
                    }
                    TorchOn();
                }
                else{
                    TorchOff();
                }
                
                return true;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to set torch status : {0}", ex.ToString());
                return false;
            }
        }

        private void TorchOn()
        {
            var flashMode = GetFlashMode();
            _params.FlashMode = flashMode;
            _torch.SetParameters(_params);
            _torch.SetPreviewTexture(new Graphics.SurfaceTexture(0));
            try
            {
                _torch.AutoFocus(new AutoFocusCallBack());
            }
            catch { }
            _torch.StartPreview();
        }

        private void TorchOff()
        {
            _torch.StopPreview();
            _torch.CancelAutoFocus();
            _params.FlashMode = Camera.Parameters.FlashModeOff;
            _torch.SetParameters(_params);
            _torch.Release();
            _torch = null;
        }

        private bool TryTorchReconnect()
        {
            try
            {
                if (_torch == null)
                {
                    _torch = Camera.Open();
                }
                return true;
            }
            catch
            {
                _torch = null;
                return false;
            }
        }

        private void UpdateFlashModes(Camera.Parameters cameraParams)
        {
            _supportedFlashModes = cameraParams.SupportedFlashModes ?? new List<string>();
        }

        private string GetFlashMode()
        {
            if (_supportedFlashModes.Contains(Camera.Parameters.FlashModeTorch))
            {
                return Camera.Parameters.FlashModeTorch;
            }
            if (_supportedFlashModes.Contains(Camera.Parameters.FlashModeOn))
            {
                return Camera.Parameters.FlashModeOn;
            }
            return Camera.Parameters.FlashModeAuto;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
            {
                return;
            }
            if (_torch != null)
            {
                try
                {
                    _torch.Release();
                    _params.Dispose();
                    _torch.Dispose();

                }
                finally
                {
                    _torch = null;
                    _params = null;
                }
            }
            _disposed = true;
        }

        internal class AutoFocusCallBack : Camera.IAutoFocusCallback
        {

            public void OnAutoFocus(bool success, Camera camera)
            {

            }

            public IntPtr Handle
            {
                get { return IntPtr.Zero; }
            }

            public void Dispose()
            {

            }
        }
    }
}