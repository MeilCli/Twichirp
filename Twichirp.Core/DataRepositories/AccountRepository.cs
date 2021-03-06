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
using System.Linq;
using Twichirp.Core.DataObjects;
using Twichirp.Core.Services;

namespace Twichirp.Core.DataRepositories {

    public class AccountRepository : BaseLongPrimaryKeyDataRepository<Account, ImmutableAccount>, IAccountRepository {

        public AccountRepository(IRealmService realmService) : base(realmService) {
        }

        public ImmutableAccount this[string screenName] {
            get {
                return Get().Where(x => x.ScreenName == screenName).FirstOrDefault();
            }
        }

        public ImmutableAccount this[long id] {
            get {
                return Find(id);
            }
        }

        protected override ImmutableAccount ToImmutable(Account item) {
            return new ImmutableAccount(item);
        }
    }
}
