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
using CoreTweet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.App.Model;
using Twichirp.Core.Model;
using Twichirp.Core.App.Event;

namespace Twichirp.Core.App.ViewModel {
    public class StatusTimelineViewModel : BaseViewModel {

        protected StatusTimelineModel StatusTimelineModel;

        public ReactiveCollection<BaseViewModel> Timeline { get; }
        public ReadOnlyReactiveProperty<bool> IsLoading { get; }
        public ReactiveCommand<string> ShowMessageCommand { get; } = new ReactiveCommand<string>();
        public ReactiveCommand LoadCommand { get; } = new ReactiveCommand();
        public ReactiveCommand LoadMoreComannd { get; } = new ReactiveCommand();

        public StatusTimelineViewModel(ITwichirpApplication application,Timeline<IEnumerable<Status>> timelineResource,Account account) : base(application) {
            StatusTimelineModel = new StatusTimelineModel(application,timelineResource,account);
            Timeline = StatusTimelineModel.Timeline;
            IsLoading = StatusTimelineModel.ObserveProperty(x => x.IsLoading).ToReadOnlyReactiveProperty().AddTo(Disposable);

            Timeline.CollectionChanged += collectionChanged;
            Observable.FromEventPattern<EventArgs<string>>(x => StatusTimelineModel.ErrorMessageCreated += x,x => StatusTimelineModel.ErrorMessageCreated -= x)
                .SubscribeOnUIDispatcher()
                .Subscribe(x => ShowMessageCommand.Execute(x.EventArgs.EventData))
                .AddTo(Disposable);
            LoadCommand.Subscribe(x => StatusTimelineModel.Load());
            LoadMoreComannd.Subscribe(x => StatusTimelineModel.LoadMore());

            Observable.FromEventPattern<StatusEventArgs>(x => Application.TwitterEvent.StatusUpdated += x,x => Application.TwitterEvent.StatusUpdated -= x)
                .SubscribeOnUIDispatcher()
                .Subscribe(x => StatusTimelineModel.NotifyStatusUpdate(x.EventArgs.Account,x.EventArgs.Status))
                .AddTo(Disposable);
        }

        private void collectionChanged(object sender,NotifyCollectionChangedEventArgs e) {
            if(e.Action == NotifyCollectionChangedAction.Add) {
                foreach(var s in e.NewItems) {
                    if(s is LoadingViewModel) {
                        LoadingViewModel loadingViewModel = s as LoadingViewModel;
                        loadingViewModel.LoadCommand.Subscribe(x => StatusTimelineModel.Load(loadingViewModel)).AddTo(Disposable);
                    }
                }
            } else if(e.Action == NotifyCollectionChangedAction.Replace) {
                foreach(var s in e.NewItems) {
                    if(s is LoadingViewModel) {
                        LoadingViewModel loadingViewModel = s as LoadingViewModel;
                        loadingViewModel.LoadCommand.Subscribe(x => StatusTimelineModel.Load(loadingViewModel)).AddTo(Disposable);
                    }
                }
            }
        }

        public override void Dispose() {
            base.Dispose();
            Timeline.CollectionChanged -= collectionChanged;
        }
    }
}
