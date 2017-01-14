// Copyright (c) 2016 meil
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
using CoreTweet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.App.Model {
    class UserModel : BaseModel {

        private User user;

        public long Id { get; private set; }

        public string ScreenName { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<TextPart> Description { get; private set; }

        public string Location { get; private set; }

        public IEnumerable<TextPart> Url { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public int FavouritesCount { get; private set; }

        public bool IsFollowRequestSent { get; private set; }

        public int FollowersCount { get; private set; }

        public int FriendsCount { get; private set; }

        public int ListedCount { get; private set; }

        public bool IsMuting { get; private set; }

        public string ProfileBannerUrl { get; private set; }

        public string ProfileImageUrl { get; private set; }

        public string ProfileLinkColor { get; private set; }

        public bool IsProtected { get; private set; }

        public int StatusesCount { get; private set; }

        // 凍結?
        public bool IsSuspended { get; private set; }

        public bool IsVerified { get; private set; }

        public UserModel(ITwichirpApplication application,User user) : base(application) {
            SetUser(user);
        }

        public void SetUser(User user) {
            this.user = user;

            Id = user.Id.Value;
            RaisePropertyChanged(nameof(Id));

            ScreenName = user.ScreenName;
            RaisePropertyChanged(nameof(ScreenName));

            Name = user.Name ?? string.Empty;
            RaisePropertyChanged(nameof(Name));

            Description = user.Description == null ? Enumerable.Empty<TextPart>() : CoreTweetSupplement.EnumerateTextParts(user.Description,user.Entities?.Description);
            RaisePropertyChanged(nameof(Description));

            Location = user.Location ?? string.Empty;
            RaisePropertyChanged(nameof(Location));

            Url = user.Url == null ? Enumerable.Empty<TextPart>() : CoreTweetSupplement.EnumerateTextParts(user.Url,user.Entities?.Url);
            RaisePropertyChanged(nameof(Url));

            CreatedAt = user.CreatedAt;
            RaisePropertyChanged(nameof(CreatedAt));

            FavouritesCount = user.FavouritesCount;
            RaisePropertyChanged(nameof(FavouritesCount));

            IsFollowRequestSent = user.IsFollowRequestSent ?? false;
            RaisePropertyChanged(nameof(IsFollowRequestSent));

            FollowersCount = user.FollowersCount;
            RaisePropertyChanged(nameof(FollowersCount));

            FriendsCount = user.FriendsCount;
            RaisePropertyChanged(nameof(FriendsCount));

            ListedCount = user.ListedCount ?? 0;
            RaisePropertyChanged(nameof(ListedCount));

            IsMuting = user.IsMuting ?? false;
            RaisePropertyChanged(nameof(IsMuting));

            ProfileBannerUrl = user.ProfileBannerUrl;
            RaisePropertyChanged(nameof(ProfileBannerUrl));

            ProfileImageUrl = user.ProfileImageUrl;
            RaisePropertyChanged(nameof(ProfileImageUrl));

            ProfileLinkColor = user.ProfileLinkColor;
            RaisePropertyChanged(nameof(ProfileLinkColor));

            IsProtected = user.IsProtected;
            RaisePropertyChanged(nameof(IsProtected));

            StatusesCount = user.StatusesCount;
            RaisePropertyChanged(nameof(StatusesCount));

            IsSuspended = user.IsSuspended ?? false;
            RaisePropertyChanged(nameof(IsSuspended));

            IsVerified = user.IsVerified;
            RaisePropertyChanged(nameof(IsVerified));
        }

        public void ExportJson() => JsonConvert.SerializeObject(user);
    }
}
