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

using Android.Widget;
using FFImageLoading.Views;
using AView = Android.Views.View;

namespace Twichirp.Android.Views.Interfaces {

    public interface IStatusView : IView, ILifeCycleView {

        int StatusType { get; }

        TextView RetweetingUser { get; }

        TextView ReplyToUser { get; }

        TextView Text { get; }

        TextView Name { get; }

        ImageViewAsync Icon { get; }

        ImageView LockIcon { get; }

        ImageView VerifyIcon { get; }

        TextView DateTime { get; }

        int MediaContainerId { get; }

        AView MediaFrame1 { get; }

        AView MediaFrame2 { get; }

        AView MediaFrame3 { get; }

        AView MediaFrame4 { get; }

        ImageViewAsync Media1 { get; }

        ImageViewAsync Media2 { get; }

        ImageViewAsync Media3 { get; }

        ImageViewAsync Media4 { get; }

        AView ReplyIconClickable { get; }

        ImageView ReplyIcon { get; }

        AView RetweetIconClickable { get; }

        ImageView RetweetIcon { get; }

        TextView RetweetCount { get; }

        AView FavoriteIconClickable { get; }

        ImageView FavoriteIcon { get; }

        TextView FavoriteCount { get; }

        AView ClickableView { get; }

        AView QuotingClickable { get; }

        TextView QuotingName { get; }

        TextView QuotingText { get; }

        int QuotingMediaContainerId { get; }

        ImageViewAsync QuotingMedia1 { get; }

        ImageViewAsync QuotingMedia2 { get; }

        ImageViewAsync QuotingMedia3 { get; }

        ImageViewAsync QuotingMedia4 { get; }
    }
}