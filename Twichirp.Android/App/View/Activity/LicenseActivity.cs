// Copyright (c) 2016-2017 meil
//
// This file is part of Twichirp.
// 
// Twichirp is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Twichirp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with Twichirp.  If not, see <http://www.gnu.org/licenses/>.
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
using Android.Support.V7.App;
using SToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Webkit;
using Twichirp.Core.Constants;
using Twichirp.Android.Constants;

namespace Twichirp.Android.App.View.Activity {

    [Activity(Label = "@string/License")]
    public class LicenseActivity : AppCompatActivity {

        private WebView web;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            base.SetContentView(Resource.Layout.LicenseActivity);
            base.SetSupportActionBar(FindViewById<SToolbar>(Resource.Id.Toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            web = FindViewById<WebView>(Resource.Id.Web);
            
            var license = LibraryConstant.Libraries.Concat(AndroidLibraryConstant.Libraries).OrderBy(x => x.LibraryName).ToList();
            var liceseText = license.Select(x => $"<ul><li>{x.LibraryName}</li></ul><pre>{x.ToNoticeText()}</pre>");
            string htmlStart = "<html><head><style> body { font - family: sans - serif;}pre { background - color: #eeeeee; padding: 1em; white-space: pre-wrap; } </style></head><body><h3> Notices for files:</h3>";
            string htmlEnd = "</body></html>";
            string html = $"{htmlStart}{string.Join("",liceseText)}{htmlEnd}";
            web.LoadData(html.Replace("\n","<br/>"),"text/html","");
        }

        public override bool OnOptionsItemSelected(IMenuItem item) {
            if(item.ItemId == global::Android.Resource.Id.Home) {
                Finish();
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume() {
            base.OnResume();
            web.OnResume();
        }

        protected override void OnPause() {
            base.OnPause();
            web.OnPause();
        }

    }
}