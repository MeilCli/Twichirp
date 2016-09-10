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
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Twichirp.Android.App.ViewController;
using Twichirp.Core.App.ViewModel;
using AView = Android.Views.View;

namespace Twichirp.Android.App.View.Holder {
    public class StatusHolder : BaseHolder<BaseViewModel>,IStatusView {

        private StatusViewController statusViewController;

        public AView ClickableView { get; private set; }

        public TextView Text { get; private set; }

        public TextView RetweetingUser { get; private set; }

        public TextView ReplyToUser { get; private set; }

        public TextView Name { get; private set; }

        public TextView ScreenName { get; private set; }

        public ImageView Icon { get; private set; }

        public TextView DateTime { get; private set; }

        public StatusHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup) : base(view,lifeCycle,viewGroup,Resource.Layout.StatusHolder) {
        }

        public override void OnCreatedView() {
            ClickableView = ItemView;
            Text = ItemView.FindViewById<TextView>(Resource.Id.Text);
            RetweetingUser = ItemView.FindViewById<TextView>(Resource.Id.RetweeingUser);
            ReplyToUser = ItemView.FindViewById<TextView>(Resource.Id.ReplyToUser);
            Name = ItemView.FindViewById<TextView>(Resource.Id.Name);
            ScreenName = ItemView.FindViewById<TextView>(Resource.Id.ScreenName);
            Icon = ItemView.FindViewById<ImageView>(Resource.Id.Icon);
            DateTime = ItemView.FindViewById<TextView>(Resource.Id.DateTime);
        }

        public override void OnPreBind(BaseViewModel item,int position) {
            this.statusViewController = new StatusViewController(this,item as StatusViewModel);
        }
    }
}