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

using Android.App;
using Android.OS;
using Android.Widget;
using Twichirp.Android.ViewControllers;
using Twichirp.Android.Views.Interfaces;
using Twichirp.Core.ViewModels;
using SToolbar = Android.Support.V7.Widget.Toolbar;

// 未使用フィールドの警告非表示
#pragma warning disable 0414

namespace Twichirp.Android.Views.Activities {

    [Activity]
    public class LoginActivity : BaseActivity, ILoginView {

        private LoginViewModel loginViewModel;
        private LoginViewController loginViewController;

        public Button GoToWeb { get; private set; }

        public Button Login { get; private set; }

        public EditText Pin { get; private set; }

        protected override void OnViewCreate(Bundle savedInstanceState) {
            loginViewModel = TwichirpApplication.Resolve<LoginViewModel>();
            loginViewController = new LoginViewController(this, loginViewModel);
            base.SetContentView(Resource.Layout.LoginActivity);
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