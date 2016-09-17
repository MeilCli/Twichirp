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
    public class StatusHolder : BaseHolder<BaseViewModel>, IStatusView {

        private StatusViewController statusViewController;

        public int StatusType { get; }

        public AView ClickableView { get; private set; }

        public TextView PrefixText { get; private set; }

        public TextView Text { get; private set; }

        public TextView SuffixText { get; private set; }

        public TextView RetweetingUser { get; private set; }

        public TextView ReplyToUser { get; private set; }

        public TextView Name { get; private set; }

        public TextView ScreenName { get; private set; }

        public ImageView Icon { get; private set; }

        public ImageView LockIcon { get; private set; }

        public ImageView VerifyIcon { get; private set; }

        public TextView DateTime { get; private set; }

        public LinearLayout MediaParent2 { get; protected set; }

        public FrameLayout MediaFrame2 { get; protected set; }

        public FrameLayout MediaFrame3 { get; protected set; }

        public FrameLayout MediaFrame4 { get; protected set; }

        public ImageView Media1 { get; protected set; }

        public ImageView Media2 { get; protected set; }

        public ImageView Media3 { get; protected set; }

        public ImageView Media4 { get; protected set; }

        public ImageView MediaPlay1 { get; protected set; }

        public ImageView MediaPlay2 { get; protected set; }

        public ImageView MediaPlay3 { get; protected set; }

        public ImageView MediaPlay4 { get; protected set; }

        public AView MediaClickable1 { get; protected set; }

        public AView MediaClickable2 { get; protected set; }

        public AView MediaClickable3 { get; protected set; }

        public AView MediaClickable4 { get; protected set; }

        public FrameLayout ReplyIconClickable { get; private set; }

        public ImageView ReplyIcon { get; private set; }

        public FrameLayout RetweetIconClickable { get; private set; }

        public ImageView RetweetIcon { get; private set; }

        public TextView RetweetCount { get; private set; }

        public FrameLayout FavoriteIconClickable { get; private set; }

        public ImageView FavoriteIcon { get; private set; }

        public TextView FavoriteCount { get; private set; }

        public StatusHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup,int statusType = StatusViewModel.NormalTweet,int layout = Resource.Layout.StatusHolder)
            : base(view,lifeCycle,viewGroup,layout) {
            StatusType = statusType;
        }

        public override void OnCreatedView() {
            ClickableView = ItemView;
            PrefixText = ItemView.FindViewById<TextView>(Resource.Id.PrefixText);
            Text = ItemView.FindViewById<TextView>(Resource.Id.Text);
            SuffixText = ItemView.FindViewById<TextView>(Resource.Id.SuffixText);
            RetweetingUser = ItemView.FindViewById<TextView>(Resource.Id.RetweeingUser);
            ReplyToUser = ItemView.FindViewById<TextView>(Resource.Id.ReplyToUser);
            Name = ItemView.FindViewById<TextView>(Resource.Id.Name);
            ScreenName = ItemView.FindViewById<TextView>(Resource.Id.ScreenName);
            Icon = ItemView.FindViewById<ImageView>(Resource.Id.Icon);
            LockIcon = ItemView.FindViewById<ImageView>(Resource.Id.LockIcon);
            VerifyIcon = ItemView.FindViewById<ImageView>(Resource.Id.VerifyIcon);
            DateTime = ItemView.FindViewById<TextView>(Resource.Id.DateTime);
            ReplyIconClickable = ItemView.FindViewById<FrameLayout>(Resource.Id.ReplyIconClickable);
            ReplyIcon = ItemView.FindViewById<ImageView>(Resource.Id.ReplyIcon);
            RetweetIconClickable = ItemView.FindViewById<FrameLayout>(Resource.Id.RetweetIconClickable);
            RetweetIcon = ItemView.FindViewById<ImageView>(Resource.Id.RetweetIcon);
            RetweetCount = ItemView.FindViewById<TextView>(Resource.Id.RetweetCount);
            FavoriteIconClickable = ItemView.FindViewById<FrameLayout>(Resource.Id.FavoriteIconClickable);
            FavoriteIcon = ItemView.FindViewById<ImageView>(Resource.Id.FavoriteIcon);
            FavoriteCount = ItemView.FindViewById<TextView>(Resource.Id.FavoriteCount);
        }

        public override void OnPreBind(BaseViewModel item,int position) {
            this.statusViewController = new StatusViewController(this,item as StatusViewModel);
        }
    }

    public class StatusMediaHolder : StatusHolder {

        public StatusMediaHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup) 
            : base(view,lifeCycle,viewGroup,StatusViewModel.MediaTweet,Resource.Layout.StatusMediaHolder) {
        }

        public override void OnCreatedView() {
            base.OnCreatedView();
            MediaParent2 = ItemView.FindViewById<LinearLayout>(Resource.Id.MediaParent2);
            MediaFrame2 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame2);
            MediaFrame3 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame3);
            MediaFrame4 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame4);
            Media1 = ItemView.FindViewById<ImageView>(Resource.Id.Media1);
            Media2 = ItemView.FindViewById<ImageView>(Resource.Id.Media2);
            Media3 = ItemView.FindViewById<ImageView>(Resource.Id.Media3);
            Media4 = ItemView.FindViewById<ImageView>(Resource.Id.Media4);
            MediaPlay1 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay1);
            MediaPlay2 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay2);
            MediaPlay3 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay3);
            MediaPlay4 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay4);
            MediaClickable1 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable1);
            MediaClickable2 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable2);
            MediaClickable3 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable3);
            MediaClickable4 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable4);
        }
    }

    public class StatusQuotingOuterMediaHolder : StatusHolder {
        public StatusQuotingOuterMediaHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup)
            : base(view,lifeCycle,viewGroup,StatusViewModel.QuotedOuterMediaTweet,Resource.Layout.StatusMediaHolder) {
        }

        public override void OnCreatedView() {
            base.OnCreatedView();
            MediaParent2 = ItemView.FindViewById<LinearLayout>(Resource.Id.MediaParent2);
            MediaFrame2 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame2);
            MediaFrame3 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame3);
            MediaFrame4 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame4);
            Media1 = ItemView.FindViewById<ImageView>(Resource.Id.Media1);
            Media2 = ItemView.FindViewById<ImageView>(Resource.Id.Media2);
            Media3 = ItemView.FindViewById<ImageView>(Resource.Id.Media3);
            Media4 = ItemView.FindViewById<ImageView>(Resource.Id.Media4);
            MediaPlay1 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay1);
            MediaPlay2 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay2);
            MediaPlay3 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay3);
            MediaPlay4 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay4);
            MediaClickable1 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable1);
            MediaClickable2 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable2);
            MediaClickable3 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable3);
            MediaClickable4 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable4);
        }
    }

    public class StatusQuotingInnerAndOuterMediaHolder : StatusHolder {
        public StatusQuotingInnerAndOuterMediaHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup)
            : base(view,lifeCycle,viewGroup,StatusViewModel.QuotedInnerAndOuterMediaTweet,Resource.Layout.StatusMediaHolder) {
        }

        public override void OnCreatedView() {
            base.OnCreatedView();
            MediaParent2 = ItemView.FindViewById<LinearLayout>(Resource.Id.MediaParent2);
            MediaFrame2 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame2);
            MediaFrame3 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame3);
            MediaFrame4 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame4);
            Media1 = ItemView.FindViewById<ImageView>(Resource.Id.Media1);
            Media2 = ItemView.FindViewById<ImageView>(Resource.Id.Media2);
            Media3 = ItemView.FindViewById<ImageView>(Resource.Id.Media3);
            Media4 = ItemView.FindViewById<ImageView>(Resource.Id.Media4);
            MediaPlay1 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay1);
            MediaPlay2 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay2);
            MediaPlay3 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay3);
            MediaPlay4 = ItemView.FindViewById<ImageView>(Resource.Id.MediaPlay4);
            MediaClickable1 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable1);
            MediaClickable2 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable2);
            MediaClickable3 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable3);
            MediaClickable4 = ItemView.FindViewById<AView>(Resource.Id.MediaClickable4);
        }
    }
}