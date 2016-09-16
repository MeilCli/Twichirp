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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Core.Model;

namespace Twichirp.Core.App {
    public class TwitterEvent : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyReactiveProperty<Tuple<Account,Status>> UpdateStatusEvent { get; }
        private Tuple<Account,Status> _updateStatus;
        public Tuple<Account,Status> UpdateStatus {
            get {
                return _updateStatus;
            }
            set {
                _updateStatus = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(UpdateStatus)));
            }
        }

        public TwitterEvent() {
            UpdateStatusEvent = this.ObserveProperty(x => x.UpdateStatus).ToReadOnlyReactiveProperty(mode: ReactivePropertyMode.None);
        }

    }
}
