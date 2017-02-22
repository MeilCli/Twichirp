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
using Twichirp.Core.App.Event;

namespace Twichirp.Core.App.Service {

    public interface ITwitterEventService {

        event EventHandler<StatusEventArgs> StatusUpdated;
        event EventHandler<UserEventArgs> UserUpdated;
        event EventHandler<UserEventArgs> FollowingUserCreated;
        event EventHandler<UserEventArgs> FollowingUserDestroyed;
        event EventHandler<UserEventArgs> BlockingUserCreated;
        event EventHandler<UserEventArgs> BlockingUserDestroyed;
        event EventHandler<UserEventArgs> SpamUserMarked;

        void UpdateStatus(Account account, Status status);

        void UpdateUser(Account account, User user);

        void CreateFollowingUser(Account account, User user);

        void DestroyFollowingUser(Account account, User user);

        void CreateBlockingUser(Account account, User user);

        void DestroyBlockingUser(Account account, User user);

        void MarkSpamUser(Account account, User user);

    }
}
