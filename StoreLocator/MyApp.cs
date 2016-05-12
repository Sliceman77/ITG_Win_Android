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
using System.Data;
namespace StoreLocator
{
    [Application()]
    public class MyApp : Application
    {
        public double Lat;
        public double Lon;
        public DataSet StoreData;
        public override void OnCreate()
        {
            base.OnCreate();
        }
        public MyApp(IntPtr a, JniHandleOwnership b) : base(a, b) { }
    }
}