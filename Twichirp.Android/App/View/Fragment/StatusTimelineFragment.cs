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
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Twichirp.Android.App.ViewController;
using Twichirp.Core.App.ViewModel;
using Twichirp.Core.Model;
using AView = Android.Views.View;
using SFragment = Android.Support.V4.App.Fragment;

namespace Twichirp.Android.App.View.Fragment {

    public enum StatusTimelineFragmentType {
        Home = 0,
        Mention = 1,
        User = 2,
        Favorite = 3
    }

    public class StatusTimelineFragment : BaseFragment, IStatusTimelineView, IAppBarOffsetChangeEventRaise {

        private const string argumentType = "arg_type";
        private const string argumentAccount = "arg_account";
        private const string argumentUser = "arg_user";

        public class Parameter {

            public StatusTimelineFragmentType Type { get; }
            public Account Account { get; }

            public Parameter(StatusTimelineFragmentType type,Account account) {
                Type = type;
                Account = account;
            }

            public virtual void Put(Bundle bundle) {
                bundle.PutLong(argumentAccount,Account.Id);
                bundle.PutInt(argumentType,(int)Type);
            }
        }

        public class HomeParameter : Parameter {

            public HomeParameter(Account account) : base(StatusTimelineFragmentType.Home,account) { }
        }

        public class MentionParameter : Parameter {

            public MentionParameter(Account account) : base(StatusTimelineFragmentType.Mention,account) { }
        }

        public class UserParameter : Parameter {

            public long UserId { get; }

            public UserParameter(Account account,long userId) : base(StatusTimelineFragmentType.User,account) {
                UserId = userId;
            }

            public override void Put(Bundle bundle) {
                base.Put(bundle);
                bundle.PutLong(argumentUser,UserId);
            }
        }

        public class FavoriteParameter : Parameter {

            public long UserId { get; }

            public FavoriteParameter(Account account,long userId) : base(StatusTimelineFragmentType.Favorite,account) {
                UserId = userId;
            }

            public override void Put(Bundle bundle) {
                base.Put(bundle);
                bundle.PutLong(argumentUser,UserId);
            }
        }

        public static StatusTimelineFragment Make(Parameter parameter) {
            var fragment = new StatusTimelineFragment();
            var bundle = new Bundle();
            parameter.Put(bundle);
            fragment.Arguments = bundle;
            return fragment;
        }

        public event EventHandler<AppBarOffsetChangedEventArgs> AppBarOffsetChanged;

        private StatusTimelineViewModel statusTimelineViewModel;
        private StatusTimelineViewController statusTimelineViewController;

        public RecyclerView RecyclerView { get; private set; }

        public SwipeRefreshLayout SwipeRefrech { get; private set; }

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            long accountId = Arguments.GetLong(argumentAccount);
            var account = TwichirpApplication.AccountManager[accountId];
            var type = (StatusTimelineFragmentType)Enum.ToObject(typeof(StatusTimelineFragmentType),Arguments.GetInt(argumentType));
            long userId = Arguments.GetLong(argumentUser,-1);
            Timeline<IEnumerable<CoreTweet.Status>> timelineResource;
            switch(type) {
                default:
                case StatusTimelineFragmentType.Home:
                    timelineResource = Timeline<IEnumerable<CoreTweet.Status>>.HomeTimeline();
                    break;
                case StatusTimelineFragmentType.Mention:
                    timelineResource = Timeline<IEnumerable<CoreTweet.Status>>.MentionTimeline();
                    break;
                case StatusTimelineFragmentType.User:
                    timelineResource = Timeline<IEnumerable<CoreTweet.Status>>.UserTimeline(userId);
                    break;
                case StatusTimelineFragmentType.Favorite:
                    timelineResource = Timeline<IEnumerable<CoreTweet.Status>>.FavoriteTimeline(userId);
                    break;
            }
            statusTimelineViewModel = new StatusTimelineViewModel(TwichirpApplication,timelineResource,account);
            statusTimelineViewController = new StatusTimelineViewController(this,statusTimelineViewModel);
        }

        public override AView OnCreateView(LayoutInflater inflater,ViewGroup container,Bundle savedInstanceState) {
            AView view = inflater.Inflate(Android.Resource.Layout.StatusTimelineFragment,container,false);
            RecyclerView = view.FindViewById<RecyclerView>(Android.Resource.Id.RecyclerView);
            SwipeRefrech = view.FindViewById<SwipeRefreshLayout>(Android.Resource.Id.SwipeRefresh);
            return view;
        }

        public override void OnDestroyView() {
            base.OnDestroyView();
            RecyclerView?.Dispose();
            SwipeRefrech?.Dispose();
        }

        public bool Equals(StatusTimelineFragmentType type,Account account) {
            var arg = Arguments;
            if(arg == null) {
                return false;
            }
            long id = arg.GetLong(argumentAccount,-1);
            if(id == -1 || id != account.Id) {
                return false;
            }
            int t = arg.GetInt(argumentType,-1);
            if(t != (int)type) {
                return false;
            }
            return true;
        }

        public void RaiseAppBarOffsetChanged(AppBarOffsetChangedEventArgs args) {
            AppBarOffsetChanged?.Invoke(this,args);
        }
    }
}