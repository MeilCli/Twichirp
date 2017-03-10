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
using SFragment = Android.Support.V4.App.Fragment;
using SToolbar = Android.Support.V7.Widget.Toolbar;
using Twichirp.Android.Extensions;

namespace Twichirp.Android.Settings {

    [Activity(Label = "@string/Setting",Theme ="@style/AppTheme.Setting")]
    public class SettingActivity : AppCompatActivity {

        private const string typeKey = "type";
        private const string titleKey = "title";
        private const int main = 1;
        private const int application = 2;
        private const int accounts = 3;
        private const int timeline = 4;
        private const int openSource = 5;

        public static void StartMain(Activity activity) {
            activity.StartActivity(
                new Intent(activity.ApplicationContext,typeof(SettingActivity))
                .Apply(x => {
                    x.PutExtra(typeKey,main);
                    x.PutExtra(titleKey,activity.GetString(Resource.String.Setting));
                })
            );
        }

        public static void StartApplication(Activity activity) {
            activity.StartActivity(
                new Intent(activity.ApplicationContext,typeof(SettingActivity))
                .Apply(x => {
                    x.PutExtra(typeKey,application);
                    x.PutExtra(titleKey,activity.GetString(Resource.String.SettingApplication));
                })
            );
        }

        public static void StartAccounts(Activity activity) {
            activity.StartActivity(
                new Intent(activity.ApplicationContext,typeof(SettingActivity))
                .Apply(x => {
                    x.PutExtra(typeKey,accounts);
                    x.PutExtra(titleKey,activity.GetString(Resource.String.SettingAccount));
                })
            );
        }

        public static void StartTimeline(Activity activity) {
            activity.StartActivity(
                new Intent(activity.ApplicationContext,typeof(SettingActivity))
                .Apply(x => {
                    x.PutExtra(typeKey,timeline);
                    x.PutExtra(titleKey,activity.GetString(Resource.String.SettingTimeline));
                })
            );
        }

        public static void StartOpenSource(Activity activity) {
            activity.StartActivity(
                new Intent(activity.ApplicationContext,typeof(SettingActivity))
                .Apply(x => {
                    x.PutExtra(typeKey,openSource);
                    x.PutExtra(titleKey,activity.GetString(Resource.String.OpenSource));
                })
            );
        }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingActivity);
            SetSupportActionBar(FindViewById<SToolbar>(Resource.Id.Toolbar));
            SupportActionBar.Title = Intent.GetStringExtra(titleKey) ?? GetString(Resource.String.Setting);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            int type = Intent.GetIntExtra(typeKey,main);
            SFragment fragment = null;
            if(type == main) {
                fragment = new MainSettingFragment();
            }
            if(type == application) {
                fragment = new ApplicationSettingFragment();
            }
            if(type == accounts) {
                fragment = new AccountsSettingFragment();
            }
            if(type == timeline) {
                fragment = new TimelineSettingFragment();
            }
            if(type == openSource) {
                fragment = new OpenSourceSettingFragment();
            }

            if(fragment == null) {
                return;
            }
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.Content,fragment).Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item) {
            if(item.ItemId == global::Android.Resource.Id.Home) {
                Finish();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}