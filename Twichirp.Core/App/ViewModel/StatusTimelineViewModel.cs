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
using Microsoft.Practices.Unity;
using CStatus = CoreTweet.Status;
using Twichirp.Core.DataObjects;
using Twichirp.Core.Repositories;
using Twichirp.Core.Services;
using Twichirp.Core.Events;
using Twichirp.Core.Settings;

namespace Twichirp.Core.App.ViewModel {

    /// <summary>
    /// Recommend to use UnityContainer
    /// </summary>
    public class StatusTimelineViewModel : BaseViewModel {

        private const string constructorTimelineRepository = "timelineRepository";
        private const string constructorAccount = "account";

        public static StatusTimelineViewModel Resolve(UnityContainer unityContainer,ITimelineRepository timelineRepository,ImmutableAccount account) {
            return unityContainer.Resolve<StatusTimelineViewModel>(
                new ParameterOverride(constructorTimelineRepository,timelineRepository),
                new ParameterOverride(constructorAccount,account)
            );
        }

        private ImmutableAccount account;
        protected StatusTimelineModel StatusTimelineModel;

        public string Json {
            get {
                return StatusTimelineModel.ExportJson();
            }
        }

        public ReadOnlyReactiveCollection<BaseViewModel> Timeline { get; }
        public ReadOnlyReactiveProperty<bool> IsLoading { get; }
        public ReactiveCommand<string> ShowMessageCommand { get; } = new ReactiveCommand<string>();
        public AsyncReactiveCommand<ITimelineRepository> LoadCommand { get; } = new AsyncReactiveCommand<ITimelineRepository>();
        public AsyncReactiveCommand LoadMoreComannd { get; } = new AsyncReactiveCommand();

        public StatusTimelineViewModel(ITwitterEventService twitterEventService,SettingManager settingManager,ImmutableAccount account,ITimelineRepository timelineRepository) {
            this.account = account;
            StatusTimelineModel = new StatusTimelineModel(twitterEventService,settingManager,account,timelineRepository);
            Timeline = StatusTimelineModel.Timeline.ToReadOnlyReactiveCollection(toViewModel).AddTo(Disposable);
            Timeline.CollectionChangedAsObservable().Subscribe(x => collectionChanged(x)).AddTo(Disposable);
            IsLoading = StatusTimelineModel.ObserveProperty(x => x.IsLoading).ToReadOnlyReactiveProperty().AddTo(Disposable);

            Observable.FromEventPattern<EventArgs<string>>(x => StatusTimelineModel.ErrorMessageCreated += x,x => StatusTimelineModel.ErrorMessageCreated -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => ShowMessageCommand.Execute(x.EventArgs.EventData))
                .AddTo(Disposable);
            LoadCommand.Subscribe(x => StatusTimelineModel.LoadAsync(x));
            LoadMoreComannd.Subscribe(x => StatusTimelineModel.LoadMoreAsync());

            Observable.FromEventPattern<StatusEventArgs>(x => twitterEventService.StatusUpdated += x,x => twitterEventService.StatusUpdated -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(async x => await StatusTimelineModel.NotifyStatusUpdatedAsync(x.EventArgs.Account,x.EventArgs.Status))
                .AddTo(Disposable);
            Observable.FromEventPattern<UserEventArgs>(x => twitterEventService.UserUpdated += x,x => twitterEventService.UserUpdated -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(async x => await StatusTimelineModel.NotifyUserUpdatedAsync(x.EventArgs.Account,x.EventArgs.User))
                .AddTo(Disposable);
        }

        private BaseViewModel toViewModel(BaseModel model) {
            if(model is StatusModel) {
                return new StatusViewModel(model as StatusModel,account);
            }
            if(model is LoadingModel) {
                return new LoadingViewModel(model as LoadingModel);
            }
            return null;
        }

        private void collectionChanged(NotifyCollectionChangedEventArgs e) {
            if(e.Action == NotifyCollectionChangedAction.Add) {
                foreach(var s in e.NewItems) {
                    if(s is LoadingViewModel) {
                        LoadingViewModel loadingViewModel = s as LoadingViewModel;
                        loadingViewModel.LoadCommand.Subscribe(async x => await StatusTimelineModel.LoadAsync(loadingViewModel.LoadingModel)).AddTo(Disposable);
                    }
                }
            } else if(e.Action == NotifyCollectionChangedAction.Replace) {
                foreach(var s in e.NewItems) {
                    if(s is LoadingViewModel) {
                        LoadingViewModel loadingViewModel = s as LoadingViewModel;
                        loadingViewModel.LoadCommand.Subscribe(async x => await StatusTimelineModel.LoadAsync(loadingViewModel.LoadingModel)).AddTo(Disposable);
                    }
                }
            }
        }

    }
}
