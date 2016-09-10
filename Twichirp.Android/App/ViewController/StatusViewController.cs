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
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.Extensions;
using Twichirp.Android.App.View;
using Twichirp.Core.App.ViewModel;

namespace Twichirp.Android.App.ViewController {
    public class StatusViewController : BaseViewController<IStatusView,StatusViewModel> {

        public StatusViewController(IStatusView view,StatusViewModel viewModel) : base(view,viewModel) {
            view.OnCreateEventHandler += onCreate;
        }

        private void onCreate(object sender,LifeCycleEventArgs e) {
            var text = ViewModel.Text
                .Select(x => x.ToText())
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            View.Text.SetBinding(x => x.Text,text);

            var visibleRetweetingUser = ViewModel.ToStatusType() == StatusViewModel.RetweetedNormalTweet ? ViewStates.Visible : ViewStates.Gone;
            View.RetweetingUser.Visibility = visibleRetweetingUser;
            View.RetweetingUser.SetBinding(x => x.Text,ViewModel.RetweetingUser);

            var visibleReplyToUser = ViewModel.InReplyToScreenName
                .Select(x => x == null ? ViewStates.Gone : ViewStates.Visible)
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            View.ReplyToUser.SetBinding(x => x.Visibility,visibleReplyToUser);
            View.ReplyToUser.SetBinding(x => x.Text,ViewModel.ReplyToUser);

            View.Name.SetBinding(x => x.Text,ViewModel.Name);
            View.ScreenName.SetBinding(x => x.Text,ViewModel.ScreenName);
            View.DateTime.SetBinding(x => x.Text,ViewModel.DateTime);
            View.ApplicationContext.LoadIntoBitmap(ViewModel.IconUrl.Value,View.Icon);

        }
    }
}