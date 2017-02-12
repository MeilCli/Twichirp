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
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using FFImageLoading.Views;
using Twichirp.Core.App.ViewModel;
using Twichirp.Core.Model;
using SToolbar = Android.Support.V7.Widget.Toolbar;
using AActivity = Android.App.Activity;
using AView = Android.Views.View;
using CoreTweet;
using Twichirp.Android.App.Extensions;
using Newtonsoft.Json;
using Twichirp.Android.App.ViewController;

namespace Twichirp.Android.App.View.Activity {

    [Activity]
    public class UserProfileActivity : BaseActivity, IUserProfileView {

        private const string extraUser = "extra_user";
        private const string extraUserId = "extra_user_id";
        private const string extraAccount = "extra_account";

        public static void Start(AActivity activity,long userId,string userJson = null,Account account = null,ImageView icon = null) {
            var intent = new Intent(activity,typeof(UserProfileActivity));
            intent.PutExtra(extraUserId,userId);
            if(userJson != null) {
                intent.PutExtra(extraUser,userJson);
            }
            if(account != null) {
                intent.PutExtra(extraAccount,account.Id);
            }
            Tuple<AView,string> iconTransition = null;
            if(icon != null) {
                iconTransition = Tuple.Create(icon as AView,UserProfileViewModel.TransitionIconName);
            }
            activity.StartActivityCompat(intent,iconTransition);
        }

        private UserProfileViewController userProfileViewController;

        public SToolbar Toolbar { get; private set; }

        public ImageViewAsync Icon { get; private set; }

        public ImageViewAsync Banner { get; private set; }

        public TextView Name { get; }

        public TextView ScreenName { get; private set; }

        public TextView Relationship { get; private set; }

        public ImageView LockIcon { get; private set; }

        public ImageView VerifyIcon { get; private set; }

        protected override void OnViewCreate(Bundle savedInstanceState) {
            long userId = Intent.GetLongExtra(extraUserId,0);
            string userJson = Intent.GetStringExtra(extraUser);
            User user = userJson != null ? JsonConvert.DeserializeObject<User>(userJson) : null;
            long accountId = Intent.GetLongExtra(extraAccount,-1);
            if(accountId == -1) {
                accountId = TwichirpApplication.SettingManager.Accounts.DefaultAccountId;
            }
            var account = TwichirpApplication.AccountManager[accountId];

            var viewModel = new UserProfileViewModel(TwichirpApplication,account,userId,user);
            userProfileViewController = new UserProfileViewController(this,viewModel);

            SetContentView(Android.Resource.Layout.UserProfileActivity);

            Toolbar = FindViewById<SToolbar>(Android.Resource.Id.Toolbar);
            SetSupportActionBar(Toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            Icon = FindViewById<ImageViewAsync>(Android.Resource.Id.Icon);
            Banner = FindViewById<ImageViewAsync>(Android.Resource.Id.Banner);
            ScreenName = FindViewById<TextView>(Android.Resource.Id.ScreenName);
            Relationship = FindViewById<TextView>(Android.Resource.Id.Relationship);
            LockIcon = FindViewById<ImageView>(Android.Resource.Id.LockIcon);
            VerifyIcon = FindViewById<ImageView>(Android.Resource.Id.VerifyIcon);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            Toolbar?.Dispose();
            Icon?.Dispose();
            Banner?.Dispose();
            ScreenName?.Dispose();
            Relationship?.Dispose();
            LockIcon?.Dispose();
            VerifyIcon?.Dispose();
        }
    }
}