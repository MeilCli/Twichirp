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
using System.Threading;
using System.Threading.Tasks;
using CoreTweet;
using Twichirp.Core.App.Event;
using Twichirp.Core.App.Service;
using Twichirp.Core.Extensions;
using Twichirp.Core.Model;

namespace Twichirp.Core.App.Model {

    public class UserProfileModel : BaseModel {

        public event EventHandler<EventArgs<UserModel>> UserModelLoaded;
        public event EventHandler<EventArgs<string>> ErrorMessageCreated;

        private SemaphoreSlim slim = new SemaphoreSlim(1,1);
        private Account account;
        private ITwitterEventService twitterEventService;
        private long userId;
        public bool IsOwnerUser => account.Id == userId;

        private bool _isLoading;
        public bool IsLoading {
            get {
                return _isLoading;
            }
            private set {
                SetValue(ref _isLoading,value,nameof(IsLoading));
            }
        }

        private UserModel _userModel;
        public UserModel UserModel {
            get {
                return _userModel;
            }
            private set {
                if(_userModel != null) {
                    return;
                }
                _userModel = value;
                ProfileBannerUrl = _userModel.ProfileBannerUrl;
                ProfileLinkColor = _userModel.ProfileLinkColor;
                _userModel.UserChanged += (s,e) => {
                    ProfileBannerUrl = _userModel.ProfileBannerUrl;
                    ProfileLinkColor = _userModel.ProfileLinkColor;
                };
                UserModelLoaded?.Invoke(this,new EventArgs<UserModel>(value));
            }
        }

        private string _profileBannerUrl;
        public string ProfileBannerUrl {
            get {
                return _profileBannerUrl;
            }
            private set {
                SetValue(ref _profileBannerUrl,value,nameof(ProfileBannerUrl));
            }
        }

        private string _profileLinkColor;
        public string ProfileLinkColor {
            get {
                return _profileLinkColor;
            }
            private set {
                SetValue(ref _profileLinkColor,value,nameof(ProfileLinkColor));
            }
        }

        private bool _isFollowing;
        public bool IsFollowing {
            get {
                return _isFollowing;
            }
            private set {
                SetValue(ref _isFollowing,value,nameof(IsFollowing));
            }
        }

        private bool _isFollowingRequested;
        public bool IsFollowingRequested {
            get {
                return _isFollowingRequested;
            }
            private set {
                SetValue(ref _isFollowingRequested,value,nameof(IsFollowingRequested));
            }
        }

        private bool _isFollowedBy;
        public bool IsFollowedBy {
            get {
                return _isFollowedBy;
            }
            private set {
                SetValue(ref _isFollowedBy,value,nameof(IsFollowedBy));
            }
        }

        private bool _isFollowingReceived;
        public bool IsFollowingReceived {
            get {
                return _isFollowingReceived;
            }
            private set {
                SetValue(ref _isFollowingReceived,value,nameof(IsFollowingReceived));
            }
        }

        private bool _isMuting;
        public bool IsMuting {
            get {
                return _isMuting;
            }
            private set {
                SetValue(ref _isMuting,value,nameof(IsMuting));
            }
        }

        private bool _isBlocking;
        public bool IsBlocking {
            get {
                return _isBlocking;
            }
            private set {
                SetValue(ref _isBlocking,value,nameof(IsBlocking));
            }
        }

        private bool _isBlockedBy;
        public bool IsBlockedBy {
            get {
                return _isBlockedBy;
            }
            private set {
                SetValue(ref _isBlockedBy,value,nameof(IsBlockedBy));
            }
        }

        private bool _isMarkedSpam;
        public bool IsMarkedSpam {
            get {
                return _isMarkedSpam;
            }
            private set {
                SetValue(ref _isMarkedSpam,value,nameof(IsMarkedSpam));
            }
        }

        // https://dev.twitter.com/rest/reference/post/direct_messages/new
        // you are unable to determine if you can Direct Message a user via the public API
        // こんなこと書かれてるけど確認した限り判別できてるような…
        private bool _canDM;
        public bool CanDM {
            get {
                return _canDM;
            }
            private set {
                SetValue(ref _canDM,value,nameof(CanDM));
            }
        }

        public UserProfileModel(ITwichirpApplication application,ITwitterEventService twitterEventService,Account account,long userId,User user = null) : base(application) {
            this.twitterEventService = twitterEventService;
            this.account = account;
            this.userId = userId;
            init(user);
        }

        private async void init(User user) {
            if(user != null) {
                UserModel = new UserModel(Application,user);
            } else {
                await LoadUserAsync();
            }
            if(IsOwnerUser == false) {
                await LoadFriendshipAsync();
            }
        }

        public async Task LoadUserAsync() {
            await slim.WaitAsync();
            IsLoading = true;
            try {
                var user = await account.Token.Users.ShowAsync(user_id: userId,include_entities: true);
                user.CheckValid();
                UserModel = new UserModel(Application,user);
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
            IsLoading = false;
        }

        public async Task LoadFriendshipAsync() {
            if(account.Id == userId) {
                return;
            }
            await slim.WaitAsync();
            IsLoading = true;
            try {
                var friendship = await account.Token.Friendships.ShowAsync(source_id: account.Id,target_id: userId);
                IsFollowing = friendship.Source.IsFollowing;
                IsFollowingRequested = friendship.Source.IsFollowingRequested ?? false;
                IsFollowedBy = friendship.Source.IsFollowedBy;
                IsFollowingReceived = friendship.Source.IsFollowingReceived ?? false;
                IsMuting = friendship.Source.IsMuting ?? false;
                IsBlocking = friendship.Source.IsBlocking ?? false;
                IsBlockedBy = friendship.Source.IsBlockedBy ?? false;
                IsMarkedSpam = friendship.Source.IsMarkedSpam ?? false;
                CanDM = friendship.Source.CanDM;
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
            IsLoading = false;
        }

        public async Task FollowAsync(Account account) {
            if(account.Id == userId) {
                return;
            }
            try {
                var user = await account.Token.Friendships.CreateAsync(user_id: userId,follow: true);
                user.CheckValid();
                twitterEventService.CreateFollowingUser(account,user);
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            }
        }

        public async Task UnFollowAsync(Account account) {
            if(account.Id == userId) {
                return;
            }
            try {
                var user = await account.Token.Friendships.DestroyAsync(user_id: userId);
                user.CheckValid();
                twitterEventService.DestroyFollowingUser(account,user);
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            }
        }

        public async Task BlockAsync(Account account) {
            if(account.Id == userId) {
                return;
            }
            try {
                var user = await account.Token.Blocks.CreateAsync(user_id: userId,include_entities: true);
                user.CheckValid();
                twitterEventService.CreateBlockingUser(account,user);
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            }
        }

        public async Task UnBlockAsync(Account account) {
            if(account.Id == userId) {
                return;
            }
            try {
                var user = await account.Token.Blocks.DestroyAsync(user_id: userId,include_entities: true);
                user.CheckValid();
                twitterEventService.DestroyBlockingUser(account,user);
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            }
        }

        public async Task ReportSpamAsync(Account account) {
            if(account.Id == userId) {
                return;
            }
            try {
                var user = await account.Token.Users.ReportSpamAsync(user_id: userId);
                user.CheckValid();
                twitterEventService.MarkSpamUser(account,user);
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            }
        }

        public void NotifyUserUpdated(Account account,User user) {
            UserModel?.SetUser(user);
        }

        public void NotifyFollowingUserCreated(Account account,User user) {
            UserModel?.SetUser(user);
            if(this.account.Id != account.Id) {
                return;
            }
            if(user.IsProtected) {
                IsFollowingRequested = true;
            } else {
                IsFollowing = true;
            }
        }

        public void NotifyFollowingUserDestroyed(Account account,User user) {
            UserModel?.SetUser(user);
            if(this.account.Id != account.Id) {
                return;
            }
            IsFollowing = false;
        }

        public void NotifyBlokingUserCreated(Account account,User user) {
            UserModel?.SetUser(user);
            if(this.account.Id != account.Id) {
                return;
            }
            IsBlocking = true;
        }

        public void NotifyBlockingUserDestroyed(Account account,User user) {
            UserModel?.SetUser(user);
            if(this.account.Id != account.Id) {
                return;
            }
            IsBlocking = false;
            IsMarkedSpam = false;
        }

        public void NotifySpamUserMarked(Account account,User user) {
            UserModel?.SetUser(user);
            if(this.account.Id != account.Id) {
                return;
            }
            IsMarkedSpam = true;
            IsBlocking = true;
        }
    }
}
