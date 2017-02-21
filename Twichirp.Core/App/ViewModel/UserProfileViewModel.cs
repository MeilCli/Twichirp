﻿// Copyright (c) 2016-2017 meil
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
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;
using Plugin.CrossFormattedText.Abstractions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Core.App.Event;
using Twichirp.Core.App.Model;
using Twichirp.Core.Model;

namespace Twichirp.Core.App.ViewModel {

    public class UserProfileViewModel : BaseViewModel {

        public const string TransitionIconName = "transition_icon";

        private UserProfileModel userProfileModel;
        private const int friendshipUnFollowing = 0;
        private const int friendshipFollowing = 1;
        private const int friendshipRequestingFollow = 2;
        private const int frindshipBlocking = 3;

        public Account Account { get; }
        public long UserId { get; }
        public bool IsOwnerAccount => userProfileModel.IsOwnerUser;
        private int friendshipType = friendshipUnFollowing;

        public ReadOnlyReactiveProperty<bool> IsLoading { get; }

        public ReadOnlyReactiveProperty<string> Banner { get; }
        public ReadOnlyReactiveProperty<string> LinkColor { get; }
        public ReactiveProperty<string> IconUrl { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ScreenName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> IsProtected { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsVerified { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<ISpannableString> Description { get; } = new ReactiveProperty<ISpannableString>();
        public ReactiveProperty<string> Location { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<ISpannableString> Url { get; } = new ReactiveProperty<ISpannableString>();
        public ReadOnlyReactiveProperty<string> Relationship { get; }
        public ReadOnlyReactiveProperty<string> Friendship { get; }
        public ReadOnlyReactiveProperty<string> Extraship { get; }

        public AsyncReactiveCommand FriendshipCommand { get; }

        public UserProfileViewModel(ITwichirpApplication application,Account account,long userId,User user = null) : base(application) {
            Account = account;
            UserId = userId;
            userProfileModel = new UserProfileModel(application,account,userId,user);

            IsLoading = userProfileModel.ObserveProperty(x => x.IsLoading).ToReadOnlyReactiveProperty().AddTo(Disposable);

            Observable.FromEventPattern<EventArgs<UserModel>>(x => userProfileModel.UserModelLoaded += x,x => userProfileModel.UserModelLoaded -= x)
                .Select(x => x.EventArgs.EventData)
                .ToReadOnlyReactiveProperty(userProfileModel.UserModel)
                .Where(x => x != null)
                .Select(x => new UserViewModel(Application,x,Account))
                .Subscribe(x => initValue(x))
                .AddTo(Disposable);
            Banner = userProfileModel.ObserveProperty(x => x.ProfileBannerUrl).ToReadOnlyReactiveProperty().AddTo(Disposable);
            LinkColor = userProfileModel.ObserveProperty(x => x.ProfileLinkColor).ToReadOnlyReactiveProperty().AddTo(Disposable);

            Relationship = userProfileModel.ObserveProperty(x => x.IsFollowedBy)
                .CombineLatest(userProfileModel.ObserveProperty(x => x.IsFollowingReceived),userProfileModel.ObserveProperty(x => x.IsBlockedBy),(x,y,z) => relationship(x,y,z))
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            Friendship = userProfileModel.ObserveProperty(x => x.IsFollowing)
                .CombineLatest(userProfileModel.ObserveProperty(x => x.IsFollowingRequested),userProfileModel.ObserveProperty(x => x.IsBlocking),(x,y,z) => friendship(x,y,z))
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            FriendshipCommand = IsLoading.Select(x => x == false).ToAsyncReactiveCommand().AddTo(Disposable);
            FriendshipCommand.Subscribe(x => friendshipCommand()).AddTo(Disposable);
            Extraship = userProfileModel.ObserveProperty(x => x.IsMuting)
                .CombineLatest(userProfileModel.ObserveProperty(x => x.IsMarkedSpam),(x,y) => extraship(x,y))
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);

            Observable.FromEventPattern<UserEventArgs>(x => application.TwitterEvent.UserUpdated += x,x => application.TwitterEvent.UserUpdated -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => userProfileModel.NotifyUserUpdated(x.EventArgs.Account,x.EventArgs.User))
                .AddTo(Disposable);
            Observable.FromEventPattern<UserEventArgs>(x => application.TwitterEvent.FollowingUserCreated += x,x => application.TwitterEvent.FollowingUserCreated -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => userProfileModel.NotifyFollowingUserCreated(x.EventArgs.Account,x.EventArgs.User))
                .AddTo(Disposable);
            Observable.FromEventPattern<UserEventArgs>(x => application.TwitterEvent.FollowingUserDestroyed += x,x => application.TwitterEvent.FollowingUserDestroyed -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => userProfileModel.NotifyFollowingUserDestroyed(x.EventArgs.Account,x.EventArgs.User))
                .AddTo(Disposable);
            Observable.FromEventPattern<UserEventArgs>(x => application.TwitterEvent.BlockingUserCreated += x,x => application.TwitterEvent.BlockingUserCreated -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => userProfileModel.NotifyBlokingUserCreated(x.EventArgs.Account,x.EventArgs.User))
                .AddTo(Disposable);
            Observable.FromEventPattern<UserEventArgs>(x => application.TwitterEvent.BlockingUserDestroyed += x,x => application.TwitterEvent.BlockingUserDestroyed -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => userProfileModel.NotifyBlockingUserDestroyed(x.EventArgs.Account,x.EventArgs.User))
                .AddTo(Disposable);
            Observable.FromEventPattern<UserEventArgs>(x => application.TwitterEvent.SpamUserMarked += x,x => application.TwitterEvent.SpamUserMarked -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => userProfileModel.NotifySpamUserMarked(x.EventArgs.Account,x.EventArgs.User))
                .AddTo(Disposable);
        }

        private void initValue(UserViewModel userViewModel) {
            userViewModel.IconUrl.Subscribe(x => IconUrl.Value = x).AddTo(Disposable);
            userViewModel.Name.Subscribe(x => Name.Value = x).AddTo(Disposable);
            userViewModel.ScreenName.Subscribe(x => ScreenName.Value = x).AddTo(Disposable);
            userViewModel.IsProtected.Subscribe(x => IsProtected.Value = x).AddTo(Disposable);
            userViewModel.IsVerified.Subscribe(x => IsVerified.Value = x).AddTo(Disposable);
            userViewModel.Description.Subscribe(x => Description.Value = x).AddTo(Disposable);
            userViewModel.Location.Subscribe(x => Location.Value = x).AddTo(Disposable);
            userViewModel.Url.Subscribe(x => Url.Value = x).AddTo(Disposable);

            Disposable.Add(userViewModel);
        }

        private string relationship(bool isFollowedBy,bool isFollowingReceived,bool isBlockedBy) {
            if(isFollowedBy) {
                return Application.Resource.UserFollowedBy.Value;
            }
            if(isBlockedBy) {
                return Application.Resource.UserBlockedBy.Value;
            }
            if(isFollowingReceived) {
                return Application.Resource.UserFollowingReceived.Value;
            }
            return string.Empty;
        }

        private string friendship(bool isFollowing,bool isFollowingRequested,bool isBlocking) {
            if(isFollowing) {
                friendshipType = friendshipFollowing;
                return Application.Resource.UserFollowing.Value;
            }
            if(isBlocking) {
                friendshipType = frindshipBlocking;
                return Application.Resource.UserBlocking.Value;
            }
            if(isFollowingRequested) {
                friendshipType = friendshipRequestingFollow;
                return Application.Resource.UserFollowingRequested.Value;
            }
            friendshipType = friendshipUnFollowing;
            return Application.Resource.UserFollow.Value;
        }

        private async Task friendshipCommand() {
            switch(friendshipType) {
                case friendshipFollowing:
                    await userProfileModel.UnFollowAsync(Account);
                    break;
                case friendshipUnFollowing:
                    await userProfileModel.FollowAsync(Account);
                    break;
                case friendshipRequestingFollow:
                    await userProfileModel.UnFollowAsync(Account);
                    break;
                case frindshipBlocking:
                    await userProfileModel.UnBlockAsync(Account);
                    break;
            }
        }

        private string extraship(bool isMuting,bool isMarkedSpam) {
            if(isMarkedSpam) {
                return Application.Resource.UserMarkedSpam.Value;
            }
            if(isMuting) {
                return Application.Resource.UserMuting.Value;
            }
            return string.Empty;
        }

    }
}
