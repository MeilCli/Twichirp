using System;
using CoreTweet;
using Twichirp.Core.DataObjects;
using Twichirp.Core.Model;
using CStatus = CoreTweet.Status;
using CUser = CoreTweet.User;

namespace Twichirp.Core.App.Event {

    public class TwitterEventArgs : EventArgs {

        public ImmutableAccount Account { get; }

        public TwitterEventArgs(ImmutableAccount account) {
            Account = account;
        }
    }

    public class StatusEventArgs : TwitterEventArgs {

        public CStatus Status { get; }

        public StatusEventArgs(ImmutableAccount account,CStatus status) : base(account) {
            Status = status;
        }
    }

    public class UserEventArgs : TwitterEventArgs {

        public CUser User { get; }

        public UserEventArgs(ImmutableAccount account,CUser user) : base(account) {
            User = user;
        }

    }
}
