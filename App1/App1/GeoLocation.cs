using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Timers;

using Xamarin.Essentials;

namespace App1
{
    public class GeoLocation
    {
        public Location Location { get; set; } = new Location();
        private GeolocationAccuracy acc;

        public GeoLocation(GeolocationAccuracy accuracy)
        {
            acc = accuracy;
        }

        public async void UpdateLocation()
        {
            var GRequest = new GeolocationRequest(GeolocationAccuracy.Best);
            Location = await Geolocation.GetLocationAsync(GRequest);
        }
    }

    public interface IGeolocationInterface
    {

    }
}