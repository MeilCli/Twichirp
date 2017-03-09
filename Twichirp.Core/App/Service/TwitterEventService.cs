using System;
using CoreTweet;
using Twichirp.Core.App.Event;
using CStatus = CoreTweet.Status;
using CUser = CoreTweet.User;
using Twichirp.Core.DataObjects;

namespace Twichirp.Core.App.Service {

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

        public void UpdateStatus(ImmutableAccount account,CStatus status) {
            StatusUpdated?.Invoke(this,new StatusEventArgs(account,status));
        }

        public void UpdateUser(ImmutableAccount account,CUser user) {
            UserUpdated?.Invoke(this,new UserEventArgs(account,user));
        }

        public void CreateFollowingUser(ImmutableAccount account,CUser user) {
            FollowingUserCreated?.Invoke(this,new UserEventArgs(account,user));
        }

        public void DestroyFollowingUser(ImmutableAccount account,CUser user) {
            FollowingUserDestroyed?.Invoke(this,new UserEventArgs(account,user));
        }

        public void CreateBlockingUser(ImmutableAccount account,CUser user) {
            BlockingUserCreated?.Invoke(this,new UserEventArgs(account,user));
        }

        public void DestroyBlockingUser(ImmutableAccount account,CUser user) {
            BlockingUserDestroyed?.Invoke(this,new UserEventArgs(account,user));
        }

        public void MarkSpamUser(ImmutableAccount account,CUser user) {
            SpamUserMarked?.Invoke(this,new UserEventArgs(account,user));
        }

    }
}
