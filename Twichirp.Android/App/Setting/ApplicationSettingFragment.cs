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
using Twichirp.Android.App.Extensions;
using Twichirp.Core.App.Setting;

namespace Twichirp.Android.App.Setting {

    public class ApplicationSettingFragment : PreferenceFragmentCompat {

        public override void OnCreatePreferences(Bundle savedInstanceState,string rootKey) {
            var screen = PreferenceManager.CreatePreferenceScreen(PreferenceManager.Context);
            var application = Context.ToTwichirpApplication();
            var settingManager = application.Resolve<SettingManager>();
            new CheckBoxPreference(screen.Context).Apply(x => {
                screen.AddPreference(x);
                x.Checked = settingManager.Applications.IsCleanLaunch;
                x.PreferenceChange += (s,e) => settingManager.Applications.IsCleanLaunch = (bool)e.NewValue;
                x.SetTitle(Resource.String.SettingCleanUp);
                x.SetSummary(Resource.String.SettingCleanUpSummary);
            });
            PreferenceScreen = screen;
        }

    }
}