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
using Twichirp.Core.DataObjects;
using Twichirp.Core.DataRepositories;
using Microsoft.Practices.Unity;
using AView = Android.Views.View;
using SFragment = Android.Support.V4.App.Fragment;
using Twichirp.Core.Repositories;

// 未使用フィールドの警告非表示
#pragma warning disable 0414

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
            public ImmutableAccount Account { get; }

            public Parameter(StatusTimelineFragmentType type,ImmutableAccount account) {
                Type = type;
                Account = account;
            }

            public virtual void Put(Bundle bundle) {
                bundle.PutLong(argumentAccount,Account.Id);
                bundle.PutInt(argumentType,(int)Type);
            }
        }

        public class HomeParameter : Parameter {

            public HomeParameter(ImmutableAccount account) : base(StatusTimelineFragmentType.Home,account) { }
        }

        public class MentionParameter : Parameter {

            public MentionParameter(ImmutableAccount account) : base(StatusTimelineFragmentType.Mention,account) { }
        }

        public class UserParameter : Parameter {

            public long UserId { get; }

            public UserParameter(ImmutableAccount account,long userId) : base(StatusTimelineFragmentType.User,account) {
                UserId = userId;
            }

            public override void Put(Bundle bundle) {
                base.Put(bundle);
                bundle.PutLong(argumentUser,UserId);
            }
        }

        public class FavoriteParameter : Parameter {

            public long UserId { get; }

            public FavoriteParameter(ImmutableAccount account,long userId) : base(StatusTimelineFragmentType.Favorite,account) {
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
            IAccountRepository accountRepository = TwichirpApplication.Resolve<IAccountRepository>();
            var account = accountRepository[accountId];
            var type = (StatusTimelineFragmentType)Enum.ToObject(typeof(StatusTimelineFragmentType),Arguments.GetInt(argumentType));
            long userId = Arguments.GetLong(argumentUser,-1);

            ITimelineRepository timelineRepository;
            switch(type) {
                default:
                case StatusTimelineFragmentType.Home:
                    timelineRepository = new HomeTimelineRepository();
                    break;
                case StatusTimelineFragmentType.Mention:
                    timelineRepository = new MentionTimelineRepository();
                    break;
                case StatusTimelineFragmentType.User:
                    timelineRepository = new UserTimelineRepository(userId);
                    break;
                case StatusTimelineFragmentType.Favorite:
                    timelineRepository = new FavoriteTimelineRepository(userId);
                    break;
            }
            statusTimelineViewModel = StatusTimelineViewModel.Resolve(TwichirpApplication.UnityContainer,timelineRepository,account);
            statusTimelineViewController = new StatusTimelineViewController(this,statusTimelineViewModel);
        }

        public override AView OnCreateView(LayoutInflater inflater,ViewGroup container,Bundle savedInstanceState) {
            AView view = inflater.Inflate(Resource.Layout.StatusTimelineFragment,container,false);
            RecyclerView = view.FindViewById<RecyclerView>(Resource.Id.RecyclerView);
            SwipeRefrech = view.FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeRefresh);
            return view;
        }

        public override void OnDestroyView() {
            base.OnDestroyView();
            RecyclerView?.Dispose();
            SwipeRefrech?.Dispose();
        }

        public bool Equals(StatusTimelineFragmentType type,ImmutableAccount account) {
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