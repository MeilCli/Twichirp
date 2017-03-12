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
using Twichirp.Core.Events;
using CStatus = CoreTweet.Status;
using CUser = CoreTweet.User;

namespace Twichirp.Core.Services {

    public class TwitterEventService : ITwitterEventService {

        public event EventHandler<StatusEventArgs> StatusUpdated;
        public event EventHandler<UserEventArgs> UserUpdated;
        public event EventHandler<UserEventArgs> FollowingUserCreated;
        public event EventHandler<UserEventArgs> FollowingUserDestroyed;
        public event EventHandler<UserEventArgs> BlockingUserCreated;
        public event EventHandler<UserEventArgs> BlockingUserDestroyed;
        public event EventHandler<UserEventArgs> SpamUserMarked;

        public TwitterEventService() {
        }

        public void UpdateStatus(ImmutableAccount account, CStatus status) {
            StatusUpdated?.Invoke(this, new StatusEventArgs(account, status));
        }

        public void UpdateUser(ImmutableAccount account, CUser user) {
            UserUpdated?.Invoke(this, new UserEventArgs(account, user));
        }

        public void CreateFollowingUser(ImmutableAccount account, CUser user) {
            FollowingUserCreated?.Invoke(this, new UserEventArgs(account, user));
        }

        public void DestroyFollowingUser(ImmutableAccount account, CUser user) {
            FollowingUserDestroyed?.Invoke(this, new UserEventArgs(account, user));
        }

        public void CreateBlockingUser(ImmutableAccount account, CUser user) {
            BlockingUserCreated?.Invoke(this, new UserEventArgs(account, user));
        }

        public void DestroyBlockingUser(ImmutableAccount account, CUser user) {
            BlockingUserDestroyed?.Invoke(this, new UserEventArgs(account, user));
        }

        public void MarkSpamUser(ImmutableAccount account, CUser user) {
            SpamUserMarked?.Invoke(this, new UserEventArgs(account, user));
        }

    }
}
