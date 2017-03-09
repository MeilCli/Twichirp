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
using SToolbar = Android.Support.V7.Widget.Toolbar;
using AActivity = Android.App.Activity;
using AView = Android.Views.View;
using CoreTweet;
using Twichirp.Android.App.Extensions;
using Newtonsoft.Json;
using Twichirp.Android.App.ViewController;
using Android.Support.Design.Widget;
using Twichirp.Core.DataObjects;
using Microsoft.Practices.Unity;
using CUser = CoreTweet.User;
using Twichirp.Core.DataRepositories;
using Twichirp.Core.App.Setting;

// 未使用フィールドの警告非表示
#pragma warning disable 0414

namespace Twichirp.Android.App.View.Activity {

    [Activity]
    public class UserProfileActivity : BaseActivity, IUserProfileView, ViewTreeObserver.IOnGlobalLayoutListener {

        private const string extraUser = "extra_user";
        private const string extraUserId = "extra_user_id";
        private const string extraAccount = "extra_account";

        public static void Start(AActivity activity,long userId,string userJson = null,ImmutableAccount account = null,ImageView icon = null) {
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

        public event EventHandler<ExpandedTitleMarginEventArgs> DecideExpandedTitleMarginEventHandler;
        public event EventHandler<AppBarOffsetChangedEventArgs> AppBarOffsetChanged;

        private UserProfileViewController userProfileViewController;
        private CollapsingToolbarLayout collapsingToolbarLayout;
        private AppBarLayout appBarLayout;

        public SToolbar Toolbar { get; private set; }

        public ImageViewAsync Icon { get; private set; }

        public ImageViewAsync Banner { get; private set; }

        public TextView Name { get; }

        public TextView ScreenName { get; private set; }

        public TextView Relationship { get; private set; }

        public ImageView LockIcon { get; private set; }

        public ImageView VerifyIcon { get; private set; }

        public Button Friendship { get; private set; }

        public TextView Extraship { get; private set; }

        public TextView Description { get; private set; }

        public TextView Location { get; private set; }

        public TextView Url { get; private set; }

        protected override void OnViewCreate(Bundle savedInstanceState) {
            var accountRepository = TwichirpApplication.Resolve<IAccountRepository>();
            var settingManager = TwichirpApplication.Resolve<SettingManager>();

            long userId = Intent.GetLongExtra(extraUserId,0);
            string userJson = Intent.GetStringExtra(extraUser);
            CUser user = userJson != null ? JsonConvert.DeserializeObject<CUser>(userJson) : null;
            long accountId = Intent.GetLongExtra(extraAccount,-1);
            if(accountId == -1) {
                accountId = settingManager.Accounts.DefaultAccountId;
            }
            var account = accountRepository[accountId];

            var viewModel = UserProfileViewModel.Resolve(TwichirpApplication.UnityContainer,account,userId,user);
            userProfileViewController = new UserProfileViewController(this,viewModel);

            SetContentView(Resource.Layout.UserProfileActivity);

            collapsingToolbarLayout = FindViewById<CollapsingToolbarLayout>(Resource.Id.CollapsingToolbar);
            collapsingToolbarLayout.ViewTreeObserver.AddOnGlobalLayoutListener(this);
            appBarLayout = FindViewById<AppBarLayout>(Resource.Id.AppBar);
            appBarLayout.OffsetChanged += offsetChanged;

            Toolbar = FindViewById<SToolbar>(Resource.Id.Toolbar);
            SetSupportActionBar(Toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            Icon = FindViewById<ImageViewAsync>(Resource.Id.Icon);
            Banner = FindViewById<ImageViewAsync>(Resource.Id.Banner);
            ScreenName = FindViewById<TextView>(Resource.Id.ScreenName);
            Relationship = FindViewById<TextView>(Resource.Id.Relationship);
            LockIcon = FindViewById<ImageView>(Resource.Id.LockIcon);
            VerifyIcon = FindViewById<ImageView>(Resource.Id.VerifyIcon);
            Friendship = FindViewById<Button>(Resource.Id.Friendship);
            Extraship = FindViewById<TextView>(Resource.Id.Extraship);
            Description = FindViewById<TextView>(Resource.Id.Description);
            Location = FindViewById<TextView>(Resource.Id.Location);
            Url = FindViewById<TextView>(Resource.Id.Url);
        }

        public void OnGlobalLayout() {
            var expandedTitleMarginEventArgs = new ExpandedTitleMarginEventArgs(collapsingToolbarLayout.Width,collapsingToolbarLayout.Height);
            DecideExpandedTitleMarginEventHandler?.Invoke(this,expandedTitleMarginEventArgs);
            if(expandedTitleMarginEventArgs.MarginBottom != null) {
                collapsingToolbarLayout.ExpandedTitleMarginBottom = expandedTitleMarginEventArgs.MarginBottom.Value;
            }
            if(expandedTitleMarginEventArgs.MarginStart != null) {
                collapsingToolbarLayout.ExpandedTitleMarginStart = expandedTitleMarginEventArgs.MarginStart.Value;
            }
            if(Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean) {
                collapsingToolbarLayout.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            } else {
                collapsingToolbarLayout.ViewTreeObserver.RemoveGlobalOnLayoutListener(this);
            }
        }

        private void offsetChanged(object sender,AppBarLayout.OffsetChangedEventArgs args) {
            var eventArgs = new AppBarOffsetChangedEventArgs(args.AppBarLayout,args.VerticalOffset);
            AppBarOffsetChanged?.Invoke(this,eventArgs);
            if(SupportFragmentManager.Fragments == null) {
                return;
            }
            foreach(var fragment in SupportFragmentManager.Fragments) {
                if(fragment is IAppBarOffsetChangeEventRaise) {
                    (fragment as IAppBarOffsetChangeEventRaise).RaiseAppBarOffsetChanged(eventArgs);
                }
            }
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            collapsingToolbarLayout?.Dispose();
            appBarLayout.OffsetChanged -= offsetChanged;
            appBarLayout.Dispose();

            Toolbar?.Dispose();
            Icon?.Dispose();
            Banner?.Dispose();
            ScreenName?.Dispose();
            Relationship?.Dispose();
            LockIcon?.Dispose();
            VerifyIcon?.Dispose();
            Friendship?.Dispose();
            Extraship?.Dispose();
            Description?.Dispose();
            Location?.Dispose();
            Url?.Dispose();
        }

        public void InvalidateExpandedTitle() {
            collapsingToolbarLayout.ViewTreeObserver.AddOnGlobalLayoutListener(this);
            collapsingToolbarLayout.Invalidate();
        }

        public void SetTitle(string title) {
            collapsingToolbarLayout.SetTitle(title);
        }
    }
}