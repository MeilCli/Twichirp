﻿// Copyright (c) 2016 meil
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
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Twichirp.Android.App.ViewController;
using Twichirp.Android.App.Extensions;
using Twichirp.Android.App.View.Fragment;

namespace Twichirp.Android.App.View.Activity{

    [Activity(LaunchMode =global::Android.Content.PM.LaunchMode.SingleTask)]
    public class MainActivity : BaseActivity,IMainView {

        private MainViewController mainViewController;

        public DrawerLayout DrawerLayout { get; private set; }

        public NavigationView Navigation { get; private set; }

        public ImageView Icon { get; private set; }

        public RelativeLayout Subtitle { get; private set; }

        public TextView Name { get; private set; }

        public TextView ScreenName { get; private set; }

        public ImageView Drop { get; private set; }

        public ImageView Background { get; private set; }

        protected override void OnViewCreate(Bundle savedInstanceState) {

            mainViewController = new MainViewController(this);
            SetContentView(Resource.Layout.MainActivity);
            SToolbar toolbar = FindViewById<SToolbar>(Resource.Id.Toolbar);
            SetSupportActionBar(toolbar);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            Navigation = FindViewById<NavigationView>(Resource.Id.Navigation);
            // xmlごしだとなぜか見つからないエラー
            Navigation.InflateHeaderView(Resource.Layout.NavigationHeader);
            var headerView = Navigation.GetHeaderView(0);
            Icon = headerView.FindViewById<ImageView>(Resource.Id.Icon);
            Subtitle = headerView.FindViewById<RelativeLayout>(Resource.Id.Subtitle);
            Name = headerView.FindViewById<TextView>(Resource.Id.Name);
            ScreenName = headerView.FindViewById<TextView>(Resource.Id.ScreenName);
            Drop = headerView.FindViewById<ImageView>(Resource.Id.Drop);
            Background = headerView.FindViewById<ImageView>(Resource.Id.Background);

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.Content,new StatusTimelineFragment()).Commit();
        }
    }
}

