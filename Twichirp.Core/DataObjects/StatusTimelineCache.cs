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
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace Twichirp.Core.DataObjects {

    public sealed class StatusTimelineCacheType {

        public static readonly StatusTimelineCacheType HomeTimeline = new StatusTimelineCacheType("HomeTimeline");
        public static readonly StatusTimelineCacheType MentionTimeline = new StatusTimelineCacheType("MentionTimeline");

        public string Tag { get; }

        private StatusTimelineCacheType(string tag) {
            Tag = tag;
        }

        // Immutableなので等値比較系の実装をしておく
        public override bool Equals(object obj) {
            if(obj == null) {
                return false;
            }
            var type = obj as StatusTimelineCacheType;
            if((object)type == null) {
                return false;
            }
            return Tag == type.Tag;
        }

        public bool Equals(StatusTimelineCacheType type) {
            if((object)type == null) {
                return false;
            }
            return Tag == type.Tag;
        }
        
        public override int GetHashCode() {
            return Tag.GetHashCode();
        }

        public static bool operator ==(StatusTimelineCacheType a,StatusTimelineCacheType b) {
            if(ReferenceEquals(a,b)) {
                return true;
            }
            if(((object)a == null) || ((object)b == null)) {
                return false;
            }
            return a.Tag == b.Tag;
        }

        public static bool operator !=(StatusTimelineCacheType a,StatusTimelineCacheType b) {
            return (a == b) == false;
        }
    }

    /// <summary>
    /// Not support converting to JSON
    /// </summary>
    public class StatusTimelineCache : RealmObject, Interfaces.IStatusTimelineCache<Status,User> {

        public static string MakePrimaryKey(long ownerUserId,StatusTimelineCacheType type) {
            return $"{ownerUserId} - {type.Tag}";
        }

        [PrimaryKey]
        public string Key { get; set; }

        /// <summary>
        /// This data's owner user id
        /// </summary>
        [Indexed]
        public long OwnerUserId { get; set; }

        /// <summary>
        /// This data's owner user.
        /// Required
        /// </summary>
        public User OwnerUser { get; set; }

        public IList<Status> Statuses { get; }

        [Ignored]
        public StatusTimelineCacheType Type { get; set; }

        [Ignored]
        public bool IsValidObject {
            get {
                return OwnerUser?.IsValidObject == true;
            }
        }

        public StatusTimelineCache() { }

        public StatusTimelineCache(User ownerUser,IEnumerable<Status> statuses,StatusTimelineCacheType type) {
            OwnerUserId = ownerUser.Id;
            OwnerUser = ownerUser;
            foreach(var status in statuses) {
                Statuses.Add(status);
            }
            Type = type;
            Key = MakePrimaryKey(OwnerUserId,Type);
        }

        public StatusTimelineCache(ImmutableStatusTimelineCache item) {
            OwnerUserId = item.OwnerUserId;
            OwnerUser = new User(item.OwnerUser);
            foreach(var status in item.Statuses) {
                Statuses.Add(new Status(status));
            }
            Type = item.Type;
            Key = item.Key;
        }

    }

    /// <summary>
    /// Not support converting to JSON
    /// </summary>
    public class ImmutableStatusTimelineCache : Interfaces.IStatusTimelineCache<ImmutableStatus,ImmutableUser> {

        public string Key { get; }

        public ImmutableUser OwnerUser { get; }

        public long OwnerUserId { get; }

        /// <summary>
        /// ImmutableList
        /// </summary>
        public IList<ImmutableStatus> Statuses { get; }

        public StatusTimelineCacheType Type { get; }

        public ImmutableStatusTimelineCache(StatusTimelineCache item) {
            Key = item.Key;
            OwnerUser = new ImmutableUser(item.OwnerUser);
            OwnerUserId = item.OwnerUserId;
            Statuses = System.Collections.Immutable.ImmutableList.Create(item.Statuses.Select(x => new ImmutableStatus(x)).ToArray());
            Type = item.Type;
        }
    }
}
