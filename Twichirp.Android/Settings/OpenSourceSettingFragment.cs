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
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Preferences;
using Twichirp.Android.Extensions;
using Twichirp.Core.Settings;
using Twichirp.Android.Views.Activities;

namespace Twichirp.Android.Settings {

    public class OpenSourceSettingFragment : PreferenceFragmentCompat {

        public override void OnCreatePreferences(Bundle savedInstanceState,string rootKey) {
            var screen = PreferenceManager.CreatePreferenceScreen(PreferenceManager.Context);
            var application = Context.ToTwichirpApplication();
            var settingManager = application.Resolve<SettingManager>();
            new Preference(screen.Context).Apply(x => {
                screen.AddPreference(x);
                x.SetTitle(Resource.String.OpenSourceCode);
                x.PreferenceClick += (s,e) => Activity.StartActivity(new Intent(Intent.ActionView,global::Android.Net.Uri.Parse("https://github.com/MeilCli/Twichirp")));
            });
            new Preference(screen.Context).Apply(x => {
                screen.AddPreference(x);
                x.SetTitle(Resource.String.OpenSourceLicense);
                x.PreferenceClick += (s,e) => Activity.StartActivityCompat(typeof(OpenSourceLicenseActivity));
            });
            new Preference(screen.Context).Apply(x => {
                screen.AddPreference(x);
                x.SetTitle(Resource.String.OpenSourceCopyingLesser);
                x.PreferenceClick += (s,e) => Activity.StartActivityCompat(typeof(OpenSourceCopyingLesserActivity));
            });
            new Preference(screen.Context).Apply(x => {
                screen.AddPreference(x);
                x.SetTitle(Resource.String.OpenSourceCopying);
                x.PreferenceClick += (s,e) => Activity.StartActivityCompat(typeof(OpenSourceCopyingActivity));
            });

            PreferenceScreen = screen;
        }

    }
}