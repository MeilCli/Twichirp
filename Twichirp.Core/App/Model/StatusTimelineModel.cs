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

namespace Twichirp.Core.App.Model {
    class StatusTimelineModel : BaseModel {

        private SemaphoreSlim slim = new SemaphoreSlim(1,1);
        public ReactiveCollection<BaseViewModel> Timeline { get; } = new ReactiveCollection<BaseViewModel>();
        private List<StatusViewModel> _timeline { get; } = new List<StatusViewModel>();
        private Timeline<IEnumerable<Status>> timelineResource;

        private bool _isLoading;
        public bool IsLoading {
            get {
                return _isLoading;
            }
            private set {
                _isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        private string _errorMessage;
        public string ErrorMessage {
            get {
                return _errorMessage;
            }
            private set {
                _errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }

        public StatusTimelineModel(ITwichirpApplication application,Timeline<IEnumerable<Status>> timelineResource) : base(application) {
            this.timelineResource = timelineResource;
        }

        public async void Load(Timeline<IEnumerable<Status>> timelineResource = null) {
            if(IsLoading) {
                return;
            }
            IsLoading = true;
            ErrorMessage = null;
            await slim.WaitAsync();
            try {
                int count = 20;
                if(_timeline.Count >= 1) {
                    IEnumerable<Status> response = await timelineResource.Load(count,_timeline[0].Id.Value);
                    if(response.Count() == count) {

                    }
                    foreach(var s in response.Select(x=>new StatusViewModel(Application,x)).Reverse()) {
                        _timeline.Insert(0,s);
                        Timeline.InsertOnScheduler(0,s);
                    }
                }else {
                    foreach(var s in (await timelineResource.Load(count)).Select(x => new StatusViewModel(Application,x)).Reverse()) {
                        _timeline.Insert(0,s);
                        Timeline.InsertOnScheduler(0,s);
                    }
                }

                
            } catch(Exception e) {
                ErrorMessage = e.Message;
            } finally {
                slim.Release();
            }
            IsLoading = false;
        }
    }
}
