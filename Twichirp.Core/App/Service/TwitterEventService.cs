using System;
using Twichirp.Core.Model;
using CoreTweet;
using Twichirp.Core.App.Event;

namespace Twichirp.Core.App.Service {

    public class TwitterEventService : ITwitterEventService{

        public event EventHandler<StatusEventArgs> StatusUpdated;
        public event EventHandler<UserEventArgs> UserUpdated;
        public event EventHandler<UserEventArgs> FollowingUserCreated;
        public event EventHandler<UserEventArgs> FollowingUserDestroyed;
        public event EventHandler<UserEventArgs> BlockingUserCreated;
        public event EventHandler<UserEventArgs> BlockingUserDestroyed;
        public event EventHandler<UserEventArgs> SpamUserMarked;

        public TwitterEventService() {
        }

        public void UpdateStatus(Account account, Status status) {
            StatusUpdated?.Invoke(this, new StatusEventArgs(account, status));
        }

        public void UpdateUser(Account account, User user) {
            UserUpdated?.Invoke(this, new UserEventArgs(account, user));
        }

        public void CreateFollowingUser(Account account, User user) {
            FollowingUserCreated?.Invoke(this, new UserEventArgs(account, user));
        }

        public void DestroyFollowingUser(Account account, User user) {
            FollowingUserDestroyed?.Invoke(this, new UserEventArgs(account, user));
        }

        public void CreateBlockingUser(Account account, User user) {
            BlockingUserCreated?.Invoke(this, new UserEventArgs(account, user));
        }

        public void DestroyBlockingUser(Account account, User user) {
            BlockingUserDestroyed?.Invoke(this, new UserEventArgs(account, user));
        }

        public void MarkSpamUser(Account account, User user) {
            SpamUserMarked?.Invoke(this, new UserEventArgs(account, user));
        }

    }
}
