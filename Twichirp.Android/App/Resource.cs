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

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Twichirp.Core.App;

namespace Twichirp.Android.App {

    public class BaseValue<T> : IValue<T> {

        protected int Resource;
        protected Resources Resources;

        public BaseValue(Resources resources,int resource) {
            this.Resources = resources;
            this.Resource = resource;
        }

        public virtual T Value {
            get {
                throw new NotImplementedException();
            }
        }
    }

    public class StringValue : BaseValue<string> {

        public StringValue(Resources resources,int resource) : base(resources,resource) {
        }

        public override string Value => Resources.GetString(Resource);
    }

    public class Resource : IResource {

        public IValue<string> SplashAccountLoading { get; }
        public IValue<string> SplashConsumerLoading { get; }
        public IValue<string> SplashAccountDownLoading { get; }

        public IValue<string> StatusRetweetingUser { get; }
        public IValue<string> StatusReplyToUser { get; }

        public IValue<string> TimeSecoundAgo { get; }
        public IValue<string> TimeSecoundsAgo { get; }
        public IValue<string> TimeMinuteAgo { get; }
        public IValue<string> TimeMinutesAgo { get; }
        public IValue<string> TimeHourAgo { get; }
        public IValue<string> TimeHoursAgo { get; }
        public IValue<string> TimeDayAgo { get; }
        public IValue<string> TimeDaysAgo { get; }

        public IValue<string> UserFollowedBy { get; }
        public IValue<string> UserFollowingReceived { get; }
        public IValue<string> UserBlockedBy { get; }
        public IValue<string> UserFollowing { get; }
        public IValue<string> UserFollowingRequested { get; }
        public IValue<string> UserBlocking { get; }
        public IValue<string> UserFollow { get; }
        public IValue<string> UserMuting { get; }
        public IValue<string> UserMarkedSpam { get; }

        public Resource(Resources resources) {
            SplashAccountLoading = new StringValue(resources,Android.Resource.String.SplashAccountLoading);
            SplashConsumerLoading = new StringValue(resources,Android.Resource.String.SplashConsumerLoading);
            SplashAccountDownLoading = new StringValue(resources,Android.Resource.String.SplashAccountDownLoading);

            StatusRetweetingUser = new StringValue(resources,Android.Resource.String.StatusRetweetingUser);
            StatusReplyToUser = new StringValue(resources,Android.Resource.String.StatusReplyToUser);

            TimeSecoundAgo = new StringValue(resources,Android.Resource.String.TimeSecondAgo);
            TimeSecoundsAgo = new StringValue(resources,Android.Resource.String.TimeSecondsAgo);
            TimeMinuteAgo = new StringValue(resources,Android.Resource.String.TimeMinuteAgo);
            TimeMinutesAgo = new StringValue(resources,Android.Resource.String.TimeMinutesAgo);
            TimeHourAgo = new StringValue(resources,Android.Resource.String.TimeHourAgo);
            TimeHoursAgo = new StringValue(resources,Android.Resource.String.TimeHoursAgo);
            TimeDayAgo = new StringValue(resources,Android.Resource.String.TimeDayAgo);
            TimeDaysAgo = new StringValue(resources,Android.Resource.String.TimeDaysAgo);

            UserFollowedBy = new StringValue(resources,Android.Resource.String.UserFollowedBy);
            UserFollowingReceived = new StringValue(resources,Android.Resource.String.UserFollowingReceived);
            UserBlockedBy = new StringValue(resources,Android.Resource.String.UserBlockedBy);
            UserFollowing = new StringValue(resources,Android.Resource.String.UserFollowing);
            UserFollowingRequested = new StringValue(resources,Android.Resource.String.UserFollowingRequested);
            UserBlocking = new StringValue(resources,Android.Resource.String.UserBlocking);
            UserFollow = new StringValue(resources,Android.Resource.String.UserFollow);
            UserMuting = new StringValue(resources,Android.Resource.String.UserMuting);
            UserMarkedSpam = new StringValue(resources,Android.Resource.String.UserMarkedSpam);
        }

    }
}