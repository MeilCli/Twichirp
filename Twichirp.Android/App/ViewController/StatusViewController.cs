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
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Graphics.Drawable;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Plugin.CrossFormattedText;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.DataHolder;
using Twichirp.Android.App.Extensions;
using Twichirp.Android.App.View;
using Twichirp.Core.App.ViewModel;

namespace Twichirp.Android.App.ViewController {
    public class StatusViewController : BaseViewController<IStatusView,StatusViewModel> {

        public StatusViewController(IStatusView view,StatusViewModel viewModel) : base(view,viewModel) {
            AutoDisposeViewModel = false;
            view.OnCreateEventHandler += onCreate;
            view.OnDestoryEventHandler += onDestory;
        }

        private void onCreate(object sender,LifeCycleEventArgs e) {
            if(ViewModel.DataHolder == null || ViewModel.DataHolder is StatusDataHolder == false) {
                ViewModel.DataHolder = new StatusDataHolder(ViewModel,View.ApplicationContext);
            }
            var statusDataHolder = ViewModel.DataHolder as StatusDataHolder;
            int statusType = ViewModel.ToStatusType();

            ViewModel.ShowStatusCommand.Execute();

            ViewModel.SpannableText.Subscribe(x => View.Text.TextFormatted = x.Span()).AddTo(Disposable);

            View.RetweetingUser.Visibility = statusDataHolder.VisibleRetweetingUser;
            View.RetweetingUser.Text = ViewModel.RetweetingUser;

            View.ReplyToUser.Visibility = statusDataHolder.VisibleReplyToUser;
            View.ReplyToUser.Text = ViewModel.ReplyToUser;

            View.Name.Text = ViewModel.Name;
            View.ScreenName.Text = ViewModel.ScreenName;
            View.DateTime.Text = ViewModel.DateTime.Value;
            ImageService.Instance.LoadUrl(ViewModel.IconUrl).FadeAnimation(true).Into(View.Icon);

            View.LockIcon.Visibility = statusDataHolder.VisibleLockIcon;
            View.VerifyIcon.Visibility = statusDataHolder.VisibleVerifyIcon;

            if(View.StatusType == StatusViewModel.MediaTweet ||
                View.StatusType == StatusViewModel.QuotedOuterMediaTweet ||
                View.StatusType == StatusViewModel.QuotedInnerAndOuterMediaTweet) {
                View.MediaParent2.Visibility = statusDataHolder.VisivleMediaParent2;
                View.MediaFrame2.Visibility = statusDataHolder.VisivleMedia2;
                View.MediaFrame3.Visibility = statusDataHolder.VisivleMedia3;
                View.MediaFrame4.Visibility = statusDataHolder.VisivleMedia4;
                var medias = new[] { View.Media1,View.Media2,View.Media3,View.Media4 };
                for(int i = 0;i < ViewModel.Media.Count() && i < medias.Length;i++) {
                    var _imageTask = ImageService.Instance.LoadUrl(ViewModel.Media.ElementAt(i).MediaUrl + ":small");
                    if(statusDataHolder.MeadiaIsVideoOrGif[i]) {
                        _imageTask.Transform(new PlayCircleTransformation(View.ApplicationContext));
                    }
                    _imageTask.Into(medias[i]);
                }
            }

            if(View.StatusType == StatusViewModel.QuotedTweet ||
                View.StatusType==StatusViewModel.QuotedInnerMediaTweet||
                View.StatusType == StatusViewModel.QuotedOuterMediaTweet||
                View.StatusType==StatusViewModel.QuotedInnerAndOuterMediaTweet) {
                View.QuotingName.Text = ViewModel.QuotedName;
                View.QuotingScreenName.Text = ViewModel.QuotedScreenName;

                ViewModel.QuotedSpannableText.Subscribe(x => View.QuotingText.TextFormatted = x.Span()).AddTo(Disposable);
            }

            if(View.StatusType == StatusViewModel.QuotedInnerMediaTweet ||
                View.StatusType == StatusViewModel.QuotedInnerAndOuterMediaTweet) {
                View.QuotingMediaParent2.Visibility = statusDataHolder.VisivleQuotingMediaParent2;
                View.QuotingMedia2.Visibility = statusDataHolder.VisivleQuotingMedia2;
                View.QuotingMedia3.Visibility = statusDataHolder.VisivleQuotingMedia3;
                View.QuotingMedia4.Visibility = statusDataHolder.VisivleQuotingMedia4;
                var medias = new[] { View.QuotingMedia1,View.QuotingMedia2,View.QuotingMedia3,View.QuotingMedia4 };
                for(int i = 0;i < ViewModel.QuotedMedia.Count() && i < medias.Length;i++) {
                    ImageService.Instance.LoadUrl(ViewModel.QuotedMedia.ElementAt(i).MediaUrl + ":small").FadeAnimation(true).Into(medias[i]);
                }
            }

            DrawableCompat.SetTint(setRetweetIcon(),statusDataHolder.RetweetDrawableTint);
            View.RetweetCount.Visibility = statusDataHolder.VisibleRetweetCount;
            View.RetweetCount.Text = ViewModel.RetweetCountText;
            View.RetweetIconClickable.Enabled = statusDataHolder.IsRetweetIconDisabled == false;
            View.RetweetIconClickable.ClickAsObservable().SetCommand(ViewModel.RetweetCommand);

            DrawableCompat.SetTint(setFavoriteIcon(),statusDataHolder.FavoriteDrawableTint);
            View.FavoriteCount.Visibility = statusDataHolder.VisibleFavoriteCount;
            View.FavoriteCount.Text = ViewModel.FavoriteCountText;
            View.FavoriteIconClickable.ClickAsObservable().SetCommand(ViewModel.FavoriteCommand);
        }

        private Drawable setRetweetIcon() {
            View.RetweetIcon.SetImageDrawable(DrawableCompat.Wrap(View.ApplicationContext.GetDrawable(Resource.Drawable.IconRepeatGrey36dp)));
            return View.RetweetIcon.Drawable;
        }

        private Drawable setFavoriteIcon() {
            View.FavoriteIcon.SetImageDrawable(DrawableCompat.Wrap(View.ApplicationContext.GetDrawable(Resource.Drawable.IconStarGrey36dp)));
            return View.FavoriteIcon.Drawable;
        }

        private void onDestory(object sender,LifeCycleEventArgs e) {
            View.Icon.ReleaseImage();
            View.LockIcon.ReleaseImage();
            View.VerifyIcon.ReleaseImage();
            View.Media1?.ReleaseImage();
            View.Media2?.ReleaseImage();
            View.Media3?.ReleaseImage();
            View.Media4?.ReleaseImage();
            View.QuotingMedia1?.ReleaseImage();
            View.QuotingMedia2?.ReleaseImage();
            View.QuotingMedia3?.ReleaseImage();
            View.QuotingMedia4?.ReleaseImage();
            View.ReplyIcon.ReleaseImage();
            View.RetweetIcon.ReleaseImage();
            View.FavoriteIcon.ReleaseImage();
        }
    }
}