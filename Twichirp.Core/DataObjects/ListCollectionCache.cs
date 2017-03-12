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
using System.Collections.Generic;
using System.Linq;
using Realms;

namespace Twichirp.Core.DataObjects {

    public sealed class ListCollectionCacheType {

        public static readonly ListCollectionCacheType OwnershipCollection = new ListCollectionCacheType("Ownership");

        public string Tag { get; }

        private ListCollectionCacheType(string tag) {
            Tag = tag;
        }

        // Immutableなので等値比較系の実装をしておく
        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }
            var type = obj as ListCollectionCacheType;
            if ((object)type == null) {
                return false;
            }
            return Tag == type.Tag;
        }

        public bool Equals(ListCollectionCacheType type) {
            if ((object)type == null) {
                return false;
            }
            return Tag == type.Tag;
        }

        public override int GetHashCode() {
            return Tag.GetHashCode();
        }

        public static bool operator ==(ListCollectionCacheType a, ListCollectionCacheType b) {
            if (ReferenceEquals(a, b)) {
                return true;
            }
            if (((object)a == null) || ((object)b == null)) {
                return false;
            }
            return a.Tag == b.Tag;
        }

        public static bool operator !=(ListCollectionCacheType a, ListCollectionCacheType b) {
            return (a == b) == false;
        }
    }

    /// <summary>
    /// Not support converting to JSON
    /// </summary>
    public class ListCollectionCache : RealmObject, Interfaces.IListCollectionCache<User, List> {

        public static string MakePrimaryKey(long ownerUserId, ListCollectionCacheType type) {
            return $"{ownerUserId} - {type.Tag}";
        }

        [PrimaryKey]
        public string Key { get; set; }

        public IList<List> Lists { get; }

        public User OwnerUser { get; set; }

        [Indexed]
        public long OwnerUserId { get; set; }

        [Ignored]
        public ListCollectionCacheType Type { get; set; }

        [Ignored]
        public bool IsValidObject {
            get {
                return OwnerUser?.IsValidObject == true;
            }
        }

        public ListCollectionCache() { }

        public ListCollectionCache(User ownerUser, IEnumerable<List> lists, ListCollectionCacheType type) {
            OwnerUser = ownerUser;
            OwnerUserId = ownerUser.Id;
            foreach (var list in lists) {
                Lists.Add(list);
            }
            Type = type;
            Key = MakePrimaryKey(OwnerUserId, Type);
        }

        public ListCollectionCache(ImmutableListCollectionCache item) {
            OwnerUser = new User(item.OwnerUser);
            OwnerUserId = item.OwnerUserId;
            foreach (var list in item.Lists) {
                Lists.Add(new List(list));
            }
            Type = item.Type;
            Key = item.Key;
        }

    }

    /// <summary>
    /// Not support converting to JSON
    /// </summary>
    public class ImmutableListCollectionCache : Interfaces.IListCollectionCache<ImmutableUser, ImmutableList> {

        public string Key { get; }

        public IList<ImmutableList> Lists { get; }

        public ImmutableUser OwnerUser { get; }

        public long OwnerUserId { get; }

        public ListCollectionCacheType Type { get; set; }

        public ImmutableListCollectionCache(ListCollectionCache item) {
            Key = item.Key;
            Lists = System.Collections.Immutable.ImmutableList.Create(item.Lists.Select(x => new ImmutableList(x)).ToArray());
            OwnerUser = new ImmutableUser(item.OwnerUser);
            OwnerUserId = item.OwnerUserId;
            Type = item.Type;
        }
    }
}
