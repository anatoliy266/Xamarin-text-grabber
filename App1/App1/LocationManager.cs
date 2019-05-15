using System;
using System.IO;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Util;
using Android.Graphics;
using Android.Support.V4.App;
using Android;

//Location libs
using Android.Gms.Location;
using Android.Gms.Common;
using Android.Locations;
using Android.Content;

namespace App1
{
    public class LocationManager
    {
        private FusedLocationProviderClient LocationClient;
        private TextView DebugText;
        Intent LIntent;
        public LocationManager(Activity activity, TextView view)
        {
            LocationClient = new FusedLocationProviderClient(activity);

            LocationCallback LCallback = new LocationCallback();
            LCallback.LocationResult += new EventHandler<LocationCallbackResultEventArgs>(OnLocationResult);

            LocationRequest LRequest = LocationRequest.Create()
                .SetFastestInterval(100)
                .SetPriority(0);

            LocationClient.RequestLocationUpdatesAsync(LRequest, LCallback);

            DebugText = view;
            LIntent = activity.Intent;
            DebugText.Text = "Location manager created";
        }

        private void OnLocationResult(object sender, LocationCallbackResultEventArgs e)
        {
            LocationResult LResult = e.Result;
            Location location = LResult.LastLocation;
            DebugText.Text = Convert.ToString(location.Latitude + "::" + location.Longitude);
        }
    }
}