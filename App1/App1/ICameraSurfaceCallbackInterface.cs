using System;


using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Util;
using Android.Graphics;
using Android.Support.V4.App;
using Android;
using Xamarin.Essentials;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace App1
{
    public class ICameraSurfaceCallbackInterface : Java.Lang.Object, ISurfaceHolderCallback
    {
        private CameraSource CamSrc;
        public ICameraSurfaceCallbackInterface(CameraSource CS)
        {
            CamSrc = CS;
        }

        public IntPtr Handle;

        public void Dispose()
        {
            
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            CamSrc.Start();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            CamSrc.Stop();
        }
    }
}