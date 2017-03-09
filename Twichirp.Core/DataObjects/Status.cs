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
using CStatus = CoreTweet.Status;

namespace Twichirp.Core.DataObjects {

    public class Status : RealmObject, Interfaces.IStatus<Status,User> {

        public static string MakePrimaryKey(long ownerUserId,long id) {
            return $"{ownerUserId} - {id}";
        }

        [PrimaryKey]
        [JsonRequired]
        public string Key { get; set; }

        [Indexed]
        [JsonRequired]
        public long Id { get; set; }

        /// <summary>
        /// This data's owner user id
        /// </summary>
        [Indexed]
        [JsonRequired]
        public long OwnerUserId { get; set; }

        [Indexed]
        [JsonRequired]
        public long UserId { get; set; }

        /// <summary>
        /// This data's owner user.
        /// Required
        /// </summary>
        [JsonRequired]
        public User OwnerUser { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [JsonRequired]
        public User User { get; set; }

        [JsonProperty]
        public Status RetweetedStatus { get; set; }

        [JsonProperty]
        public Status QuotedStatus { get; set; }

        [JsonRequired]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonRequired]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        [JsonRequired]
        public string Json { get; set; }

        [JsonIgnore]
        private CStatus _coreTweetStatus;
        [Ignored]
        [JsonIgnore]
        public CStatus CoreTweetStatus {
            get {
                if(_coreTweetStatus == null) {
                    _coreTweetStatus = JsonConvert.DeserializeObject<CStatus>(Json);
                    _coreTweetStatus.User = User.CoreTweetUser;
                    _coreTweetStatus.RetweetedStatus = RetweetedStatus?.CoreTweetStatus;
                    _coreTweetStatus.QuotedStatus = QuotedStatus?.CoreTweetStatus;
                }
                return _coreTweetStatus;
            }
        }

        [Ignored]
        [JsonIgnore]
        public bool IsValidObject {
            get {
                return OwnerUser?.IsValidObject == true && User?.IsValidObject == true && Json != null;
            }
        }

        /// <summary>
        /// Backlink property.
        /// use to check this status containing other object when delete this status. 
        /// </summary>
        [Backlink(nameof(Status.RetweetedStatus))]
        public IQueryable<Status> RetweetingStatuses { get; }

        /// <summary>
        /// Backlink property.
        /// use to check this status containing other object when delete this status. 
        /// </summary>
        [Backlink(nameof(Status.QuotedStatus))]
        public IQueryable<Status> QuotingStatuses { get; }

        /// <summary>
        /// Backlink property.
        /// use to check this status containing other object when delete this status. 
        /// </summary>
        [Backlink(nameof(StatusTimelineCache.Statuses))]
        public IQueryable<StatusTimelineCache> StatusTimelineCaches { get; }

        public Status() { }

        public Status(User ownerUser,CStatus status) {
            OwnerUserId = ownerUser.Id;
            OwnerUser = ownerUser;
            Id = status.Id;
            UserId = status.User.Id ?? -1;
            User = new User(status.User);
            if(status.RetweetedStatus != null) {
                RetweetedStatus = new Status(OwnerUser,status.RetweetedStatus);
            }
            if(status.QuotedStatus != null) {
                QuotedStatus = new Status(OwnerUser,status.QuotedStatus);
            }
            UpdatedAt = DateTimeOffset.Now;
            CreatedAt = status.CreatedAt;
            {
                // 軽量化
                var tempUser = status.User;
                var tempRetweetedStatus = status.RetweetedStatus;
                var tempQuotedStatus = status.QuotedStatus;
                status.User = null;
                status.RetweetedStatus = null;
                status.QuotedStatus = null;
                Json = JsonConvert.SerializeObject(status);
                status.User = tempUser;
                status.RetweetedStatus = tempRetweetedStatus;
                status.QuotedStatus = tempQuotedStatus;
            }
            Key = MakePrimaryKey(OwnerUserId,Id);
        }

        public Status(ImmutableStatus item) {
            OwnerUser = new User(item.OwnerUser);
            Id = item.Id;
            UserId = item.UserId;
            User = new User(item.User);
            if(item.RetweetedStatus != null) {
                RetweetedStatus = new Status(item.RetweetedStatus);
            }
            if(item.QuotedStatus != null) {
                QuotedStatus = new Status(item.QuotedStatus);
            }
            UpdatedAt = item.UpdatedAt;
            CreatedAt = item.CreatedAt;
            Json = item.Json;
            Key = item.Key;
        }

    }

    public class ImmutableStatus : Interfaces.IStatus<ImmutableStatus,ImmutableUser> {

        public DateTimeOffset CreatedAt { get; }

        public long Id { get; }

        public string Json { get; }

        public string Key { get; }

        public ImmutableUser OwnerUser { get; }

        public ImmutableStatus QuotedStatus { get; }

        public ImmutableStatus RetweetedStatus { get; }

        public DateTimeOffset UpdatedAt { get; }

        public ImmutableUser User { get; }

        public long UserId { get; }

        [JsonIgnore]
        private CStatus _coreTweetStatus;
        [JsonIgnore]
        public CStatus CoreTweetStatus {
            get {
                if(_coreTweetStatus == null) {
                    _coreTweetStatus = JsonConvert.DeserializeObject<CStatus>(Json);
                    _coreTweetStatus.User = User.CoreTweetUser;
                    _coreTweetStatus.RetweetedStatus = RetweetedStatus?.CoreTweetStatus;
                    _coreTweetStatus.QuotedStatus = QuotedStatus?.CoreTweetStatus;
                }
                return _coreTweetStatus;
            }
        }

        public ImmutableStatus(Status item) {
            CreatedAt = item.CreatedAt;
            Id = item.Id;
            Json = item.Json;
            Key = item.Key;
            OwnerUser = new ImmutableUser(item.OwnerUser);
            if(item.QuotedStatus != null) {
                QuotedStatus = new ImmutableStatus(item.QuotedStatus);
            }
            if(item.RetweetedStatus != null) {
                RetweetedStatus = new ImmutableStatus(item.RetweetedStatus);
            }
            UpdatedAt = item.UpdatedAt;
            User = new ImmutableUser(item.User);
            UserId = item.UserId;
        }
    }
}
