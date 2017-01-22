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
using Twichirp.Core.App.ViewModel;
using Twichirp.Android.App.ViewController;
using SToolbar = Android.Support.V7.Widget.Toolbar;

namespace Twichirp.Android.App.View.Activity {
    [Activity]
    public class LoginActivity : BaseActivity,ILoginView {

        private LoginViewModel loginViewModel;
        private LoginViewController loginViewController;

        public Button GoToWeb { get; private set; }

        public Button Login { get; private set; }

        public EditText Pin { get; private set; }

        protected override void OnViewCreate(Bundle savedInstanceState) {
            loginViewModel = new LoginViewModel(TwichirpApplication);
            loginViewController = new LoginViewController(this,loginViewModel);
            SetContentView(Resource.Layout.LoginActivity);
            var toolbar = FindViewById<SToolbar>(Resource.Id.Toolbar);
            SetSupportActionBar(toolbar);
            GoToWeb = FindViewById<Button>(Resource.Id.GoToWeb);
            Login = FindViewById<Button>(Resource.Id.Login);
            Pin = FindViewById<EditText>(Resource.Id.Pin);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            GoToWeb?.Dispose();
            Login?.Dispose();
            Pin?.Dispose();
        }
    }
}