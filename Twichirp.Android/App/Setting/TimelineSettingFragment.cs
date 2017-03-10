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
using Twichirp.Core.App.Setting;

namespace Twichirp.Android.App.Setting {

    public class TimelineSettingFragment : PreferenceFragmentCompat {

        public override void OnCreatePreferences(Bundle savedInstanceState,string rootKey) {
            var screen = PreferenceManager.CreatePreferenceScreen(PreferenceManager.Context);
            var application = Context.ToTwichirpApplication();
            var settingManager = application.Resolve<SettingManager>();
            new ListPreference(screen.Context).Apply(x => {
                screen.AddPreference(x);
                x.SetTitle(Resource.String.SettingTimelineDownloadCount);
                x.SetDialogTitle(Resource.String.SettingTimelineDownloadCount);
                x.Key = x.Title;
                string[] values = new string[] { "20","30","50","100","150","200",settingManager.Timeline.Count.ToString() }.Distinct().ToArray();
                x.SetEntries(values);
                x.SetEntryValues(values);
                x.SetValueIndex(values.ToList().FindIndex(y => y == settingManager.Timeline.Count.ToString()));
                x.PreferenceChange += (s,e) => settingManager.Timeline.Count = int.Parse((string)e.NewValue);
            });
            new ListPreference(screen.Context).Apply(x => {
                screen.AddPreference(x);
                x.SetTitle(Resource.String.SettingTimelineOwnedNumber);
                x.SetDialogTitle(Resource.String.SettingTimelineOwnedNumber);
                x.Key = x.Title;
                string[] values = new string[] { "400","600","800","1000",settingManager.Timeline.OwnedNumber.ToString() }.Distinct().ToArray();
                x.SetEntries(values);
                x.SetEntryValues(values);
                x.SetValueIndex(values.ToList().FindIndex(y => y == settingManager.Timeline.OwnedNumber.ToString()));
                x.PreferenceChange += (s,e) => settingManager.Timeline.OwnedNumber = int.Parse((string)e.NewValue);
            });
            PreferenceScreen = screen;
        }

    }
}