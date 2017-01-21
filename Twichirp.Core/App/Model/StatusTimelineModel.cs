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

namespace Twichirp.Core.App.Model {
    public class StatusTimelineModel : BaseModel {

        public event EventHandler<EventArgs<string>> ErrorMessageCreated;

        private SemaphoreSlim slim = new SemaphoreSlim(1,1);
        public ReactiveCollection<BaseViewModel> Timeline { get; } = new ReactiveCollection<BaseViewModel>();
        private List<StatusViewModel> _timeline { get; } = new List<StatusViewModel>();
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

        public StatusTimelineModel(ITwichirpApplication application,Timeline<IEnumerable<Status>> timelineResource,Account account) : base(application) {
            this.timelineResource = timelineResource;
            this.account = account;
        }

        public async void Load(Timeline<IEnumerable<Status>> timelineResource = null) {
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
                    if(response.Count() == count) {
                        var loadingViewModel = new LoadingViewModel(Application);
                        Timeline.InsertOnScheduler(0,loadingViewModel);
                    }
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusViewModel(Application,x,account)).Reverse()) {
                        _timeline.Insert(0,s);
                        Timeline.InsertOnScheduler(0,s);
                    }
                } else {
                    foreach(var s in (await timelineResource.Load(account,count)).Where(x => x.IsValid()).Select(x => new StatusViewModel(Application,x,account)).Reverse()) {
                        _timeline.Insert(0,s);
                        Timeline.InsertOnScheduler(0,s);
                    }
                }
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
            IsLoading = false;
        }

        public async void Load(LoadingViewModel target,Timeline<IEnumerable<Status>> timelineResource = null) {
            timelineResource = timelineResource ?? this.timelineResource;
            if(IsLoading) {
                return;
            }
            if(target.IsLoaing.Value) {
                return;
            }
            await slim.WaitAsync();
            IsLoading = true;
            target.IsLoaing.Value = true;
            try {
                int count = Application.SettingManager.Timeline.Count;
                int targetIndex = Timeline.IndexOf(target);
                StatusViewModel previousStatus = null;
                StatusViewModel nextStatus = null;

                if(targetIndex != -1 && Timeline.Count >= 3 && 0 < targetIndex && targetIndex < Timeline.Count - 1) {
                    previousStatus = Timeline[targetIndex - 1] as StatusViewModel;
                    nextStatus = Timeline[targetIndex + 1] as StatusViewModel;
                } else if(targetIndex == 0 && Timeline.Count >= 2) {
                    nextStatus = Timeline[targetIndex + 1] as StatusViewModel;
                } else if(targetIndex == Timeline.Count - 1 && Timeline.Count >= 2) {
                    previousStatus = Timeline[targetIndex - 1] as StatusViewModel;
                } else {
                }
                if(previousStatus != null && nextStatus != null) {
                    IEnumerable<Status> response = await timelineResource.Load(account,count,sinceId: nextStatus.Id,maxId: previousStatus.Id - 1);
                    if(response.Count() < count) {
                        Timeline.RemoveOnScheduler(target);
                    }
                    int _index = _timeline.IndexOf(previousStatus) + 1;
                    int index = Timeline.IndexOf(previousStatus) + 1;
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusViewModel(Application,x,account)).Reverse()) {
                        _timeline.Insert(_index,s);
                        Timeline.InsertOnScheduler(index,s);
                    }
                } else if(previousStatus != null) {
                    IEnumerable<Status> response = await timelineResource.Load(account,count,maxId: previousStatus.Id - 1);
                    Timeline.RemoveOnScheduler(target);
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusViewModel(Application,x,account))) {
                        _timeline.Add(s);
                        Timeline.AddOnScheduler(s);
                    }
                } else if(nextStatus != null) {
                    IEnumerable<Status> response = await timelineResource.Load(account,count,sinceId: nextStatus.Id);
                    Timeline.RemoveOnScheduler(target);
                    foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusViewModel(Application,x,account)).Reverse()) {
                        _timeline.Insert(0,s);
                        Timeline.InsertOnScheduler(0,s);
                    }
                }
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
            IsLoading = false;
            target.IsLoaing.Value = false;
        }

        public async void LoadMore(Timeline<IEnumerable<Status>> timelineResource = null) {
            timelineResource = timelineResource ?? this.timelineResource;
            if(IsLoading) {
                return;
            }
            await slim.WaitAsync();
            IsLoading = true;
            try {
                int count = Application.SettingManager.Timeline.Count;
                IEnumerable<Status> response = await timelineResource.Load(account,count,maxId: _timeline[_timeline.Count - 1].Id - 1);
                foreach(var s in response.Where(x => x.IsValid()).Select(x => new StatusViewModel(Application,x,account))) {
                    _timeline.Add(s);
                    Timeline.AddOnScheduler(s);
                }
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
            IsLoading = false;
        }

        public async void NotifyStatusUpdate(Account account,Status status) {
            await slim.WaitAsync();
            try {
                IEnumerable<StatusViewModel> changeStatus = await Task.Run(() => {
                    return _timeline
                    .Where(x => x.Account.Id == account.Id)
                    .Where(x => x.StatusModel.DeploymentStatus().Any(y => y.Id == status.Id))
                    .ToList();
                });
                foreach(var s in changeStatus) {
                    foreach(var m in s.StatusModel.DeploymentStatus().Where(x => x.Id == status.Id)) {
                        m.SetStatus(status);
                    }
                    // ↓Adapter経由で更新を強制する
                    //int index = Timeline.IndexOf(s);
                    //if(index == -1) {
                    //    continue;
                    //}
                    //Timeline.SetOnScheduler(index,s);
                }
            } finally {
                slim.Release();
            }
        }


    }
}
