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
using Newtonsoft.Json;
using Realms;
using CUser = CoreTweet.User;

namespace Twichirp.Core.DataObjects {

    /// <summary>
    /// If delete user object from Realm, containing Account user must not delete.
    /// Should check contain Account when delete.
    /// </summary>
    public class User : RealmObject, Interfaces.IUser {

        [PrimaryKey]
        [JsonRequired]
        public long Id { get; set; }

        [Indexed]
        [JsonRequired]
        public string ScreenName { get; set; }

        [Indexed]
        [JsonRequired]
        public string Name { get; set; }

        [Required]
        [JsonRequired]
        public string Json { get; set; }

        [JsonRequired]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonIgnore]
        private CUser _coreTweetUser;
        [Ignored]
        [JsonIgnore]
        public CUser CoreTweetUser {
            get {
                if(_coreTweetUser == null) {
                    _coreTweetUser = JsonConvert.DeserializeObject<CUser>(Json);
                    _coreTweetUser.Status = null; //軽量化
                }
                return _coreTweetUser;
            }
            set {
                _coreTweetUser = value;
            }
        }

        [Ignored]
        [JsonIgnore]
        public bool IsValidObject {
            get {
                return ScreenName != null && Json != null;
            }
        }

        /// <summary>
        /// Backlink property.
        /// use to check this user containing other object when delete this user. 
        /// </summary>
        [Backlink(nameof(Account.User))]
        public IQueryable<Account> Accounts { get; }

        /// <summary>
        /// Backlink property.
        /// use to check this user containing other object when delete this user. 
        /// </summary>
        [Backlink(nameof(Status.OwnerUser))]
        public IQueryable<Status> OwnerStatuses { get; }

        /// <summary>
        /// Backlink property.
        /// use to check this user containing other object when delete this user. 
        /// </summary>
        [Backlink(nameof(Status.User))]
        public IQueryable<Status> Statuses { get; }

        /// <summary>
        /// Backlink property.
        /// use to check this user containing other object when delete this user. 
        /// </summary>
        [Backlink(nameof(StatusTimelineCache.OwnerUser))]
        public IQueryable<StatusTimelineCache> OwnerStatusTimelineCaches { get; }

        /// <summary>
        /// Backlink property.
        /// use to check this user containing other object when delete this user.
        /// </summary>
        [Backlink(nameof(List.User))]
        public IQueryable<List> Lists { get; }

        /// <summary>
        /// Backlink property.
        /// use to check this user containing other object when delete this user.
        /// </summary>
        [Backlink(nameof(ListCollectionCache.OwnerUser))]
        public IQueryable<ListCollectionCache> ListCollectionCaches { get; }

        /// <summary>
        /// Backlink property.
        /// use to check this user containing other object when delete this user.
        /// </summary>
        [Backlink(nameof(DirectMessage.Recipient))]
        public IQueryable<DirectMessage> ReceivedDirectMessages { get; }

        /// <summary>
        /// Backlink property.
        /// use to check this user containing other object when delete this user.
        /// </summary>
        [Backlink(nameof(DirectMessage.Sender))]
        public IQueryable<DirectMessage> SentDirectMessages { get; }

        public User() { }

        public User(CUser user) {
            Id = user.Id ?? -1;
            ScreenName = user.ScreenName;
            Name = user.Name;
            {
                //軽量化
                var temp = user.Status;
                user.Status = null;
                Json = JsonConvert.SerializeObject(user);
                user.Status = temp;
            }
            UpdatedAt = DateTimeOffset.Now;
            CoreTweetUser = user;
        }

        public User(ImmutableUser item) {
            Id = item.Id;
            ScreenName = item.ScreenName;
            Name = item.Name;
            Json = item.Json;
            UpdatedAt = item.UpdatedAt;
        }
    }

    public class ImmutableUser : Interfaces.IUser {

        public long Id { get; }

        public string Json { get; }

        public string Name { get; }

        public string ScreenName { get; }

        public DateTimeOffset UpdatedAt { get; }

        [JsonIgnore]
        private CUser _coreTweetUser;
        [JsonIgnore]
        public CUser CoreTweetUser {
            get {
                if(_coreTweetUser == null) {
                    _coreTweetUser = JsonConvert.DeserializeObject<CUser>(Json);
                }
                return _coreTweetUser;
            }
        }

        public ImmutableUser(User item) {
            Id = item.Id;
            Json = item.Json;
            Name = item.Name;
            ScreenName = item.ScreenName;
            UpdatedAt = item.UpdatedAt;
        }
    }
}
