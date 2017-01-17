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
using Android.Views;
using Android.Widget;
using Android.Util;
using SToolbar = Android.Support.V7.Widget.Toolbar;
using Twichirp.Core.App.ViewModel;
using Twichirp.Android.App.ViewController;

namespace Twichirp.Android.App.View.Activity {
    [Activity(MainLauncher =true)]
    public class SplashActivity : BaseActivity,ISplashView {

        private SplashViewModel splashViewModel;
        private SplashViewController splashViewController;

        public TextView Message { get; private set; }

        protected override void OnViewCreate(Bundle savedInstanceState) {
            splashViewModel = new SplashViewModel(TwichirpApplication);
            splashViewController = new SplashViewController(this,splashViewModel);
            SetContentView(Resource.Layout.SplashActivity);
            var toolbar = FindViewById<SToolbar>(Resource.Id.Toolbar);
            SetSupportActionBar(toolbar);
            Message = FindViewById<TextView>(Resource.Id.Message);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            Message?.Dispose();
        }
    }
}