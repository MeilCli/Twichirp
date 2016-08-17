// Copyright (c) 2016 meil
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
using Twichirp.Android.App.Extensions;
using Twichirp.Android.App.View.Activity;
using Android.Support.V7.Widget;

namespace Twichirp.Android.App.Setting {
    public class MainSettingFragment : PreferenceFragmentCompat {

        public override void OnCreatePreferences(Bundle savedInstanceState,string rootKey) {
            var screen = PreferenceManager.CreatePreferenceScreen(PreferenceManager.Context);
            var application = Context.ToTwichirpApplication();
            var settingManager = application.SettingManager;
            new PreferenceCategory(screen.Context).Apply(category => {
                screen.AddPreference(category);
                category.Title = Context.GetString(Resource.String.SettingGeneral);
                new Preference(screen.Context).Apply(x => {
                    category.AddPreference(x);
                    x.SetTitle(Resource.String.SettingAccount);
                    x.PreferenceClick += (s,e) => SettingActivity.StartAccounts(Activity);
                    x.SetIcon(Resource.Drawable.IconPeopleGrey36dp);
                });
                new Preference(screen.Context).Apply(x => {
                    category.AddPreference(x);
                    x.SetTitle(Resource.String.SettingTimeline);
                    x.PreferenceClick += (s,e) => SettingActivity.StartTimeline(Activity);
                    x.SetIcon(Resource.Drawable.IconDashboardGrey36dp);
                });
                new Preference(screen.Context).Apply(x => {
                    category.AddPreference(x);
                    x.SetTitle(Resource.String.SettingApplication);
                    x.PreferenceClick += (s,e) => SettingActivity.StartApplication(Activity);
                    x.SetIcon(Resource.Drawable.IconAndroidGrey36dp);
                });
            });
            new PreferenceCategory(screen.Context).Apply(category => {
                screen.AddPreference(category);
                category.Title = Context.GetString(Resource.String.SettingOther);
                new Preference(screen.Context).Apply(x => {
                    category.AddPreference(x);
                    x.SetTitle(Resource.String.License);
                    x.PreferenceClick += (s,e) => Activity.StartActivityCompat(typeof(LicenseActivity));
                });
                new Preference(screen.Context).Apply(x => {
                    category.AddPreference(x);
                    x.SetTitle(Resource.String.OpenSource);
                    x.PreferenceClick += (s,e) => SettingActivity.StartOpenSource(Activity);
                });
            });

            PreferenceScreen = screen;
        }

    }

}
