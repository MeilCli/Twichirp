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
using CoreTweet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.Models {

    public class UserModel : BaseModel {

        public event EventHandler<EventArgs> UserChanged;

        private User user;

        public long Id { get; private set; }

        public string ScreenName { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<TextPart> Description { get; private set; }

        public string Location { get; private set; }

        public IEnumerable<TextPart> Url { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public int FavouritesCount { get; private set; }

        public int FollowersCount { get; private set; }

        public int FriendsCount { get; private set; }

        public int ListedCount { get; private set; }

        public string ProfileBannerUrl { get; private set; }

        public string ProfileImageUrl { get; private set; }

        public string ProfileLinkColor { get; private set; }

        public bool IsProtected { get; private set; }

        public int StatusesCount { get; private set; }

        public bool IsVerified { get; private set; }

        public UserModel(User user) {
            SetUser(user);
        }

        public void SetUser(User user) {
            this.user = user;

            SetProperty(this,x => x.Id,user.Id.Value);
            SetProperty(this,x => x.ScreenName,user.ScreenName);
            SetProperty(this,x => x.Name,user.Name ?? string.Empty);

            {
                var _description = user.Description == null ? Enumerable.Empty<TextPart>() : CoreTweetSupplement.EnumerateTextParts(user.Description,user.Entities?.Description);
                SetProperty(this,x => x.Description,_description);
            }

            SetProperty(this,x => x.Location,user.Location ?? string.Empty);

            {
                var _url = user.Url == null ? Enumerable.Empty<TextPart>() : CoreTweetSupplement.EnumerateTextParts(user.Url,user.Entities?.Url);
                SetProperty(this,x => x.Url,_url);
            }

            SetProperty(this,x => x.CreatedAt,user.CreatedAt);
            SetProperty(this,x => x.FavouritesCount,user.FavouritesCount);
            SetProperty(this,x => x.FollowersCount,user.FollowersCount);
            SetProperty(this,x => x.FriendsCount,user.FriendsCount);
            SetProperty(this,x => x.ListedCount,user.ListedCount ?? 0);
            SetProperty(this,x => x.ProfileBannerUrl,user.ProfileBannerUrl);
            SetProperty(this,x => x.ProfileImageUrl,user.ProfileImageUrl);
            SetProperty(this,x => x.ProfileLinkColor,user.ProfileLinkColor);
            SetProperty(this,x => x.IsProtected,user.IsProtected);
            SetProperty(this,x => x.StatusesCount,user.StatusesCount);
            SetProperty(this,x => x.IsVerified,user.IsVerified);

            UserChanged?.Invoke(this,new EventArgs());
        }

        public string ExportJson() => JsonConvert.SerializeObject(user);
    }
}
