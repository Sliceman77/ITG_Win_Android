using System.Collections.Generic;
using System.Linq;

using Android.Content;
using Android.Views;
using Android.Widget;
using System;

namespace StoreLocator
{
    internal class StoreLocatorActivityAdaptor : BaseAdapter<StoresActivity>
    {
        private readonly List<StoresActivity> _activities;
        private readonly Context _context;

        public StoreLocatorActivityAdaptor(Context context, IEnumerable<StoresActivity> sampleActivities)
        {
            _context = context;
            if (sampleActivities == null)
            {
                _activities = new List<StoresActivity>(0);
            }
            else
            {
                _activities = sampleActivities.ToList();
            }
        }
        public override int Count { get { return _activities.Count; } }

        public override StoresActivity this[int position] { get { return _activities[position]; } }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            FeatureRowHolder row = convertView as FeatureRowHolder ?? new FeatureRowHolder(_context);
            StoresActivity sample = _activities[position];

            row.UpdateFrom(sample);
            return row;
        }


    }
}