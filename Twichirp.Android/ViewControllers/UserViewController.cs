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
using System.Reactive.Linq;
using Android.Views;
using FFImageLoading;
using FFImageLoading.Transformations;
using Plugin.CrossFormattedText;
using Plugin.CrossFormattedText.Abstractions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.Events;
using Twichirp.Android.Extensions;
using Twichirp.Android.Views.Interfaces;
using Twichirp.Core.ViewModels;

namespace Twichirp.Android.ViewControllers {

    public class UserViewController : BaseViewController<IUserView, UserViewModel> {

        public UserViewController(IUserView view, UserViewModel viewModel) : base(view, viewModel) {
            AutoDisposeViewModel = false;
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Created += x, x => view.Created -= x)
                .Subscribe(x => onCreate(x.Sender, x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Destroyed += x, x => view.Destroyed -= x)
                .Subscribe(x => onDestroy(x.Sender, x.EventArgs))
                .AddTo(Disposable);
        }

        private void onCreate(object sender, LifeCycleEventArgs args) {
            ViewModel.IconUrl.Subscribe(x => ImageService.Instance.LoadUrl(x).Transform(new RoundedTransformation(60d)).FadeAnimation(true).Into(View.Icon)).AddTo(Disposable);
            View.Name?.SetBinding(x => x.Text, ViewModel.Name).AddTo(Disposable);
            View.ScreenName.SetBinding(x => x.Text, ViewModel.ScreenName).AddTo(Disposable);
            View.ScreenName.SetTextColor(global::Android.Graphics.Color.Green);
            ViewModel.IsProtected.Subscribe(x => View.LockIcon.Visibility = x ? ViewStates.Visible : ViewStates.Gone).AddTo(Disposable);
            ViewModel.IsVerified.Subscribe(x => View.VerifyIcon.Visibility = x ? ViewStates.Visible : ViewStates.Gone).AddTo(Disposable);

            ViewModel.Description.Subscribe(x => setDescription(x)).AddTo(Disposable);
            ViewModel.Location.Subscribe(x => setLocation(x)).AddTo(Disposable);
            ViewModel.Url.Subscribe(x => setUrl(x)).AddTo(Disposable);
        }

        private void onDestroy(object sender, LifeCycleEventArgs args) {
            View.Icon.ReleaseImage();
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