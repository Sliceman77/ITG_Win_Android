using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Locations;
using System.Data;
using AndroidUri = Android.Net.Uri;

namespace StoreLocator
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ListActivity, ILocationListener
    {
        public static readonly int InstallGooglePlayServicesId = 1000;
        public static readonly string Tag = "StoreLocator";

        private List<StoresActivity> _activities;
        private bool _isGooglePlayServicesInstalled;
        double lat, lon;
        private LocationManager lm;
        String bestProvider;
        MyApp app;
        TradeService.TradeService rvc = new TradeService.TradeService();

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch (resultCode)
            {
                case Result.Ok:
                    // Try again.
                    _isGooglePlayServicesInstalled = true;
                    break;

                default:
                    Log.Debug("MainActivity", "Unknown resultCode {0} for request {1}", resultCode, requestCode);
                    break;
            }
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _isGooglePlayServicesInstalled = TestIfGooglePlayServicesIsInstalled();
            InitializeListView();
            app = (MyApp)this.Application;

            Criteria cr = new Criteria();
            cr.Accuracy = Accuracy.Coarse;
            cr.PowerRequirement = Power.Low;
            cr.AltitudeRequired = false;
            cr.BearingRequired = false;
            cr.SpeedRequired = false;
            cr.CostAllowed = true;
            String serviceString = Context.LocationService;
            lm = (LocationManager)GetSystemService(serviceString);
            bestProvider = lm.GetBestProvider(cr, false);
            Location l = lm.GetLastKnownLocation(bestProvider);

            lm.RequestLocationUpdates(bestProvider, 5000, 10f, this);
        }
        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            //if (position == 0)
            //{
            //    AndroidUri geoUri = AndroidUri.Parse("geo:42.374260,-71.120824");
            //    Intent mapIntent = new Intent(Intent.ActionView, geoUri);
            //    StartActivity(mapIntent);
            //    return;
            //}

            StoresActivity activity = _activities[position];
            activity.Start(this);
        }
        private void InitializeListView()
        {
            if (_isGooglePlayServicesInstalled)
            {
                _activities = new List<StoresActivity>
                                  {
                                     // new StoresActivity(Resource.String.activity_label_axml, Resource.String.activity_description_axml, typeof(BasicDemoActivity)),
                                      new StoresActivity(Resource.String.activity_label_mapwithmarkers, Resource.String.activity_description_mapwithmarkers, typeof(MapWithMarkersActivity)),
                                      //new StoresActivity(Resource.String.activity_label_mapwithoverlays, Resource.String.activity_description_mapwithoverlays, typeof(MapWithOverlaysActivity))
                                  };

                ListAdapter = new StoreLocatorActivityAdaptor(this, _activities);
            }
            else
            {
                Log.Error("MainActivity", "Google Play Services is not installed");
                ListAdapter = new StoreLocatorActivityAdaptor(this, null);
            }
        }

        private bool TestIfGooglePlayServicesIsInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info(Tag, "Google Play Services is installed on this device.");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error(Tag, "There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString);
                Dialog errorDialog = GoogleApiAvailability.Instance.GetErrorDialog(this, queryResult, InstallGooglePlayServicesId);
                ErrorDialogFragment dialogFrag = new ErrorDialogFragment(errorDialog);

                dialogFrag.Show(FragmentManager, "GooglePlayServicesDialog");
            }
            return false;
        }

        public void OnLocationChanged(Location location)
        {
            lat = location.Latitude;
            lon = location.Longitude;
            app.Lat = lat;
            app.Lon = lon;
            GetStoreLocations(lat, lon);

        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, int status, Bundle extras)
        {
            //throw new NotImplementedException();
        }



        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }
        private void GetStoreLocations(double Latitude, double Longitude)
        {
            DataSet ds;
            //DataTable dt;
            try
            {
                ds = rvc.RetailLocatorLatLong("Generic", Latitude, Longitude, "20");
                app.StoreData = ds;
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

    }
}

