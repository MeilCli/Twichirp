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
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidSlideLayout;
using Newtonsoft.Json;
using Twichirp.Android.Extensions;
using Twichirp.Android.ViewControllers;
using Twichirp.Android.Views.Interfaces;
using Twichirp.Core.DataObjects;
using Twichirp.Core.DataRepositories;
using Twichirp.Core.Settings;
using Twichirp.Core.ViewModels;
using AActivity = Android.App.Activity;
using CStatus = CoreTweet.Status;

// 未使用フィールドの警告非表示
#pragma warning disable 0414

namespace Twichirp.Android.Views.Activities {

    [Activity]
    public class VideoViewerActivity : BaseActivity, IVideoViewerView {

        private const string extraStatus = "extra_status";
        private const string extraAccount = "extra_account";

        public static void Start(AActivity activity, ImmutableAccount account, string statusJson) {
            var intent = new Intent(activity, typeof(VideoViewerActivity));
            intent.PutExtra(extraStatus, statusJson);
            intent.PutExtra(extraAccount, account.Id);
            activity.StartActivityCompat(intent);
        }

        private StatusViewModel statusViewModel;
        private VideoViewerViewController videoViewerViewController;

        public SlideLayout SlideLayout { get; private set; }

        public VideoView VideoView { get; private set; }

        protected override void OnViewCreate(Bundle savedInstanceState) {
            var accountRepository = TwichirpApplication.Resolve<IAccountRepository>();
            var settingManager = TwichirpApplication.Resolve<SettingManager>();

            ImmutableAccount account = accountRepository[Intent.GetLongExtra(extraAccount, settingManager.Accounts.DefaultAccountId)];
            var status = JsonConvert.DeserializeObject<CStatus>(Intent.GetStringExtra(extraStatus));
            statusViewModel = StatusViewModel.Resolve(TwichirpApplication.UnityContainer, status, account);
            videoViewerViewController = new VideoViewerViewController(this, statusViewModel);

            SetContentView(Resource.Layout.VideoViewerActivity);
            SlideLayout = FindViewById<SlideLayout>(Resource.Id.SlideLayout);
            VideoView = FindViewById<VideoView>(Resource.Id.VideoView);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            SlideLayout?.Dispose();
            VideoView?.Dispose();
        }
    }
}