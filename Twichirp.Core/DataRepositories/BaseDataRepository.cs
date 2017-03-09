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
using Realms;
using Twichirp.Core.Services;

namespace Twichirp.Core.DataRepositories {

    public abstract class BaseDataRepository<TPrimaryKey, TData, TImmutableData> : IDataRepository<TPrimaryKey,TData,TImmutableData> where TData : RealmObject {

        protected IRealmService RealmService { get; }

        public BaseDataRepository(IRealmService realmService) {
            RealmService = realmService;
        }

        protected abstract TImmutableData ToImmutable(TData item);

        public TImmutableData AddOrUpdate(TData item) {
            using(var realm = RealmService.GetRealm()) {
                TData result = null;
                realm.Write(() => {
                    result = realm.Add(item,update: true);
                });
                return ToImmutable(result);
            }
        }

        public IEnumerable<TImmutableData> AddOrUpdateAll(IEnumerable<TData> items) {
            using(var realm = RealmService.GetRealm()) {
                var result = new List<TData>();
                realm.Write(() => {
                    foreach(var item in items) {
                        result.Add(realm.Add(item,update: true));
                    }
                });
                return result.Select(x => ToImmutable(x)).ToList();
            }
        }

        public void Clear() {
            using(var realm = RealmService.GetRealm()) {
                realm.Write(() => realm.RemoveAll<TData>());
            }
        }

        public int Count(Func<IQueryable<TData>,IQueryable<TData>> querySelecter = null) {
            using(var realm = RealmService.GetRealm()) {
                var query = realm.All<TData>();
                query = querySelecter?.Invoke(query) ?? query;
                return query.Count();
            }
        }

        public IEnumerable<TImmutableData> Get(Func<IQueryable<TData>,IQueryable<TData>> querySelecter = null,Func<IEnumerable<TData>,IEnumerable<TData>> enumerableSelecter=null) {
            using(var realm = RealmService.GetRealm()) {
                var query = realm.All<TData>();
                query = querySelecter?.Invoke(query) ?? query;
                IEnumerable<TData> enumerable = query.ToList();
                enumerable = enumerableSelecter?.Invoke(enumerable) ?? enumerable;
                return enumerable.Select(x => ToImmutable(x)).ToList();
            }
        }

        public TImmutableData FirstOrDefault() {
            using(var realm = RealmService.GetRealm()) {
                var result = realm.All<TData>().ToList().FirstOrDefault();
                return result != null ? ToImmutable(result) : default(TImmutableData);
            }
        }

        public TImmutableData LastOrDefault() {
            using(var realm = RealmService.GetRealm()) {
                var result = realm.All<TData>().ToList().LastOrDefault();
                return result != null ? ToImmutable(result) : default(TImmutableData);
            }
        }

        public void Remove(TData item) {
            using(var realm = RealmService.GetRealm()) {
                realm.Write(() => realm.Remove(item));
            }
        }

        public void RemoveAll(IEnumerable<TData> items) {
            using(var realm = RealmService.GetRealm()) {
                realm.Write(() => {
                    foreach(var item in items) {
                        realm.Remove(item);
                    }
                });
            }
        }

        public void RemoveRange(Func<IQueryable<TData>,IQueryable<TData>> querySelecter) {
            using(var realm = RealmService.GetRealm()) {
                realm.Write(() => {
                    var query = querySelecter(realm.All<TData>());
                    realm.RemoveRange(query);
                });
            }
        }

        public abstract TImmutableData Find(TPrimaryKey key);
    }
}
