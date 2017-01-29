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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Core.Model;

namespace Twichirp.Core.App {

    public class TwitterEventArgs : EventArgs {

        public Account Account { get; }

        public TwitterEventArgs(Account account) {
            Account = account;
        }
    }

    public class StatusEventArgs : TwitterEventArgs {

        public Status Status { get; }

        public StatusEventArgs(Account account,Status status) : base(account) {
            Status = status;
        }
    }

    public class UserEventArgs : TwitterEventArgs {

        public User User { get; }

        public UserEventArgs(Account account,User user) : base(account) {
            User = user;
        }

    }

    public class TwitterEvent {

        public event EventHandler<StatusEventArgs> StatusUpdated;
        public event EventHandler<UserEventArgs> UserUpdated;

        public TwitterEvent() {
        }

        public void UpdateStatus(Account account,Status status) {
            StatusUpdated?.Invoke(this,new StatusEventArgs(account,status));
        }

        public void UpdateUser(Account account,User user) {
            UserUpdated?.Invoke(this,new UserEventArgs(account,user));
        }

    }
}
