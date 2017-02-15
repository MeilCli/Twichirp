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
using Twichirp.Core.Model;
using Twichirp.Core.Extensions;
using Twichirp.Core.App.Event;
using Newtonsoft.Json;

namespace Twichirp.Core.App.Model {
    public class StatusTimelineModel : BaseModel {

        public event EventHandler<EventArgs<string>> ErrorMessageCreated;

        private SemaphoreSlim slim = new SemaphoreSlim(1,1);
        public ReactiveCollection<BaseModel> Timeline { get; } = new ReactiveCollection<BaseModel>();
        private List<StatusModel> _timeline { get; } = new List<StatusModel>();
        private Timeline<IEnumerable<Status>> timelineResource;
        private Account account;

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

        public StatusTimelineModel(ITwichirpApplication application,Timeline<IEnumerable<Status>> timelineResource,Account account) : base(application) {
            this.timelineResource = timelineResource;
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

        public async Task LoadAsync(Timeline<IEnumerable<Status>> timelineResource = null) {
            timelineResource = timelineResource ?? this.timelineResource;
            if(IsLoading) {
                return;
            }
            await slim.WaitAsync();
            IsLoading = true;
            try {
                int count = Application.SettingManager.Timeline.Count;
                if(_timeline.Count >= 1) {
                    IEnumerable<Status> response = await timelineResource.Load(account,count,sinceId: _timeline[0].Id);
                    if(response.Count() == count || response.Count() == count - 1) {
                        // 間にツイートがある場合でも(count-1)個しかないことがある
                        var loadingModel = new LoadingModel(Application);
                        Timeline.InsertOnScheduler(0,loadingModel);
                    }
                    int index = 0;
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(Application,x))) {
                        _timeline.Insert(index,s);
                        Timeline.InsertOnScheduler(index,s);
                        index++;
                    }
                    if(response.Any()) {
                        canLoadMore = true;
                    }
                } else {
                    foreach(var s in (await timelineResource.Load(account,count)).Where(x => x.IsValid()).Select(x => new StatusModel(Application,x))) {
                        _timeline.Add(s);
                        Timeline.AddOnScheduler(s);
                    }
                    if(_timeline.Count > 0) {
                        canLoadMore = true;
                    } else {
                        canLoadMore = false;
                    }
                }

                if(_timeline.Count > Application.SettingManager.Timeline.OwnedNumber) {
                    int removeCount = _timeline.Count - Application.SettingManager.Timeline.OwnedNumber;
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

        public async Task LoadAsync(LoadingModel target,Timeline<IEnumerable<Status>> timelineResource = null) {
            timelineResource = timelineResource ?? this.timelineResource;
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
                int count = Application.SettingManager.Timeline.Count;
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
                    IEnumerable<Status> response = await timelineResource.Load(account,count,sinceId: nextStatus.Id,maxId: previousStatus.Id - 1);
                    if(response.Count() < count) {
                        Timeline.RemoveOnScheduler(target);
                    }
                    int _index = _timeline.IndexOf(previousStatus) + 1;
                    int index = Timeline.IndexOf(previousStatus) + 1;
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(Application,x))) {
                        _timeline.Insert(_index,s);
                        Timeline.InsertOnScheduler(index,s);
                        _index++;
                        index++;
                    }
                } else if(previousStatus != null) {
                    IEnumerable<Status> response = await timelineResource.Load(account,count,maxId: previousStatus.Id - 1);
                    Timeline.RemoveOnScheduler(target);
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(Application,x))) {
                        _timeline.Add(s);
                        Timeline.AddOnScheduler(s);
                    }
                } else if(nextStatus != null) {
                    IEnumerable<Status> response = await timelineResource.Load(account,count,sinceId: nextStatus.Id);
                    Timeline.RemoveOnScheduler(target);
                    int index = 0;
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(Application,x))) {
                        _timeline.Insert(index,s);
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

        public async Task LoadMoreAsync(Timeline<IEnumerable<Status>> timelineResource = null) {
            timelineResource = timelineResource ?? this.timelineResource;
            if(IsLoading) {
                return;
            }
            if(canLoadMore == false) {
                return;
            }
            await slim.WaitAsync();
            IsLoading = true;
            try {
                int count = Application.SettingManager.Timeline.Count;
                IEnumerable<Status> response = await timelineResource.Load(account,count,maxId: _timeline[_timeline.Count - 1].Id - 1);
                foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusModel(Application,x))) {
                    _timeline.Add(s);
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
                    _timeline.Remove(r as StatusModel);
                }
            }
        }

        public async Task NotifyStatusUpdatedAsync(Account account,Status status) {
            if(this.account.Id != account.Id) {
                return;
            }
            await slim.WaitAsync();
            try {
                IEnumerable<StatusModel> changeStatus = await Task.Run(() => {
                    return _timeline
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

        public async Task NotifyUserUpdatedAsync(Account account,User user) {
            await slim.WaitAsync();
            try {
                IEnumerable<UserModel> changeUser = await Task.Run(() => {
                    return _timeline
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
