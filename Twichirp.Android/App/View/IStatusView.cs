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
using AView = Android.Views.View;

namespace Twichirp.Android.App.View {
    public interface IStatusView :IView,ILifeCycle{

        int StatusType { get; }

        TextView RetweetingUser { get; }

        TextView ReplyToUser { get; }

        TextView PrefixText { get; }

        TextView Text { get; }

        TextView SuffixText { get; }

        TextView Name { get; }

        TextView ScreenName { get; }

        ImageView Icon { get; }

        ImageView LockIcon { get; }

        ImageView VerifyIcon { get; }

        TextView DateTime { get; }

        LinearLayout MediaParent2 { get; }

        FrameLayout MediaFrame2 { get; }

        FrameLayout MediaFrame3 { get; }

        FrameLayout MediaFrame4 { get; }

        ImageView Media1 { get; }

        ImageView Media2 { get; }

        ImageView Media3 { get; }

        ImageView Media4 { get; }

        ImageView MediaPlay1 { get; }

        ImageView MediaPlay2 { get; }

        ImageView MediaPlay3 { get; }

        ImageView MediaPlay4 { get; }

        AView MediaClickable1 { get; }

        AView MediaClickable2 { get; }

        AView MediaClickable3 { get; }

        AView MediaClickable4 { get; }

        FrameLayout ReplyIconClickable { get; }

        ImageView ReplyIcon { get; }

        FrameLayout RetweetIconClickable { get; }

        ImageView RetweetIcon { get; }

        TextView RetweetCount { get; }

        FrameLayout FavoriteIconClickable { get; }

        ImageView FavoriteIcon { get; }

        TextView FavoriteCount { get; }

        AView ClickableView { get; }
        
        AView QuotingClickable { get; }
        
        TextView QuotingName { get; }
        
        TextView QuotingScreenName { get; }
        
        TextView QuotingPrefixText { get; }
        
        TextView QuotingText { get; }
        
        TextView QuotingSuffixText { get; }

        LinearLayout QuotingMediaParent2 { get; }

        ImageView QuotingMedia1 { get; }

        ImageView QuotingMedia2 { get; }

        ImageView QuotingMedia3 { get; }

        ImageView QuotingMedia4 { get; }
    }
}