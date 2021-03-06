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
using System.Reactive.Linq;
using Android.Support.Constraints;
using Android.Support.V4.Content.Res;
using Android.Support.V4.Graphics.Drawable;
using Android.Views;
using CoreTweet;
using FFImageLoading;
using FFImageLoading.Transformations;
using Plugin.CrossFormattedText;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.Events;
using Twichirp.Android.Extensions;
using Twichirp.Android.Views;
using Twichirp.Android.Views.Activities;
using Twichirp.Android.Views.Interfaces;
using Twichirp.Core.ViewModels;

namespace Twichirp.Android.ViewControllers {

    public class StatusViewController : BaseViewController<IStatusView, StatusViewModel> {

        private bool isVideoMedia;

        public StatusViewController(IStatusView view, StatusViewModel viewModel) : base(view, viewModel) {
            AutoDisposeViewModel = false;
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Created += x, x => view.Created -= x)
                .Subscribe(x => onCreate(x.Sender, x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Destroyed += x, x => view.Destroyed -= x)
                .Subscribe(x => onDestory(x.Sender, x.EventArgs))
                .AddTo(Disposable);
        }

        private void onCreate(object sender, LifeCycleEventArgs e) {
            ViewModel.UpdateDateTimeCommand.Execute();

            ViewModel.SpannableText.Subscribe(x => View.Text.SetTextWithCommandableSpan(x)).AddTo(Disposable);

            ViewModel.RetweetingUser
                .Subscribe(x => {
                    View.RetweetingUser.Visibility = x != null ? ViewStates.Visible : ViewStates.Gone;
                    if (x != null) {
                        View.RetweetingUser.Text = x;
                    }
                })
                .AddTo(Disposable);
            ViewModel.ReplyToUser
                .Subscribe(x => {
                    View.ReplyToUser.Visibility = x != null ? ViewStates.Visible : ViewStates.Gone;
                    if (x != null) {
                        View.ReplyToUser.Text = x;
                    }
                })
                .AddTo(Disposable);

            ViewModel.SpannableName.Subscribe(x => View.Name.TextFormatted = x.Span()).AddTo(Disposable);
            View.DateTime.SetBinding(x => x.Text, ViewModel.DateTime).AddTo(Disposable);
            ViewModel.IconUrl.Subscribe(x => ImageService.Instance.LoadUrl(x).Transform(new RoundedTransformation(60d)).FadeAnimation(true).Into(View.Icon)).AddTo(Disposable);

            ViewModel.IsProtected.Subscribe(x => View.LockIcon.Visibility = x ? ViewStates.Visible : ViewStates.Gone).AddTo(Disposable);
            ViewModel.IsVerified.Subscribe(x => View.VerifyIcon.Visibility = x ? ViewStates.Visible : ViewStates.Gone).AddTo(Disposable);

            if (View.StatusType == StatusViewModel.MediaTweet ||
                View.StatusType == StatusViewModel.QuotedOuterMediaTweet ||
                View.StatusType == StatusViewModel.QuotedInnerAndOuterMediaTweet) {

                ViewModel.Media.Subscribe(x => setMedia(x)).AddTo(Disposable);
                ViewModel.StartMediaViewerPageCommand
                    .Subscribe(x => {
                        startMediaViewer(x);
                    })
                    .AddTo(Disposable);
                View.MediaFrame1.ClickAsObservable().Select(x => 0).SetCommand(ViewModel.StartMediaViewerPageCommand).AddTo(Disposable);
                View.MediaFrame2.ClickAsObservable().Select(x => 1).SetCommand(ViewModel.StartMediaViewerPageCommand).AddTo(Disposable);
                View.MediaFrame3.ClickAsObservable().Select(x => 2).SetCommand(ViewModel.StartMediaViewerPageCommand).AddTo(Disposable);
                View.MediaFrame4.ClickAsObservable().Select(x => 3).SetCommand(ViewModel.StartMediaViewerPageCommand).AddTo(Disposable);
            }

            if (View.StatusType == StatusViewModel.QuotedTweet ||
                View.StatusType == StatusViewModel.QuotedInnerMediaTweet ||
                View.StatusType == StatusViewModel.QuotedOuterMediaTweet ||
                View.StatusType == StatusViewModel.QuotedInnerAndOuterMediaTweet) {

                ViewModel.QuotedSpannableName.Subscribe(x => View.QuotingName.TextFormatted = x.Span()).AddTo(Disposable);
                ViewModel.QuotedSpannableText.Subscribe(x => View.QuotingText.TextFormatted = x.Span()).AddTo(Disposable);
            }

            if (View.StatusType == StatusViewModel.QuotedInnerMediaTweet ||
                View.StatusType == StatusViewModel.QuotedInnerAndOuterMediaTweet) {

                ViewModel.QuotedMedia.Subscribe(x => setQuotingMedia(x)).AddTo(Disposable);
            }

            ViewModel.IsRetweeted.Subscribe(x => setRetweetIcon(x)).AddTo(Disposable);
            ViewModel.RetweetCount
                .Subscribe(x => {
                    View.RetweetCount.Visibility = x > 0 ? ViewStates.Visible : ViewStates.Gone;
                    View.RetweetCount.Text = x.ToString();
                })
                .AddTo(Disposable);
            ViewModel.IsProtected.Subscribe(x => View.RetweetIconClickable.Enabled = x == false).AddTo(Disposable);
            View.RetweetIconClickable.ClickAsObservable().SetCommand(ViewModel.RetweetCommand).AddTo(Disposable);

            ViewModel.IsFavorited.Subscribe(x => setFavoriteIcon(x)).AddTo(Disposable);
            ViewModel.FavoriteCount
                .Subscribe(x => {
                    View.FavoriteCount.Visibility = x > 0 ? ViewStates.Visible : ViewStates.Gone;
                    View.FavoriteCount.Text = x.ToString();
                })
                .AddTo(Disposable);
            View.FavoriteIconClickable.ClickAsObservable().SetCommand(ViewModel.FavoriteCommand).AddTo(Disposable);

            ViewModel.StartUserProfilePageCommand.Subscribe(x => UserProfileActivity.Start(View.Activity, x, null, ViewModel.Account)).AddTo(Disposable);
        }

        private void setRetweetIcon(bool isRetweeted) {
            var drawable = DrawableCompat.Wrap(ResourcesCompat.GetDrawable(View.ApplicationContext.Resources, Resource.Drawable.IconRepeatGrey36dp, null));
            View.RetweetIcon.SetImageDrawable(drawable);
            if (ViewModel.IsProtected.Value) {
                DrawableCompat.SetTint(drawable, ResourcesCompat.GetColor(View.ApplicationContext.Resources, Resource.Color.Grey300, null));
            } else if (isRetweeted) {
                DrawableCompat.SetTint(drawable, ResourcesCompat.GetColor(View.ApplicationContext.Resources, Resource.Color.Retweet, null));
            } else {
                DrawableCompat.SetTint(drawable, ResourcesCompat.GetColor(View.ApplicationContext.Resources, Resource.Color.Grey600, null));
            }
        }

        private void setFavoriteIcon(bool isFavorited) {
            var drawable = DrawableCompat.Wrap(ResourcesCompat.GetDrawable(View.ApplicationContext.Resources, Resource.Drawable.IconStarGrey36dp, null));
            View.FavoriteIcon.SetImageDrawable(drawable);
            if (isFavorited) {
                DrawableCompat.SetTint(drawable, ResourcesCompat.GetColor(View.ApplicationContext.Resources, Resource.Color.Favorite, null));
            } else {
                DrawableCompat.SetTint(drawable, ResourcesCompat.GetColor(View.ApplicationContext.Resources, Resource.Color.Grey600, null));
            }
        }

        private void setMedia(IEnumerable<MediaEntity> media) {
            int count = media.Count();
            View.MediaFrame2.Visibility = count >= 2 ? ViewStates.Visible : ViewStates.Gone;
            View.MediaFrame3.Visibility = count >= 3 ? ViewStates.Visible : ViewStates.Gone;
            View.MediaFrame4.Visibility = count >= 4 ? ViewStates.Visible : ViewStates.Gone;
            View.Media2.Visibility = count >= 2 ? ViewStates.Visible : ViewStates.Gone;
            View.Media3.Visibility = count >= 3 ? ViewStates.Visible : ViewStates.Gone;
            View.Media4.Visibility = count >= 4 ? ViewStates.Visible : ViewStates.Gone;

            // ConstraintLayout 1.0.0 alpha-9のバグでGoneが反映されない
            // Issueによると1.0.0 beta-2ぐらいで直ったらしい(遠い目)
            bool changed = false;
            if (View.Media1.LayoutParameters is ConstraintLayout.LayoutParams layoutParams1) {
                if (View.Media2.Visibility == ViewStates.Gone) {
                    layoutParams1.RightToLeft = ConstraintLayout.LayoutParams.Unset;
                    layoutParams1.RightToRight = Resource.Id.MediaContainer;
                    changed = true;
                }
                if (View.Media3.Visibility == ViewStates.Gone) {
                    layoutParams1.BottomToTop = ConstraintLayout.LayoutParams.Unset;
                    layoutParams1.BottomToBottom = Resource.Id.MediaContainer;
                    changed = true;
                }
            }
            if (View.Media4.Visibility == ViewStates.Gone && View.Media3.LayoutParameters is ConstraintLayout.LayoutParams layoutParams3) {
                layoutParams3.RightToLeft = ConstraintLayout.LayoutParams.Unset;
                layoutParams3.RightToRight = Resource.Id.MediaContainer;
                changed = true;
            }
            if (changed) {
                View.Media1.Parent.RequestLayout();
            }

            var mediaViews = new[] { View.Media1, View.Media2, View.Media3, View.Media4 };
            for (int i = 0; i < count && i < mediaViews.Length; i++) {
                var m = media.ElementAt(i);
                var imageTask = ImageService.Instance.LoadUrl(m.MediaUrl + ":small");
                if ((m.VideoInfo?.Variants.Length ?? 0) > 0) {
                    imageTask.Transform(new PlayCircleTransformation(View.ApplicationContext));
                    isVideoMedia = true;
                }
                imageTask.Into(mediaViews[i]);
            }
        }

        private void startMediaViewer(int page) {
            if (isVideoMedia) {
                VideoViewerActivity.Start(View.Activity, ViewModel.Account, ViewModel.Json);
            } else {
                switch (page) {
                    case 0:
                        ImageViewerActivity.Start(View.Activity, ViewModel.Account, ViewModel.Json, View.Media1, 0);
                        break;
                    case 1:
                        ImageViewerActivity.Start(View.Activity, ViewModel.Account, ViewModel.Json, View.Media2, 1);
                        break;
                    case 2:
                        ImageViewerActivity.Start(View.Activity, ViewModel.Account, ViewModel.Json, View.Media3, 2);
                        break;
                    case 3:
                        ImageViewerActivity.Start(View.Activity, ViewModel.Account, ViewModel.Json, View.Media4, 3);
                        break;
                }
            }
        }

        private void setQuotingMedia(IEnumerable<MediaEntity> media) {
            int count = media.Count();
            View.QuotingMedia2.Visibility = count >= 2 ? ViewStates.Visible : ViewStates.Gone;
            View.QuotingMedia3.Visibility = count >= 3 ? ViewStates.Visible : ViewStates.Gone;
            View.QuotingMedia4.Visibility = count >= 4 ? ViewStates.Visible : ViewStates.Gone;

            // ConstraintLayout 1.0.0 alpha-9のバグでGoneが反映されない
            // Issueによると1.0.0 beta-2ぐらいで直ったらしい(遠い目)
            bool changed = false;
            if (View.QuotingMedia1.LayoutParameters is ConstraintLayout.LayoutParams layoutParams1) {
                if (View.QuotingMedia2.Visibility == ViewStates.Gone) {
                    layoutParams1.RightToLeft = ConstraintLayout.LayoutParams.Unset;
                    layoutParams1.RightToRight = Resource.Id.QuotingMediaContainer;
                    changed = true;
                }
                if (View.QuotingMedia3.Visibility == ViewStates.Gone) {
                    layoutParams1.BottomToTop = ConstraintLayout.LayoutParams.Unset;
                    layoutParams1.BottomToBottom = Resource.Id.QuotingMediaContainer;
                    changed = true;
                }
            }
            if (View.QuotingMedia4.Visibility == ViewStates.Gone && View.QuotingMedia3.LayoutParameters is ConstraintLayout.LayoutParams layoutParams3) {
                layoutParams3.RightToLeft = ConstraintLayout.LayoutParams.Unset;
                layoutParams3.RightToRight = Resource.Id.QuotingMediaContainer;
                changed = true;
            }
            if (changed) {
                View.QuotingMedia1.Parent.RequestLayout();
            }

            var mediaViews = new[] { View.QuotingMedia1, View.QuotingMedia2, View.QuotingMedia3, View.QuotingMedia4 };
            for (int i = 0; i < count && i < mediaViews.Length; i++) {
                ImageService.Instance.LoadUrl(media.ElementAt(i).MediaUrl + ":small").FadeAnimation(true).Into(mediaViews[i]);
                var m = media.ElementAt(i);
                var imageTask = ImageService.Instance.LoadUrl(m.MediaUrl + ":small");
                if ((m.VideoInfo?.Variants.Length ?? 0) > 0) {
                    imageTask.Transform(new PlayCircleTransformation(View.ApplicationContext));
                }
                imageTask.Into(mediaViews[i]);
            }
        }

        private void onDestory(object sender, LifeCycleEventArgs e) {
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