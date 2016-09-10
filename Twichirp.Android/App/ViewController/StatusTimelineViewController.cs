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
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.View;
using Twichirp.Android.App.View.Holder;
using Twichirp.Core.App.ViewModel;
using static Android.Support.V7.Widget.RecyclerView;

namespace Twichirp.Android.App.ViewController {
    public class StatusTimelineViewController : BaseViewController<IStatusTimelineView,StatusTimelineViewModel> {

        private const int loadingType = 1;
        private const int preStatusTypeParam = 10;

        private ReactiveCollectionAdapter<BaseViewModel> adapter;

        public StatusTimelineViewController(IStatusTimelineView view,StatusTimelineViewModel viewModel) : base(view,viewModel) {
            view.OnCreateEventHandler += onCreate;
            view.OnDestoryEventHandler += onDestroy;
        }

        private void onCreate(object sender,LifeCycleEventArgs e) {
            adapter = ViewModel.Timeline.ToAdapter(adapterViewSelect,adapterViewCreate);
            adapter.LastItemVisibleCommand.Subscribe(x => ViewModel.LoadMoreComannd.Execute()).AddTo(Disposable);
            View.RecyclerView.SetLayoutManager(new LinearLayoutManager(View.RecyclerView.Context));
            View.RecyclerView.SetAdapter(adapter);
            View.RecyclerView.AddItemDecoration(new DividerItemDecoration());
            View.SwipeRefrech.SetBinding(x => x.Refreshing,ViewModel.IsLoading);
            View.SwipeRefrech.Refresh += onRefresh;
        }

        private void onDestroy(object sender,LifeCycleEventArgs e) {
            adapter.Dispose();
        }

        private ViewHolder adapterViewCreate(ViewGroup parent,int itemType) {
            switch(itemType) {
                case loadingType:
                    return new LoadingHolder(View,View,parent);
                case preStatusTypeParam + StatusViewModel.NormalTweet:
                    return new StatusHolder(View,View,parent);
                case preStatusTypeParam + StatusViewModel.RetweetedNormalTweet:
                    return new StatusHolder(View,View,parent);
            }
            return null;
        }

        private int adapterViewSelect(BaseViewModel viewModel,int position) {
            if(viewModel is LoadingViewModel) {
                return loadingType;
            }
            if(viewModel is StatusViewModel) {
                return preStatusTypeParam + (viewModel as StatusViewModel).ToStatusType();
            }
            return 0;
        }

        public void onRefresh(object sender,EventArgs args) {
            ViewModel.LoadCommand.Execute();
        }
    }
}