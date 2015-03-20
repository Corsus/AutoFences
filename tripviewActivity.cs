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

        protected override void OnCreate (Bundle bundle){
            base.OnCreate (bundle);
            string latlng = Intent.GetStringExtra ("MyData") ?? "Data not available";
            Console.WriteLine ("Location {0}", latlng);
            SetContentView (Resource.Layout.tripStatus);           

            currentLocation = GetCurrentLocation ();
            InitMapFragment ();


        }

        protected override void OnResume() {
            base.OnResume();
            SetupMapIfNeeded();

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

        private void SetupMapIfNeeded()
        {
            if (_map == null)
            {
                _map = _mapFragment.Map;
                if (_map != null) {
                    MarkerOptions marker1 = new MarkerOptions();
                    marker1.SetPosition(currentLocation);
                    _map.AddMarker(marker1);

                    // We create an instance of CameraUpdate, and move the map to it.
                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(currentLocation, 15);
                    _map.MoveCamera(cameraUpdate);

                }
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
