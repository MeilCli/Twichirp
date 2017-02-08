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
using FFImageLoading.Views;
using Twichirp.Android.App.ViewModel;

namespace Twichirp.Android.App.View.Activity {

    [Activity(LaunchMode = global::Android.Content.PM.LaunchMode.SingleTask)]
    public class MainActivity : BaseActivity, IMainView {

        private MainViewController mainViewController;

        public SToolbar Toolbar { get; private set; }

        public DrawerLayout DrawerLayout { get; private set; }

        public NavigationView Navigation { get; private set; }

        public CoordinatorLayout Coordinator { get; private set; }

        public FrameLayout IconClickable { get; private set; }

        public ImageViewAsync Icon { get; private set; }

        public FrameLayout FirstSubIconClickable { get; private set; }

        public ImageViewAsync FirstSubIcon { get; private set; }

        public FrameLayout SecondSubIconClickable { get; private set; }

        public ImageViewAsync SecondSubIcon { get; private set; }

        public RelativeLayout Subtitle { get; private set; }

        public TextView Name { get; private set; }

        public TextView ScreenName { get; private set; }

        public ImageView Drop { get; private set; }

        public ImageViewAsync Background { get; private set; }

        protected override void OnViewCreate(Bundle savedInstanceState) {

            mainViewController = new MainViewController(this,new MainViewModel(TwichirpApplication));
            base.SetContentView(Android.Resource.Layout.MainActivity);
            Toolbar = FindViewById<SToolbar>(Android.Resource.Id.Toolbar);
            SetSupportActionBar(Toolbar);

            DrawerLayout = FindViewById<DrawerLayout>(Android.Resource.Id.DrawerLayout);
            Navigation = FindViewById<NavigationView>(Android.Resource.Id.Navigation);
            Coordinator = FindViewById<CoordinatorLayout>(Android.Resource.Id.Coordinator);
            // xmlごしだとなぜか見つからないエラー
            Navigation.InflateHeaderView(Android.Resource.Layout.NavigationHeader);
            var headerView = Navigation.GetHeaderView(0);
            IconClickable = headerView.FindViewById<FrameLayout>(Android.Resource.Id.IconClickable);
            Icon = headerView.FindViewById<ImageViewAsync>(Android.Resource.Id.Icon);
            FirstSubIconClickable = headerView.FindViewById<FrameLayout>(Android.Resource.Id.FirstSubIconClickable);
            FirstSubIcon = headerView.FindViewById<ImageViewAsync>(Android.Resource.Id.FirstSubIcon);
            SecondSubIconClickable = headerView.FindViewById<FrameLayout>(Android.Resource.Id.SecondSubIconClickable);
            SecondSubIcon = headerView.FindViewById<ImageViewAsync>(Android.Resource.Id.SecondSubIcon);
            Subtitle = headerView.FindViewById<RelativeLayout>(Android.Resource.Id.Subtitle);
            Name = headerView.FindViewById<TextView>(Android.Resource.Id.Name);
            ScreenName = headerView.FindViewById<TextView>(Android.Resource.Id.ScreenName);
            Drop = headerView.FindViewById<ImageView>(Android.Resource.Id.Drop);
            Background = headerView.FindViewById<ImageViewAsync>(Android.Resource.Id.Background);

            if(savedInstanceState == null) {
                base.SupportFragmentManager.BeginTransaction().Replace(Android.Resource.Id.Content,new StatusTimelineFragment()).Commit();
            }
        }
    }
}

