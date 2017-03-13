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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Twichirp.Core.DataRepositories;

namespace Twichirp.Core.Settings {

    public class AccountsSetting : BaseSetting {

        [JsonIgnore]
        private IAccountRepository accountRepository;

        [JsonProperty]
        public Dictionary<long, AccountSetting> Accounts { get; set; } = new Dictionary<long, AccountSetting>();

        [JsonProperty]
        public long DefaultAccountId {
            get {
                long result = SettingManager.AppSettings.GetValueOrDefault<long>(MakeSettingName(nameof(DefaultAccountId)), -1);
                if (result == -1) {
                    result = accountRepository.FirstOrDefault()?.Id ?? -1;
                    DefaultAccountId = result;
                }
                return result;
            }
            set {
                SettingManager.AppSettings.AddOrUpdateValue(MakeSettingName(nameof(DefaultAccountId)), value);
                RaisePropertyChanged(nameof(DefaultAccountId));
            }
        }

        public AccountSetting this[long id] {
            get {
                if (Accounts.ContainsKey(id)) {
                    return Accounts[id];
                }
                var account = new AccountSetting(SettingManager, MakeSettingName(id.ToString()));
                Accounts[id] = account;
                return account;
            }
        }

        [JsonIgnore]
        public SettingList<long> AccountUsedOrder { get; private set; }

        public AccountsSetting(SettingManager settingManager, IAccountRepository accountRepository) : base(settingManager, nameof(AccountSetting)) {
            this.accountRepository = accountRepository;
            AccountUsedOrder = new SettingList<long>(settingManager, MakeSettingName(nameof(AccountUsedOrder)));
        }

        public override void ImportJson(JObject jObject) {
            JObject accounts = (JObject)jObject[nameof(Accounts)];
            foreach (var token in accounts.Properties()) {
                long id = long.Parse(token.Name);
                var account = new AccountSetting(SettingManager, MakeSettingName(id.ToString()));
                account.ImportJson((JObject)token.Value);
                Accounts[id] = account;
            }
        }
    }

    public class AccountSetting : BaseSetting {


        public AccountSetting(SettingManager settingManager, string prefix) : base(settingManager, prefix) {
        }

        public override void ImportJson(JObject jObject) {

        }
    }
}
