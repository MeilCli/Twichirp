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
using Android.OS;
using Android.Widget;
using AndroidPageLayout;
using AndroidSlideLayout;
using Newtonsoft.Json;
using Twichirp.Android.Extensions;
using Twichirp.Android.ViewControllers;
using Twichirp.Android.ViewModels;
using Twichirp.Android.Views.Interfaces;
using Twichirp.Core.DataObjects;
using Twichirp.Core.DataRepositories;
using Twichirp.Core.Settings;
using AActivity = Android.App.Activity;
using AView = Android.Views.View;
using CStatus = CoreTweet.Status;

// 未使用フィールドの警告非表示
#pragma warning disable 0414

namespace Twichirp.Android.Views.Activities {

    [Activity]
    public class ImageViewerActivity : BaseActivity, IImageViewerView {

        private const string extraDefaultPage = "extra_default_page";
        private const string extraStatus = "extra_status";
        private const string extraAccount = "extra_account";

        public static void Start(AActivity activity, ImmutableAccount account, string statusJson, ImageView imageView, int defaultPage) {
            var intent = new Intent(activity, typeof(ImageViewerActivity));
            intent.PutExtra(extraDefaultPage, defaultPage);
            intent.PutExtra(extraStatus, statusJson);
            intent.PutExtra(extraAccount, account.Id);
            activity.StartActivityCompat(intent, Tuple.Create<AView, string>(imageView, ImageViewerViewModel.TransitionName));
        }

        private ImageViewerViewModel imageViewerViewModel;
        private ImageViewerViewController imageViewerViewControll;

        public PageLayout PageLayout { get; private set; }

        public SlideLayout SlideLayout { get; private set; }

        protected override void OnViewCreate(Bundle savedInstanceState) {
            var accountRepository = TwichirpApplication.Resolve<IAccountRepository>();
            var settingManager = TwichirpApplication.Resolve<SettingManager>();
            ImmutableAccount account = accountRepository[Intent.GetLongExtra(extraAccount, settingManager.Accounts.DefaultAccountId)];
            var status = JsonConvert.DeserializeObject<CStatus>(Intent.GetStringExtra(extraStatus));
            int defaultPage = Intent.GetIntExtra(extraDefaultPage, 0);
            imageViewerViewModel = ImageViewerViewModel.Resolve(TwichirpApplication.UnityContainer, status, account, defaultPage);
            imageViewerViewControll = new ImageViewerViewController(this, imageViewerViewModel);

            SetContentView(Resource.Layout.ImageViewerActivity);

            PageLayout = FindViewById<PageLayout>(Resource.Id.PageLayout);
            SlideLayout = FindViewById<SlideLayout>(Resource.Id.SlideLayout);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            PageLayout?.Dispose();
            SlideLayout?.Dispose();
        }
    }
}