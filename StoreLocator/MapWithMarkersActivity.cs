using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
//using Android.Gms.Maps.GoogleMap.InfoWindowAdapter;
using Android.OS;
using Android.Widget;
using Android.Util;
using Android.Locations;
//using Android.Gms.Location;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
//using Android.Support.V4.App;

namespace StoreLocator
{
    [Activity(Label = "@string/activity_label_mapwithmarkers")]
    //public class MapWithMarkersActivity : Activity, GoogleApiClient.IConnectionCallbacks,
    //    GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    public class MapWithMarkersActivity : Activity
    {

        private static readonly LatLng Passchendaele = new LatLng(50.897778, 3.013333);
        private static readonly LatLng VimyRidge = new LatLng(50.379444, 2.773611);
        private static LatLng StartLocation;
        private GoogleMap _map;
        private MapFragment _mapFragment;
        TradeService.TradeService rvc = new TradeService.TradeService();
        private double latitude;
        private double longitude;
        private string provider;
        String bestProvider;
        double lat, lon;

        private DataSet StoreData { get; set; }
        //GoogleApiClient apiClient;
        //LocationRequest locRequest;



        private void GetStoreLocations(double Latitude, double Longitude)
        {
            DataSet ds;
            //DataTable dt;
            try
            {
                ds = rvc.RetailLocatorLatLong("Generic", Latitude, Longitude, "20");
                StoreData = ds;
                //dt = ds.Tables[0];
                //if (!IsEmpty(ds))
                //{
                //    foreach (DataRow row in dt.Rows)
                //    {
                //        dtDate = (DateTime)row["BIRTH_DATE"];
                //    }
                //}

            }
            catch (Exception ex)
            {
                throw new Exception("GetStoreLocations Exception: " + ex.ToString());
            }
            //return ds;
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.MapLayout);
            var app = (MyApp)this.Application;
            if (app.Lat == 0)
            {
                app.Lat = 36.09330;
                app.Lon = -80.25250;
            }
            lat = app.Lat;
            lon = app.Lon;
            if (!(app.StoreData == null))
                StoreData = app.StoreData;
            latitude = app.Lat;
            longitude = app.Lon;
            InitMapFragment();
            SetupMapIfNeeded();
        }
        private void InitMapFragment()
        {
            _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeHybrid)
                    .InvokeZoomControlsEnabled(false)
                    .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.mapWithOverlay, _mapFragment, "map");
                fragTx.Commit();
            }
        }
        private void SetupMapIfNeeded()
        {
            if (_map == null)
            {
                _mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
                _map = _mapFragment.Map;

                //var frag = FragmentManager.FindFragmentByTag("map") as MapFragment;
                //var mapReadyCallback = new OnMapReadyClass();

                //mapReadyCallback.MapReadyAction += delegate (GoogleMap googleMap)
                //{
                //    _map = googleMap;
                //};

                //frag.GetMapAsync(mapReadyCallback);
                //_map = frag.Map;


                if (_map != null)
                {
                    //apiClient.Connect();
                    //if (apiClient.IsConnected)
                    //{
                    //    Location location = LocationServices.FusedLocationApi.GetLastLocation(apiClient);
                    //    if (location != null)
                    //    {
                    //        latitude = location.Latitude;
                    //        longitude = location.Longitude;
                    //        provider = location.Provider;
                    //        Log.Debug("LocationClient", "Last location printed");
                    //    }
                    //}
                    //loc = _locMgr.GetLastKnownLocation();
                    StartLocation = new LatLng(latitude, longitude);

                    GetStoreLocations(latitude, longitude);
                    var dt = StoreData.Tables[0];
                    if (!IsEmpty(StoreData))
                    {
                        LatLng StoreLocation;
                        double StoreLat;
                        double StoreLong;
                        int i = 0;
                        StringBuilder sb;
                        foreach (DataRow row in dt.Rows)
                        {
                            StoreLat = Convert.ToDouble(row["LAT"].ToString());
                            StoreLong = Convert.ToDouble(row["LONG"].ToString());
                            StoreLocation = new LatLng(StoreLat, StoreLong);
                            i++;
                            sb = new StringBuilder();
                            sb.Append("Address: ");
                            sb.Append(row["ADDRESS1"].ToString());
                            //sb.Append(" " + row["ADDRESS2"].ToString());
                            //sb.Append(" " + row["CITY"].ToString());
                            //sb.Append(" " + row["STATE"].ToString());
                            //sb.Append(" " + row["ZIPCODE"].ToString());
                            sb.Append("; Sells: ");
                            //Set USA Gold
                            string strUSAG = "USA Gold";
                            sb.Append(strUSAG);
                            //Set Winston
                            string strWinston = "Winston";
                            if (i % 3 == 0)
                            {
                                sb.Append(", ");
                                sb.Append(strWinston);
                            }
                            //Set Kool
                            string strKool = "Kool";
                            if (i % 2 == 0)
                            {
                                sb.Append(", ");
                                sb.Append(strKool);
                            }
                            //Set Salem
                            string strSalem = "Salem";
                            if (i % 5 == 0)
                            {
                                sb.Append(", ");
                                sb.Append(strSalem);
                            }
                            //CustomMarkerPopupAdapter ma = new CustomMarkerPopupAdapter();
                            BitmapDescriptor icon = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan);
                            MarkerOptions markerOptions = new MarkerOptions()
                                .SetPosition(StoreLocation)
                                .SetIcon(icon)
                                .SetSnippet(sb.ToString())
                                .SetTitle(row["storeName"].ToString());

                            _map.AddMarker(markerOptions);
                            //_map.SetInfoWindowAdapter(new CustomMarkerPopupAdapter(LayoutInflater));
                        }
                    }
                        //MarkerOptions markerOpt1 = new MarkerOptions();
                        //markerOpt1.SetPosition(VimyRidge);
                        //markerOpt1.SetTitle("Vimy Ridge");
                        //markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
                        //_map.AddMarker(markerOpt1);

                        //MarkerOptions markerOpt2 = new MarkerOptions();
                        //markerOpt2.SetPosition(Passchendaele);
                        //markerOpt2.SetTitle("Passchendaele");
                        //_map.AddMarker(markerOpt2);

                        // We create an instance of CameraUpdate, and move the map to it.
                        CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(StartLocation, 15);
                    _map.MoveCamera(cameraUpdate);
                }
            }
        }
        protected override void OnPause()
        {
            base.OnPause();

            _map.MyLocationEnabled = false;
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (MapIsSetup())
            {
                // Enable the my-location layer.
                _map.MyLocationEnabled = true;
                _map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(lat, lon), 15));
                // Setup a handler for when the user clicks on a marker.
               // _map.MarkerClick += MapOnMarkerClick;
            }

        }
        private bool MapIsSetup()
        {
            if (_map == null)
            {
                //var fragment = SupportFragmentManager.FindFragmentByTag("map") as SupportMapFragment;
                var fragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);

                if (fragment != null)
                {
                    _map = fragment.Map;
                }
            }
            return _map != null;
        }

        string FormatAddress(Address addr)
        {
            StringBuilder addressText = new StringBuilder();
            addressText.Append(addr.SubThoroughfare);
            addressText.AppendFormat(" {0}", addr.Thoroughfare);
            addressText.AppendFormat(", {0}", addr.Locality);
            addressText.AppendFormat(", {0}", addr.CountryCode);
            addressText.AppendLine();
            addressText.AppendLine();
            return addressText.ToString();
        }
        protected bool IsEmpty(DataSet dataSet)
        {
            foreach (DataTable Table in dataSet.Tables)
            {
                return false;
            }
            return true;
        }

    }
    public class OnMapReadyClass : Java.Lang.Object, IOnMapReadyCallback
    {
        public GoogleMap Map { get; private set; }
        public event Action<GoogleMap> MapReadyAction;

        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;

            if (MapReadyAction != null)
                MapReadyAction(Map);
        }
    }
    //public class CustomMarkerPopupAdapter : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
    //{
    //    private LayoutInflater _layoutInflater = null;

    //    public CustomMarkerPopupAdapter(LayoutInflater inflater)
    //    {
    //        _layoutInflater = inflater;
    //    }

    //    public View GetInfoWindow(Marker marker)
    //    {
    //        return null;
    //    }

    //    public View GetInfoContents(Marker marker)
    //    {
    //        var customPopup = _layoutInflater.Inflate(Resource.Layout.CustomMarkerPopup, null);

    //        var titleTextView = customPopup.FindViewById<TextView>(Resource.Id.custom_marker_popup_title);
    //        if (titleTextView != null)
    //        {
    //            titleTextView.Text = marker.Title;
    //        }

    //        var snippetTextView = customPopup.FindViewById<TextView>(Resource.Id.custom_marker_popup_snippet);
    //        if (snippetTextView != null)
    //        {
    //            snippetTextView.Text = marker.Snippet;
    //        }

    //        return customPopup;
    //    }
    //}
}