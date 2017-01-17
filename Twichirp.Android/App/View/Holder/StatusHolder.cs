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
using FFImageLoading.Views;
using Twichirp.Android.App.ViewController;
using Twichirp.Core.App.ViewModel;
using AView = Android.Views.View;

namespace Twichirp.Android.App.View.Holder {
    public class StatusHolder : BaseHolder<BaseViewModel>, IStatusView {

        private StatusViewController statusViewController;

        public int StatusType { get; }

        public AView ClickableView { get; private set; }

        public TextView Text { get; private set; }

        public TextView RetweetingUser { get; private set; }

        public TextView ReplyToUser { get; private set; }

        public TextView Name { get; private set; }

        public ImageViewAsync Icon { get; private set; }

        public ImageView LockIcon { get; private set; }

        public ImageView VerifyIcon { get; private set; }

        public TextView DateTime { get; private set; }

        public LinearLayout MediaParent2 { get; protected set; }

        public FrameLayout MediaFrame1 { get; protected set; }

        public FrameLayout MediaFrame2 { get; protected set; }

        public FrameLayout MediaFrame3 { get; protected set; }

        public FrameLayout MediaFrame4 { get; protected set; }

        public ImageViewAsync Media1 { get; protected set; }

        public ImageViewAsync Media2 { get; protected set; }

        public ImageViewAsync Media3 { get; protected set; }

        public ImageViewAsync Media4 { get; protected set; }

        public FrameLayout ReplyIconClickable { get; private set; }

        public ImageView ReplyIcon { get; private set; }

        public FrameLayout RetweetIconClickable { get; private set; }

        public ImageView RetweetIcon { get; private set; }

        public TextView RetweetCount { get; private set; }

        public FrameLayout FavoriteIconClickable { get; private set; }

        public ImageView FavoriteIcon { get; private set; }

        public TextView FavoriteCount { get; private set; }

        public AView QuotingClickable { get; protected set; }

        public TextView QuotingName { get; protected set; }

        public TextView QuotingText { get; protected set; }

        public LinearLayout QuotingMediaParent2 { get; protected set; }

        public ImageViewAsync QuotingMedia1 { get; protected set; }

        public ImageViewAsync QuotingMedia2 { get; protected set; }

        public ImageViewAsync QuotingMedia3 { get; protected set; }

        public ImageViewAsync QuotingMedia4 { get; protected set; }

        public StatusHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup,int statusType = StatusViewModel.NormalTweet,int layout = Resource.Layout.StatusHolder)
            : base(view,lifeCycle,viewGroup,layout) {
            StatusType = statusType;
        }

        ~StatusHolder() {
            ClickableView?.Dispose();
            Text?.Dispose();
            RetweetingUser?.Dispose();
            ReplyToUser?.Dispose();
            Name?.Dispose();
            Icon?.Dispose();
            LockIcon?.Dispose();
            VerifyIcon?.Dispose();
            DateTime?.Dispose();
            MediaParent2?.Dispose();
            MediaFrame1?.Dispose();
            MediaFrame2?.Dispose();
            MediaFrame3?.Dispose();
            MediaFrame4?.Dispose();
            Media1?.Dispose();
            Media2?.Dispose();
            Media3?.Dispose();
            Media4?.Dispose();
            ReplyIconClickable?.Dispose();
            ReplyIcon?.Dispose();
            RetweetIconClickable?.Dispose();
            RetweetIcon?.Dispose();
            RetweetCount?.Dispose();
            FavoriteIconClickable?.Dispose();
            FavoriteIcon?.Dispose();
            FavoriteCount?.Dispose();
            QuotingClickable?.Dispose();
            QuotingName?.Dispose();
            QuotingText?.Dispose();
            QuotingMediaParent2?.Dispose();
            QuotingMedia1?.Dispose();
            QuotingMedia2?.Dispose();
            QuotingMedia3?.Dispose();
            QuotingMedia4?.Dispose();
        }

        public override void OnCreatedView() {
            ClickableView = ItemView;
            Text = ItemView.FindViewById<TextView>(Resource.Id.Text);
            RetweetingUser = ItemView.FindViewById<TextView>(Resource.Id.RetweeingUser);
            ReplyToUser = ItemView.FindViewById<TextView>(Resource.Id.ReplyToUser);
            Name = ItemView.FindViewById<TextView>(Resource.Id.Name);
            Icon = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Icon);
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
            MediaFrame1 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame1);
            MediaFrame2 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame2);
            MediaFrame3 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame3);
            MediaFrame4 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame4);
            Media1 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media1);
            Media2 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media2);
            Media3 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media3);
            Media4 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media4);
        }
    }

    public class StatusQuotingHolder : StatusHolder {
        public StatusQuotingHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup)
            : base(view,lifeCycle,viewGroup,StatusViewModel.QuotedTweet,Resource.Layout.StatusQuotingHolder) {
        }

        public override void OnCreatedView() {
            base.OnCreatedView();
            QuotingClickable = ItemView.FindViewById<AView>(Resource.Id.QuotingClickable);
            QuotingName = ItemView.FindViewById<TextView>(Resource.Id.QuotingName);
            QuotingText = ItemView.FindViewById<TextView>(Resource.Id.QuotingText);
        }
    }

    public class StatusQuotingInnerMediaHolder : StatusHolder {
        public StatusQuotingInnerMediaHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup)
            : base(view,lifeCycle,viewGroup,StatusViewModel.QuotedInnerMediaTweet,Resource.Layout.StatusQuotingInnerMediaHolder) {
        }

        public override void OnCreatedView() {
            base.OnCreatedView();
            QuotingClickable = ItemView.FindViewById<AView>(Resource.Id.QuotingClickable);
            QuotingName = ItemView.FindViewById<TextView>(Resource.Id.QuotingName);
            QuotingText = ItemView.FindViewById<TextView>(Resource.Id.QuotingText);

            QuotingMediaParent2 = ItemView.FindViewById<LinearLayout>(Resource.Id.QuotingMediaParent2);
            QuotingMedia1 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.QuotingMedia1);
            QuotingMedia2 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.QuotingMedia2);
            QuotingMedia3 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.QuotingMedia3);
            QuotingMedia4 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.QuotingMedia4);
        }
    }

    public class StatusQuotingOuterMediaHolder : StatusHolder {
        public StatusQuotingOuterMediaHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup)
            : base(view,lifeCycle,viewGroup,StatusViewModel.QuotedOuterMediaTweet,Resource.Layout.StatusQuotingOuterMediaHolder) {
        }

        public override void OnCreatedView() {
            base.OnCreatedView();
            QuotingClickable = ItemView.FindViewById<AView>(Resource.Id.QuotingClickable);
            QuotingName = ItemView.FindViewById<TextView>(Resource.Id.QuotingName);
            QuotingText = ItemView.FindViewById<TextView>(Resource.Id.QuotingText);

            MediaParent2 = ItemView.FindViewById<LinearLayout>(Resource.Id.MediaParent2);
            MediaFrame1 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame1);
            MediaFrame2 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame2);
            MediaFrame3 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame3);
            MediaFrame4 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame4);
            Media1 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media1);
            Media2 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media2);
            Media3 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media3);
            Media4 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media4);
        }
    }

    public class StatusQuotingInnerAndOuterMediaHolder : StatusHolder {
        public StatusQuotingInnerAndOuterMediaHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup)
            : base(view,lifeCycle,viewGroup,StatusViewModel.QuotedInnerAndOuterMediaTweet,Resource.Layout.StatusQuotingInnerAndOuterMediaHolder) {
        }

        public override void OnCreatedView() {
            base.OnCreatedView();
            MediaParent2 = ItemView.FindViewById<LinearLayout>(Resource.Id.MediaParent2);
            MediaFrame1 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame1);
            MediaFrame2 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame2);
            MediaFrame3 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame3);
            MediaFrame4 = ItemView.FindViewById<FrameLayout>(Resource.Id.MediaFrame4);
            Media1 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media1);
            Media2 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media2);
            Media3 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media3);
            Media4 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.Media4);

            QuotingClickable = ItemView.FindViewById<AView>(Resource.Id.QuotingClickable);
            QuotingName = ItemView.FindViewById<TextView>(Resource.Id.QuotingName);
            QuotingText = ItemView.FindViewById<TextView>(Resource.Id.QuotingText);

            QuotingMediaParent2 = ItemView.FindViewById<LinearLayout>(Resource.Id.QuotingMediaParent2);
            QuotingMedia1 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.QuotingMedia1);
            QuotingMedia2 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.QuotingMedia2);
            QuotingMedia3 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.QuotingMedia3);
            QuotingMedia4 = ItemView.FindViewById<ImageViewAsync>(Resource.Id.QuotingMedia4);
        }
    }
}