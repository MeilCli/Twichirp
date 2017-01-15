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
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content.Res;
using Android.Support.V4.Graphics.Drawable;
using Android.Util;
using Android.Views;
using Android.Widget;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.Extensions;
using Twichirp.Core.App.DataHolder;
using Twichirp.Core.App.ViewModel;
using Twichirp.Core.Extensions;

namespace Twichirp.Android.App.DataHolder {
    public class StatusDataHolder : BaseDataHolder {
        public ViewStates VisibleRetweetingUser { get; private set; }

        public ViewStates VisibleReplyToUser { get; private set; }

        public ViewStates VisibleLockIcon { get; private set; }
        public ViewStates VisibleVerifyIcon { get; private set; }

        public ViewStates VisivleMediaParent2 { get; private set; }
        public ViewStates VisivleMedia2 { get; private set; }
        public ViewStates VisivleMedia3 { get; private set; }
        public ViewStates VisivleMedia4 { get; private set; }
        public bool[] MeadiaIsVideoOrGif { get; private set; }

        public bool IsRetweetIconDisabled { get; private set; }
        public int RetweetDrawableTint { get; private set; }
        public ViewStates VisibleRetweetCount { get; private set; }
        public int FavoriteDrawableTint { get; private set; }
        public ViewStates VisibleFavoriteCount { get; private set; }

        public ViewStates VisivleQuotingMediaParent2 { get; private set; }
        public ViewStates VisivleQuotingMedia2 { get; private set; }
        public ViewStates VisivleQuotingMedia3 { get; private set; }
        public ViewStates VisivleQuotingMedia4 { get; private set; }

        public StatusDataHolder(StatusViewModel viewModel,Context context) {
            VisibleRetweetingUser = viewModel.IsRetweeting.Map(x => x == true ? ViewStates.Visible : ViewStates.Gone);

            VisibleReplyToUser = viewModel.InReplyToScreenName.Map(x => x == null ? ViewStates.Gone : ViewStates.Visible);

            VisibleLockIcon = viewModel.IsProtected.Map(x => x == true ? ViewStates.Visible : ViewStates.Gone);
            VisibleVerifyIcon = viewModel.IsVerified.Map(x => x == true ? ViewStates.Visible : ViewStates.Gone);

            VisivleMediaParent2 = viewModel.Media.Map(x => x.Count() >= 2 ? ViewStates.Visible : ViewStates.Gone);
            VisivleMedia2 = viewModel.Media.Map(x => x.Count() >= 2 ? ViewStates.Visible : ViewStates.Gone);
            VisivleMedia3 = viewModel.Media.Map(x => x.Count() >= 3 ? ViewStates.Visible : ViewStates.Gone);
            VisivleMedia4 = viewModel.Media.Map(x => x.Count() >= 4 ? ViewStates.Visible : ViewStates.Gone);
            MeadiaIsVideoOrGif = viewModel.Media.Select(x => (x?.VideoInfo?.Variants.Length ?? 0) > 0 ? true : false).ToArray();

            IsRetweetIconDisabled = viewModel.IsProtected;
            RetweetDrawableTint = viewModel.IsRetweeted.Map(x => toRetweetDrawableTint(context,x));
            VisibleRetweetCount = viewModel.RetweetCount.Map(x => x > 0 ? ViewStates.Visible : ViewStates.Invisible);
            FavoriteDrawableTint = viewModel.IsFavorited.Map(x => toFavoriteDrawableTint(context,x));
            VisibleFavoriteCount = viewModel.FavoriteCount.Map(x => x > 0 ? ViewStates.Visible : ViewStates.Invisible);

            VisivleQuotingMediaParent2 = viewModel.QuotedMedia.Map(x => (x?.Count() ?? 0) >= 2 ? ViewStates.Visible : ViewStates.Gone);
            VisivleQuotingMedia2 = viewModel.QuotedMedia.Map(x => (x?.Count() ?? 0) >= 2 ? ViewStates.Visible : ViewStates.Gone);
            VisivleQuotingMedia3 = viewModel.QuotedMedia.Map(x => (x?.Count() ?? 0) >= 3 ? ViewStates.Visible : ViewStates.Gone);
            VisivleQuotingMedia4 = viewModel.QuotedMedia.Map(x => (x?.Count() ?? 0) >= 4 ? ViewStates.Visible : ViewStates.Gone);

        }

        private int toRetweetDrawableTint(Context context,bool isRetweeted) {
            if(IsRetweetIconDisabled) {
                return ResourcesCompat.GetColor(context.Resources,Resource.Color.Grey300,null);
            } else if(isRetweeted) {
                return ResourcesCompat.GetColor(context.Resources,Resource.Color.Retweet,null);
            }
            return ResourcesCompat.GetColor(context.Resources,Resource.Color.Grey600,null);
        }

        private int toFavoriteDrawableTint(Context context,bool isFavorited) {
            if(isFavorited) {
                return ResourcesCompat.GetColor(context.Resources,Resource.Color.Favorite,null);
            }
            return ResourcesCompat.GetColor(context.Resources,Resource.Color.Grey600,null);
        }
    }
}