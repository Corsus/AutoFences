using System.Text;
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Mojio;
using Mojio.Client;
using Mojio.Events;

namespace AutoFences
{
    [Activity (Label = "Trip Status")]            
    public class tripviewActivity : Activity, ILocationListener
    {
        private GoogleMap _map;
        private MapFragment _mapFragment;
        private LocationManager locMgr;
        private LatLng currentLocation;
        private LatLng markerLocation; 


        protected override void OnCreate (Bundle bundle){
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.tripStatus);
            TextView end = FindViewById<TextView> (Resource.Id.endTime);
            TextView speed = FindViewById<TextView> (Resource.Id.maxSpeed);
            currentLocation = GetCurrentLocation(); 
            Bundle extras = Intent.GetBundleExtra ("extras");
            String lat = extras.GetString ("lat") ?? "Latitude not available";     
            String lng = extras.GetString ("lng") ?? "Longitude not available";
            String startTime = extras.GetString ("startTime") ?? "Start time not available";
            String startDate = extras.GetString ("startDate") ?? "Start date not available";
            String maxSpeed = extras.GetString ("maxSpeed") ?? "Max speed not available";
            String endDateTime = extras.GetString("endTime") ?? "Time Not Available";


            end.Text = endDateTime;
            speed.Text = "Maximum Speed: " + maxSpeed;
            markerLocation = new LatLng (Convert.ToDouble(lat), Convert.ToDouble(lng));
            Console.WriteLine ("Location {0} , {1}", lat, lng);
            InitMapFragment ();

        }

        protected override void OnResume() {
            base.OnResume();
            SetMarker ();
        }
           
        private void MapOnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
        {
            markerClickEventArgs.Handled = true;
            Marker marker = markerClickEventArgs.Marker;

                _map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(20.72110, -156.44776), 13));
            
                Toast.MakeText(this, String.Format("You clicked on Marker ID {0}", marker.Id), ToastLength.Short).Show();
            
        }

        private void InitMapFragment()
        {
            _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
                    .InvokeZoomControlsEnabled(false)
                    .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();

            }
        }

        private LatLng GetCurrentLocation() { 

            Criteria locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Medium;

            var service = (LocationManager)GetSystemService(LocationService); 
            var provider = service.GetBestProvider(locationCriteria, true); 
            var location = service.GetLastKnownLocation(provider);            

            if (provider != null) {
                service.RequestLocationUpdates (provider, 2000, 1, this);
            } else {
                Toast.MakeText (this, "No location providers available", ToastLength.Short).Show ();
            }

            return new LatLng(location.Latitude, location.Longitude);          
        }

        private void SetMarker()
        {
            if (_map == null)
            {
                _map = _mapFragment.Map;
            }
            if (_map != null) {
                MarkerOptions marker1 = new MarkerOptions();
                marker1.SetPosition(markerLocation);
                _map.AddMarker(marker1);
                // We create an instance of CameraUpdate, and move the map to it.
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(markerLocation, 15);
                _map.MoveCamera(cameraUpdate);

            }
        }
        public void OnProviderEnabled (string provider)
        {
            //
        }

        public void OnProviderDisabled (string provider)
        {

        }
        public void OnStatusChanged (string provider, Availability status, Bundle extras)
        {

        }
        public void OnLocationChanged (Android.Locations.Location location)
        {
        }

    }
}
