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
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Twichirp.Android.App.ViewController;
using Twichirp.Core.App.ViewModel;
using Twichirp.Core.Model;
using AView = Android.Views.View;

namespace Twichirp.Android.App.View.Fragment {
    public class StatusTimelineFragment : BaseFragment,IStatusTimelineView {

        private StatusTimelineViewModel statusTimelineViewModel;
        private StatusTimelineViewController statusTimelineViewController;

        public RecyclerView RecyclerView { get; private set; }

        public SwipeRefreshLayout SwipeRefrech { get; private set; }

        public override AView OnCreateView(LayoutInflater inflater,ViewGroup container,Bundle savedInstanceState) {
            AView view =  inflater.Inflate(Resource.Layout.StatusTimelineFragment, container, false);
            RecyclerView = view.FindViewById<RecyclerView>(Resource.Id.RecyclerView);
            SwipeRefrech = view.FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeRefresh);
            var account = TwichirpApplication.AccountManager[TwichirpApplication.SettingManager.Accounts.DefaultAccountId];
            var timelineResource = Timeline<IEnumerable<CoreTweet.Status>>.HomeTimeline();
            statusTimelineViewModel = new StatusTimelineViewModel(TwichirpApplication,timelineResource,account);
            statusTimelineViewController = new StatusTimelineViewController(this,statusTimelineViewModel);
            return view;
        }

        public override void OnDestroy() {
            base.OnDestroy();
            RecyclerView?.Dispose();
            SwipeRefrech?.Dispose();
        }
    }
}