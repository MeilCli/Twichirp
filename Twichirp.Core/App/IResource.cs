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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.App {

    public interface IValue<T> {
        T Value { get; }
    }

    public interface IResource {

        IValue<string> SplashAccountLoading { get; }
        IValue<string> SplashConsumerLoading { get; }
        IValue<string> SplashAccountDownLoading { get; }

        IValue<string> StatusRetweetingUser { get; }
        IValue<string> StatusReplyToUser { get; }

        IValue<string> TimeSecoundAgo { get; }
        IValue<string> TimeSecoundsAgo { get; }
        IValue<string> TimeMinuteAgo { get; }
        IValue<string> TimeMinutesAgo { get; }
        IValue<string> TimeHourAgo { get; }
        IValue<string> TimeHoursAgo { get; }
        IValue<string> TimeDayAgo { get; }
        IValue<string> TimeDaysAgo { get; }

        IValue<string> UserFollowedBy { get; }
        IValue<string> UserFollowingReceived { get; }
        IValue<string> UserBlockedBy { get; }
        IValue<string> UserFollowing { get; }
        IValue<string> UserFollowingRequested { get; }
        IValue<string> UserBlocking { get; }
        IValue<string> UserFollow { get; }
        IValue<string> UserMuting { get; }
        IValue<string> UserMarkedSpam { get; }

    }
}
