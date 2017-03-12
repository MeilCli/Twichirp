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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twichirp.Core.App.ViewModel;
using Twichirp.Core.Extensions;
using Newtonsoft.Json;
using Twichirp.Core.DataObjects;
using CStatus = CoreTweet.Status;
using CUser = CoreTweet.User;
using Twichirp.Core.UseCases;
using Twichirp.Core.Repositories;
using Twichirp.Core.Services;
using Twichirp.Core.Events;
using Twichirp.Core.Settings;

namespace Twichirp.Core.Models {

    public class StatusTimelineModel : BaseModel {

        public event EventHandler<EventArgs<string>> ErrorMessageCreated;

        private SemaphoreSlim slim = new SemaphoreSlim(1,1);
        public ReactiveCollection<BaseModel> Timeline { get; } = new ReactiveCollection<BaseModel>();
        private List<StatusModel> statusTimeline { get; } = new List<StatusModel>();
        private TimelineUseCase timelineUseCase;
        private ImmutableAccount account;
        private ITwitterEventService twitterEventService;
        private SettingManager settingManager;

        private bool _isLoading;
        public bool IsLoading {
            get {
                return _isLoading;
            }
            private set {
                SetValue(ref _isLoading,value,nameof(IsLoading));
            }
        }

        private bool _canLoadMore = true;
        private bool canLoadMore {
            get {
                return _canLoadMore;
            }
            set {
                _canLoadMore = value;
            }
        }

        public StatusTimelineModel(ITwitterEventService twitterEventService,SettingManager settingManager,ImmutableAccount account,ITimelineRepository defaultRepository) {
            this.twitterEventService = twitterEventService;
            this.timelineUseCase = new TimelineUseCase(twitterEventService,defaultRepository);
            this.settingManager = settingManager;
            this.account = account;
        }

        /// <summary>
        /// Export Statuses
        /// </summary>
        /// <returns>Array of string</returns>
        public string ExportJson() {
            var list = new List<string>();
            foreach(var viewModel in Timeline) {
                if(viewModel is StatusModel == false) {
                    break;
                }
                list.Add((viewModel as StatusModel).ExportJson());
            }
            return JsonConvert.SerializeObject(list);
        }

        public async Task LoadAsync(ITimelineRepository timelineRepository = null) {
            if(IsLoading) {
                return;
            }
            await slim.WaitAsync();
            IsLoading = true;
            try {
                int count = settingManager.Timeline.Count;
                if(statusTimeline.Count >= 1) {
                    IEnumerable<CStatus> response = await timelineUseCase.Load(account,count,sinceId: statusTimeline[0].Id);
                    if(response.Count() == count || response.Count() == count - 1) {
                        // 間にツイートがある場合でも(count-1)個しかないことがある
                        var loadingModel = new LoadingModel();
                        Timeline.InsertOnScheduler(0,loadingModel);
                    }
                    int index = 0;
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(twitterEventService,x))) {
                        statusTimeline.Insert(index,s);
                        Timeline.InsertOnScheduler(index,s);
                        index++;
                    }
                    if(response.Any()) {
                        canLoadMore = true;
                    }
                } else {
                    foreach(var s in (await timelineUseCase.Load(account,count)).Where(x => x.IsValid()).Select(x => new StatusModel(twitterEventService,x))) {
                        statusTimeline.Add(s);
                        Timeline.AddOnScheduler(s);
                    }
                    if(statusTimeline.Count > 0) {
                        canLoadMore = true;
                    } else {
                        canLoadMore = false;
                    }
                }

                if(statusTimeline.Count > settingManager.Timeline.OwnedNumber) {
                    int removeCount = statusTimeline.Count - settingManager.Timeline.OwnedNumber;
                    removeLast(removeCount);
                }
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }

            // TLのUIの更新中に読み込み可能になるのを防ぐ(暫定) 
            await Task.Delay(800);
            IsLoading = false;
        }

        public async Task LoadAsync(LoadingModel target,ITimelineRepository timelineRepository = null) {
            if(IsLoading) {
                return;
            }
            if(target.IsLoading) {
                return;
            }
            await slim.WaitAsync();
            IsLoading = true;
            target.StartLoading();
            try {
                int count = settingManager.Timeline.Count;
                int targetIndex = Timeline.IndexOf(target);
                StatusModel previousStatus = null;
                StatusModel nextStatus = null;

                if(targetIndex != -1 && Timeline.Count >= 3 && 0 < targetIndex && targetIndex < Timeline.Count - 1) {
                    previousStatus = Timeline[targetIndex - 1] as StatusModel;
                    nextStatus = Timeline[targetIndex + 1] as StatusModel;
                } else if(targetIndex == 0 && Timeline.Count >= 2) {
                    nextStatus = Timeline[targetIndex + 1] as StatusModel;
                } else if(targetIndex == Timeline.Count - 1 && Timeline.Count >= 2) {
                    previousStatus = Timeline[targetIndex - 1] as StatusModel;
                } else {
                }
                if(previousStatus != null && nextStatus != null) {
                    IEnumerable<CStatus> response = await timelineUseCase.Load(account,count,sinceId: nextStatus.Id,maxId: previousStatus.Id - 1);
                    if(response.Count() < count) {
                        Timeline.RemoveOnScheduler(target);
                    }
                    int statusIndex = statusTimeline.IndexOf(previousStatus) + 1;
                    int index = Timeline.IndexOf(previousStatus) + 1;
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(twitterEventService,x))) {
                        statusTimeline.Insert(statusIndex,s);
                        Timeline.InsertOnScheduler(index,s);
                        statusIndex++;
                        index++;
                    }
                } else if(previousStatus != null) {
                    IEnumerable<CStatus> response = await timelineUseCase.Load(account,count,maxId: previousStatus.Id - 1);
                    Timeline.RemoveOnScheduler(target);
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(twitterEventService,x))) {
                        statusTimeline.Add(s);
                        Timeline.AddOnScheduler(s);
                    }
                } else if(nextStatus != null) {
                    IEnumerable<CStatus> response = await timelineUseCase.Load(account,count,sinceId: nextStatus.Id);
                    Timeline.RemoveOnScheduler(target);
                    int index = 0;
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(twitterEventService,x))) {
                        statusTimeline.Insert(index,s);
                        Timeline.InsertOnScheduler(index,s);
                        index++;
                    }
                }
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
            IsLoading = false;
            target.StopLoading();
        }

        public async Task LoadMoreAsync(ITimelineRepository timelineRepository = null) {
            if(IsLoading) {
                return;
            }
            if(canLoadMore == false) {
                return;
            }
            await slim.WaitAsync();
            IsLoading = true;
            try {
                int count = settingManager.Timeline.Count;
                IEnumerable<CStatus> response = await timelineUseCase.Load(account,count,maxId: statusTimeline[statusTimeline.Count - 1].Id - 1);
                foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(twitterEventService,x))) {
                    statusTimeline.Add(s);
                    Timeline.AddOnScheduler(s);
                }
                if(response.Any()) {
                    canLoadMore = true;
                } else {
                    canLoadMore = false;
                }
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
            IsLoading = false;
        }

        private void removeLast(int count) {
            var removes = Timeline.Reverse().Take(count).ToList();
            foreach(var r in removes) {
                Timeline.RemoveOnScheduler(r);
                if(r is StatusModel) {
                    statusTimeline.Remove(r as StatusModel);
                }
            }
        }

        public async Task NotifyStatusUpdatedAsync(ImmutableAccount account,CStatus status) {
            if(this.account.Id != account.Id) {
                return;
            }
            await slim.WaitAsync();
            try {
                IEnumerable<StatusModel> changeStatus = await Task.Run(() => {
                    return statusTimeline
                    .Where(x => x.DeploymentStatus().Any(y => y.Id == status.Id))
                    .ToList();
                });
                foreach(var s in changeStatus) {
                    foreach(var m in s.DeploymentStatus().Where(x => x.Id == status.Id)) {
                        m.SetStatus(status);
                    }
                }
            } finally {
                slim.Release();
            }
        }

        public async Task NotifyUserUpdatedAsync(ImmutableAccount account,CUser user) {
            await slim.WaitAsync();
            try {
                IEnumerable<UserModel> changeUser = await Task.Run(() => {
                    return statusTimeline
                    .SelectMany(x => x.DeploymentStatus())
                    .Select(x => x.User)
                    .Where(x => x.Id == user.Id)
                    .ToList();
                });
                foreach(var u in changeUser) {
                    u.SetUser(user);
                }
            } finally {
                slim.Release();
            }
        }

    }
}
