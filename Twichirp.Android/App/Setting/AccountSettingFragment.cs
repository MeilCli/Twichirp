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

namespace Twichirp.Android.App.Setting {

    public class AccountsSettingFragment : PreferenceFragmentCompat {

        public override void OnCreatePreferences(Bundle savedInstanceState,string rootKey) {
            var screen = PreferenceManager.CreatePreferenceScreen(PreferenceManager.Context);
            var application = Context.ToTwichirpApplication();
            var settingManager = application.SettingManager;
            new ListPreference(screen.Context).Apply(x => {
                screen.AddPreference(x);
                x.SetTitle(Android.Resource.String.SettingUseAccount);
                x.SetDialogTitle(Android.Resource.String.SettingUseAccount);
                x.Key = x.Title;
                x.SetEntries(application.AccountManager.Select(y => y.ScreenName).ToArray());
                x.SetEntryValues(application.AccountManager.Select(y => y.ScreenName).ToArray());
                x.SetValueIndex(application.AccountManager.ToList().FindIndex(y => y.Id == settingManager.Accounts.DefaultAccountId));
                x.PreferenceChange += (s,e) => settingManager.Accounts.DefaultAccountId = application.AccountManager.First(y => y.ScreenName == (string)e.NewValue).Id;
            });
            foreach(var account in application.AccountManager) {
                new Preference(screen.Context).Apply(x => {
                    screen.AddPreference(x);
                    x.Title = account.ScreenName;
                    x.SetIcon(Android.Resource.Drawable.IconPersonGrey36dp);
                    if(account.User != null) {
                        x.Summary = account.User.Name;
                    }
                });
            }
            PreferenceScreen = screen;
        }

    }
}