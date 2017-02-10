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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.Model;
using System.Collections;

namespace Twichirp.Core.App.Manager {

    public class AccountManager : IEnumerable<Account> {

        private ITwichirpApplication application;

        private List<Account> account = new List<Account>();

        public Account this[long id] => account.FirstOrDefault(x => x.Id == id);

        public Account this[string screenName] => account.FirstOrDefault(x => x.ScreenName == screenName);

        public AccountManager(ITwichirpApplication application) {
            this.application = application;
            application.DatabaseManager.AccountConnection.CreateTableAsync<Account>().Wait();
        }

        public async Task InitAsync() {
            account = await application.DatabaseManager.AccountConnection.Table<Account>().ToListAsync();
        }

        public async Task AddAsync(Account account) {
            await application.DatabaseManager.AccountConnection.InsertOrReplaceAsync(account);
            if(this.account.Any(x => x.Id == account.Id) == false) {
                this.account.Add(account);
            }
        }

        public async Task RemoveAsync(Account account) {
            if(this.account.Count <= 1) {
                return;
            }
            await application.DatabaseManager.AccountConnection.DeleteAsync(account);
            this.account.Remove(this.account.First(x => x.Id == account.Id));
        }

        public async Task ImportJsonAsync(string json) {
            List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(json);
            if(accounts.Count == 0) {
                return;
            }
            if(accounts.Any(x => x.IsValid == false)) {
                throw new ArgumentException("Invalid Account Object");
            }
            await application.DatabaseManager.AccountConnection.DeleteAllAsync<Account>();
            await application.DatabaseManager.AccountConnection.InsertAllAsync(accounts);
            account = accounts;
        }

        public string ExportJson() => JsonConvert.SerializeObject(account);

        public IEnumerator<Account> GetEnumerator() {
            return account.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
