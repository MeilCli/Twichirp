using System;
using CoreTweet;
using Twichirp.Core.Model;

namespace Twichirp.Core.App.Event {

    public class TwitterEventArgs : EventArgs {

        public Account Account { get; }

        public TwitterEventArgs(Account account) {
            Account = account;
        }
    }

    public class StatusEventArgs : TwitterEventArgs {

        public Status Status { get; }

        public StatusEventArgs(Account account, Status status) : base(account) {
            Status = status;
        }
    }

    public class UserEventArgs : TwitterEventArgs {

        public User User { get; }

        public UserEventArgs(Account account, User user) : base(account) {
            User = user;
        }

    }
}
