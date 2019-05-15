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
using Xamarin.Essentials;
using System.Text;
using static Android.Gms.Vision.Detector;

namespace App1
{
    [Activity(MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ISurfaceHolderCallback, IProcessor
    {
        private SurfaceView CameraView;
        private CameraSource CameraSource;
        GeoLocation geo;
        private TextView DebugText;
        private bool CameraAccesible { get; set; }
        private bool GeolocationAccresible { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //* Find views on form
            CameraView = FindViewById<SurfaceView>(Resource.Id.CameraSpace);
            DebugText = FindViewById<TextView>(Resource.Id.DebugText);

            //* Init camera and detector
            TextRecognizer Recognizer = new TextRecognizer.Builder(ApplicationContext).Build();

            if (!Recognizer.IsOperational)
            {
                DebugText.Text = "Recognizer is not supported";
            } else
            {
                CameraSource = new CameraSource.Builder(ApplicationContext, Recognizer)
                .SetFacing(CameraFacing.Back)
                .SetRequestedPreviewSize(1920, 1080)
                .SetRequestedFps(2.0f)
                .SetAutoFocusEnabled(true)
                .Build();

                //* Apply ISurfaceHolderCallback interfase to finded surface view holder
                
            }
            CameraView.Holder.AddCallback(this);
            Recognizer.SetProcessor(this);
            //* Init geolocation system
            geo = new GeoLocation(GeolocationAccuracy.Best);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            switch(requestCode)
            {
                case 1001:
                    {
                        if (grantResults[Array.IndexOf(permissions, Android.Manifest.Permission.Camera)] == Android.Content.PM.Permission.Granted)
                        {
                            CameraSource.Start(CameraView.Holder);
                            CameraAccesible = true;
                        } else
                        {
                            CameraAccesible = false;
                            DebugText.Text = "Sorry, but your potato phone has no camera :-) ";
                        }

                        if (grantResults[Array.IndexOf(permissions, Android.Manifest.Permission.AccessFineLocation)] == Android.Content.PM.Permission.Granted)
                        {
                            GeolocationAccresible = true;
                            geo.UpdateLocation();
                            DebugText.Text = Convert.ToString(geo.Location.Latitude) + ":::" + Convert.ToString(geo.Location.Longitude) + ":::" + Convert.ToString(geo.Location.Altitude);
                        } else
                        {
                            GeolocationAccresible = false;
                            DebugText.Text = "If you dont want granted Geolocation acess you refuse of half part of this app content :-)";
                        }
                    }
                    break;
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (CheckSelfPermission(Android.Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted 
                || CheckSelfPermission(Android.Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] {
                    Android.Manifest.Permission.Camera,
                    Android.Manifest.Permission.AccessFineLocation
                }, 1001);
                return;
            } else
            {
                CameraSource.Start(CameraView.Holder);
                geo.UpdateLocation();
                DebugText.Text = Convert.ToString(geo.Location.Latitude) + ":::" + Convert.ToString(geo.Location.Longitude) + ":::" + Convert.ToString(geo.Location.Altitude);
            }
            
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            CameraSource.Stop();
        }

        public void ReceiveDetections(Detections detections)
        {
            SparseArray items = detections.DetectedItems;
            if (items.Size() != 0)
            {
                DebugText.Post(() => {
                    StringBuilder strBuilder = new StringBuilder();
                    for (var i = 0; i < items.Size(); i++)
                    {
                        strBuilder.Append(((TextBlock)items.ValueAt(i)).Value);
                        strBuilder.Append("\n");
                    }
                    DebugText.Text = strBuilder.ToString();
                });
            }
        }

        public void Release()
        {
            
        }
    }
}