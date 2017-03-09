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
using CList = CoreTweet.List;

namespace Twichirp.Core.DataObjects {

    public class List : RealmObject, Interfaces.IList<User> {

        [PrimaryKey]
        [JsonProperty]
        public long Id { get; set; }

        [Required]
        [JsonRequired]
        public string Json { get; set; }

        [JsonRequired]
        public User User { get; set; }

        [JsonProperty]
        public long UserId { get; set; }

        [JsonRequired]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonIgnore]
        private CList _coreTweetList;
        [Ignored]
        [JsonIgnore]
        public CList CoreTweetList {
            get {
                if(_coreTweetList == null) {
                    _coreTweetList = JsonConvert.DeserializeObject<CList>(Json);
                    _coreTweetList.User = User.CoreTweetUser;
                }
                return _coreTweetList;
            }
        }

        [Ignored]
        [JsonIgnore]
        public bool IsValidObject {
            get {
                return Json != null;
            }
        }

        /// <summary>
        /// Backlink property.
        /// use to check this list containing other object when delete this list.
        /// </summary>
        [Backlink(nameof(ListCollectionCache.Lists))]
        public IQueryable<ListCollectionCache> ListCollectionCaches { get; }

        public List() { }

        public List(CList list) {
            Id = list.Id;
            {
                //軽量化
                var temp = list.User;
                list.User = null;
                Json = JsonConvert.SerializeObject(list);
                list.User = temp;
            }
            User = new User(list.User);
            UserId = list.User.Id ?? -1;
            UpdatedAt = DateTimeOffset.Now;
        }

        public List(ImmutableList item) {
            Id = item.Id;
            Json = item.Json;
            User = new User(item.User);
            UserId = item.UserId;
            UpdatedAt = item.UpdatedAt;
        }
    }

    public class ImmutableList : Interfaces.IList<ImmutableUser> {

        public long Id { get; }

        public string Json { get; }

        public DateTimeOffset UpdatedAt { get; }

        public ImmutableUser User { get; }

        public long UserId { get; }

        [JsonIgnore]
        private CList _coreTweetList;
        [JsonIgnore]
        public CList CoreTweetList {
            get {
                if(_coreTweetList == null) {
                    _coreTweetList = JsonConvert.DeserializeObject<CList>(Json);
                    _coreTweetList.User = User.CoreTweetUser;
                }
                return _coreTweetList;
            }
        }

        public ImmutableList(List item) {
            Id = item.Id;
            Json = item.Json;
            UpdatedAt = item.UpdatedAt;
            User = new ImmutableUser(item.User);
            UserId = item.UserId;
        }
    }
}
