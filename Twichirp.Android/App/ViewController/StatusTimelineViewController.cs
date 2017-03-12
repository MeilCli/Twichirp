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
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.View;
using Twichirp.Android.App.View.Holder;
using Twichirp.Android.Events;
using Twichirp.Android.Repositories;
using Twichirp.Core.DataObjects;
using Twichirp.Core.ViewModels;
using static Android.Support.V7.Widget.RecyclerView;
using CStatus = CoreTweet.Status;

namespace Twichirp.Android.App.ViewController {
    public class StatusTimelineViewController : BaseViewController<IStatusTimelineView,StatusTimelineViewModel> {

        private const string stateTimeline = "state_timeline";
        private const int loadingType = 1;
        private const int preStatusTypeParam = 10;

        private ReactiveCollectionAdapter<BaseViewModel> adapter;
        private CompositeDisposable onDestroyViewDisposable;

        public StatusTimelineViewController(IStatusTimelineView view,StatusTimelineViewModel viewModel) : base(view,viewModel) {
            Observable.FromEventPattern<LifeCycleEventArgs>(x => View.Created += x,x => View.Created -= x)
                .Subscribe(x => onCreate(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => View.Destroyed += x,x => View.Destroyed -= x)
                .Subscribe(x => onDestroy(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => View.ViewDestroyed += x,x => View.ViewDestroyed -= x)
                .Subscribe(x => onDestroyView(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => View.InstanceStateSaved += x,x => View.InstanceStateSaved -= x)
                .Subscribe(x => onSaveInstanceState(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<AppBarOffsetChangedEventArgs>(x => View.AppBarOffsetChanged += x,x => View.AppBarOffsetChanged -= x)
                .Subscribe(x => appBarOffsetChanged(x.Sender,x.EventArgs))
                .AddTo(Disposable);

            adapter = ViewModel.Timeline.ToAdapter(adapterViewSelect,adapterViewCreate);
            Observable.FromEventPattern<EventArgs>(x => adapter.LastItemShowed += x,x => adapter.LastItemShowed -= x)
                .Subscribe(x => ViewModel.LoadMoreComannd.Execute())
                .AddTo(Disposable);
            ViewModel.ShowMessageCommand.Subscribe(x => Toast.MakeText(View.ApplicationContext,x,ToastLength.Short).Show()).AddTo(Disposable);
        }

        private void onCreate(object sender,LifeCycleEventArgs e) {
            onDestroyViewDisposable = new CompositeDisposable();

            View.RecyclerView.SetLayoutManager(new LinearLayoutManager(View.RecyclerView.Context));
            View.RecyclerView.SetAdapter(adapter);
            View.RecyclerView.AddItemDecoration(new DividerItemDecoration(View.RecyclerView.Context) { Size = 2 });
            View.RecyclerView.SetItemViewCacheSize(0);
            View.SwipeRefrech.SetBinding(x => x.Refreshing,ViewModel.IsLoading).AddTo(onDestroyViewDisposable);
            View.SwipeRefrech.Refresh += onRefresh;
            if(adapter.ItemCount == 0) {
                if(e.State != null) {
                    // ソース書いたはいいもののFragmentの設定上ここは呼ばれることない気がする
                    var statuses = JsonConvert.DeserializeObject<List<string>>(e.State.GetString(stateTimeline));
                    var timeline = new JsonConvertedTimelineRepository(statuses);
                    ViewModel.LoadCommand.Execute(timeline);
                } else {
                    ViewModel.LoadCommand.Execute(null);
                }
            }
        }

        private void onSaveInstanceState(object sender,LifeCycleEventArgs args) {
            args.State.PutString(stateTimeline,ViewModel.Json);
        }

        private void onDestroy(object sender,LifeCycleEventArgs e) {
            adapter.Dispose();
        }

        private void onDestroyView(object sender,LifeCycleEventArgs e) {
            onDestroyViewDisposable.Dispose();
        }

        private void appBarOffsetChanged(object sender,AppBarOffsetChangedEventArgs args) {
            View.SwipeRefrech.Enabled = args.Offset == 0;
        }

        private ViewHolder adapterViewCreate(ViewGroup parent,int itemType) {
            switch(itemType) {
                case loadingType:
                    return LoadingHolder.Make(View);
                case preStatusTypeParam + StatusViewModel.NormalTweet:
                    return StatusHolder.Make(View);
                case preStatusTypeParam + StatusViewModel.MediaTweet:
                    return StatusMediaHolder.Make(View);
                case preStatusTypeParam + StatusViewModel.QuotedTweet:
                    return StatusQuotingHolder.Make(View);
                case preStatusTypeParam + StatusViewModel.QuotedInnerMediaTweet:
                    return StatusQuotingInnerMediaHolder.Make(View);
                case preStatusTypeParam + StatusViewModel.QuotedOuterMediaTweet:
                    return StatusQuotingOuterMediaHolder.Make(View);
                case preStatusTypeParam + StatusViewModel.QuotedInnerAndOuterMediaTweet:
                    return StatusQuotingInnerAndOuterMediaHolder.Make(View);
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
            if(ViewModel.LoadCommand.CanExecute()) {
                ViewModel.LoadCommand.Execute(null);
            }
        }

    }
}