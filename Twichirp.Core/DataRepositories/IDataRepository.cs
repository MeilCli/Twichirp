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

namespace Twichirp.Core.DataRepositories {

    public interface IDataRepository<TPrimaryKey, TData, TImmutableData> where TData : RealmObject {

        TImmutableData AddOrUpdate(TData item);

        IEnumerable<TImmutableData> AddOrUpdateAll(IEnumerable<TData> items);

        void Remove(TData item);

        void RemoveAll(IEnumerable<TData> items);

        void RemoveRange(Func<IQueryable<TData>,IQueryable<TData>> querySelecter);

        void Clear();

        TImmutableData Find(TPrimaryKey key);

        IEnumerable<TImmutableData> Get(Func<IQueryable<TData>,IQueryable<TData>> querySelecter = null,Func<IEnumerable<TData>,IEnumerable<TData>> enumerableSelecter = null);

        TImmutableData FirstOrDefault();

        TImmutableData LastOrDefault();

        int Count(Func<IQueryable<TData>,IQueryable<TData>> querySelecter = null);
    }
}
