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
using Twichirp.Core.DataObjects;
using CStatus = CoreTweet.Status;
using CUser = CoreTweet.User;

namespace Twichirp.Core.Events {

    public class TwitterEventArgs : EventArgs {

        public ImmutableAccount Account { get; }

        public TwitterEventArgs(ImmutableAccount account) {
            Account = account;
        }
    }

    public class StatusEventArgs : TwitterEventArgs {

        public CStatus Status { get; }

        public StatusEventArgs(ImmutableAccount account, CStatus status) : base(account) {
            Status = status;
        }
    }

    public class UserEventArgs : TwitterEventArgs {

        public CUser User { get; }

        public UserEventArgs(ImmutableAccount account, CUser user) : base(account) {
            User = user;
        }

    }
}
