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
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Transformations;
using Plugin.CrossFormattedText;
using Plugin.CrossFormattedText.Abstractions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.Extensions;
using Twichirp.Android.App.View;
using Twichirp.Android.App.View.Fragment;
using Twichirp.Core.App.ViewModel;
using AView = Android.Views.View;

namespace Twichirp.Android.App.ViewController {

    public class UserProfileViewController : BaseViewController<IUserProfileView,UserProfileViewModel> {

        public UserProfileViewController(IUserProfileView view,UserProfileViewModel viewModel) : base(view,viewModel) {
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.OnCreateEventHandler += x,x => view.OnCreateEventHandler -= x)
                .Subscribe(x => onCreate(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.OnResumeEventHandler += x,x => view.OnResumeEventHandler -= x)
                .Subscribe(x => onResume(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.OnDestroyEventHandler += x,x => view.OnDestroyEventHandler -= x)
                .Subscribe(x => onDestroy(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<ExpandedTitleMarginEventArgs>(x => view.DecideExpandedTitleMarginEventHandler += x,x => view.DecideExpandedTitleMarginEventHandler -= x)
                .Subscribe(x => decideExpandedTitleMargin(x.Sender,x.EventArgs))
                .AddTo(Disposable);
        }

        private void onCreate(object sender,LifeCycleEventArgs args) {
            ViewModel.Banner.CombineLatest(ViewModel.LinkColor,(x,y) => Tuple.Create(x,y)).Subscribe(x => setUserBanner(x.Item1,x.Item2)).AddTo(Disposable);
            ViewModel.IconUrl.Subscribe(x => ImageService.Instance.LoadUrl(x).Transform(new RoundedTransformation(60d)).FadeAnimation(true).Into(View.Icon)).AddTo(Disposable);

            ViewModel.Name.Subscribe(x => View.Toolbar.Title = x).AddTo(Disposable);
            View.Name?.SetBinding(x => x.Text,ViewModel.Name).AddTo(Disposable);
            View.ScreenName.SetBinding(x => x.Text,ViewModel.ScreenName).AddTo(Disposable);
            View.ScreenName.SetTextColor(Color.Green);
            ViewModel.IsProtected.Subscribe(x => View.LockIcon.Visibility = x ? ViewStates.Visible : ViewStates.Gone).AddTo(Disposable);
            ViewModel.IsVerified.Subscribe(x => View.VerifyIcon.Visibility = x ? ViewStates.Visible : ViewStates.Gone).AddTo(Disposable);
            View.Relationship.SetBinding(x => x.Text,ViewModel.Relationship).AddTo(Disposable);
            View.Relationship.Visibility = ViewModel.IsOwnerAccount ? ViewStates.Gone : ViewStates.Visible;

            View.Friendship.SetBinding(x => x.Text,ViewModel.Friendship).AddTo(Disposable);
            View.Friendship.Visibility = ViewModel.IsOwnerAccount ? ViewStates.Gone : ViewStates.Visible;
            View.Extraship.SetBinding(x => x.Text,ViewModel.Extraship).AddTo(Disposable);
            View.Extraship.Visibility = ViewModel.IsOwnerAccount ? ViewStates.Gone : ViewStates.Visible;

            ViewModel.Description.Subscribe(x => setDescription(x)).AddTo(Disposable);
            ViewModel.Location.Subscribe(x => setLocation(x)).AddTo(Disposable);
            ViewModel.Url.Subscribe(x => setUrl(x)).AddTo(Disposable);

            View.Activity.SupportFragmentManager.BeginTransaction().Replace(Android.Resource.Id.Content,StatusTimelineFragment.MakeUser(StatusTimelineFragmentType.User,ViewModel.Account,ViewModel.Account.Id)).Commit();
        }

        private void onResume(object sender,LifeCycleEventArgs args) {
            // Toolbarの描写タイミング合わないので調整
            if(ViewModel.Name.Value != null) {
                View.Toolbar.Title = ViewModel.Name.Value;
            }
        }

        private void onDestroy(object sender,LifeCycleEventArgs args) {
            View.Banner.ReleaseImage();
        }

        private void decideExpandedTitleMargin(object sender,ExpandedTitleMarginEventArgs args) {
            args.MarginStart = View.Icon.Right + View.ApplicationContext.ConvertDensityIndependentPixelToPixel(16f);
            args.MarginBottom = args.TotalHeight - View.ApplicationContext.ConvertDensityIndependentPixelToPixel(120f);
        }

        private void setUserBanner(string banner,string linkColor) {
            if(banner != null) {
                ImageService.Instance.LoadUrl($"{banner}/mobile_retina").IntoAsync(View.Banner);
            } else if(banner != null) {
                try {
                    View.Banner.SetImageDrawable(new ColorDrawable((Color.ParseColor($"#{linkColor}"))));
                } catch(Exception) {
                    View.Banner.SetImageDrawable(new ColorDrawable((Color.Green)));
                }
            } else {
                View.Banner.SetImageDrawable(new ColorDrawable((Color.Green)));
            }
        }

        private void setDescription(ISpannableString description) {
            var span = description.Span();
            View.Description.Visibility = span.Length() > 0 ? ViewStates.Visible : ViewStates.Gone;
            View.Description.SetTextWithCommandableSpan(description);
        }

        private void setLocation(string location) {
            View.Location.Visibility = location.Length > 0 ? ViewStates.Visible : ViewStates.Gone;
            View.Location.Text = location;
        }

        private void setUrl(ISpannableString url) {
            var span = url.Span();
            View.Url.Visibility = span.Length() > 0 ? ViewStates.Visible : ViewStates.Gone;
            View.Url.SetTextWithCommandableSpan(url);
        }
    }
}