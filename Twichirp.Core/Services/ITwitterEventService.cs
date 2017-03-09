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
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Core.App.Event;
using Twichirp.Core.DataObjects;
using CStatus = CoreTweet.Status;
using CUser = CoreTweet.User;

namespace Twichirp.Core.Services {

    public interface ITwitterEventService {

        event EventHandler<StatusEventArgs> StatusUpdated;
        event EventHandler<UserEventArgs> UserUpdated;
        event EventHandler<UserEventArgs> FollowingUserCreated;
        event EventHandler<UserEventArgs> FollowingUserDestroyed;
        event EventHandler<UserEventArgs> BlockingUserCreated;
        event EventHandler<UserEventArgs> BlockingUserDestroyed;
        event EventHandler<UserEventArgs> SpamUserMarked;

        void UpdateStatus(ImmutableAccount account,CStatus status);

        void UpdateUser(ImmutableAccount account,CUser user);

        void CreateFollowingUser(ImmutableAccount account,CUser user);

        void DestroyFollowingUser(ImmutableAccount account,CUser user);

        void CreateBlockingUser(ImmutableAccount account,CUser user);

        void DestroyBlockingUser(ImmutableAccount account,CUser user);

        void MarkSpamUser(ImmutableAccount account,CUser user);

    }
}
