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
using Android.Util;
using Android.Views;
using Android.Widget;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.DataHolder;
using Twichirp.Android.App.Extensions;
using Twichirp.Android.App.View;
using Twichirp.Core.App.ViewModel;

namespace Twichirp.Android.App.ViewController {
    public class StatusViewController : BaseViewController<IStatusView,StatusViewModel> {

        public StatusViewController(IStatusView view,StatusViewModel viewModel) : base(view,viewModel) {
            view.OnCreateEventHandler += onCreate;
        }

        private void onCreate(object sender,LifeCycleEventArgs e) {
            if(ViewModel.DataHolder == null||ViewModel.DataHolder is StatusDataHolder==false) {
                ViewModel.DataHolder = new StatusDataHolder(ViewModel);
            }
            var statusDataHolder = ViewModel.DataHolder as StatusDataHolder;

            ViewModel.ShowStatusCommand.Execute();

            View.PrefixText.Visibility = statusDataHolder.VisiblePrefixText;
            View.PrefixText.Text = statusDataHolder.PrefixText;
            View.Text.Visibility = statusDataHolder.VisibleText;
            View.Text.Text = statusDataHolder.Text;
            View.SuffixText.Visibility = statusDataHolder.VisibleSuffixText;
            View.SuffixText.Text = statusDataHolder.SuffixText;

            View.RetweetingUser.Visibility = statusDataHolder.VisibleRetweetingUser;
            View.RetweetingUser.Text = ViewModel.RetweetingUser;

            View.ReplyToUser.Visibility = statusDataHolder.VisibleReplyToUser;
            View.ReplyToUser.Text = ViewModel.ReplyToUser;

            View.Name.Text = ViewModel.Name;
            View.ScreenName.Text = ViewModel.ScreenName;
            View.DateTime.Text = ViewModel.DateTime.Value;
            View.ApplicationContext.LoadIntoBitmap(ViewModel.IconUrl,View.Icon);

            View.LockIcon.Visibility = statusDataHolder.VisibleLockIcon;
            View.VerifyIcon.Visibility = statusDataHolder.VisibleVerifyIcon;

        }
    }
}