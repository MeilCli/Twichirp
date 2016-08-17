// Copyright (c) 2016 meil
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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Twichirp.Core.App.Setting {
    public class AccountsSetting : BaseSetting {

        [JsonProperty]
        public Dictionary<long,AccountSetting> Accounts { get; set; } = new Dictionary<long,AccountSetting>();

        [JsonProperty]
        public long DefaultAccountId {
            get {
                return SettingManager.AppSettings.GetValueOrDefault(MakeSettingName(nameof(DefaultAccountId)),SettingManager.Application.AccountManager.Account[0].Id);
            }
            set {
                SettingManager.AppSettings.AddOrUpdateValue(MakeSettingName(nameof(DefaultAccountId)),value);
                RaisePropertyChanged(nameof(DefaultAccountId));
            }
        }

        public AccountSetting this[long id] {
            get {
                if(Accounts.ContainsKey(id)) {
                    return Accounts[id];
                }
                var account = new AccountSetting(SettingManager,MakeSettingName(id.ToString()));
                Accounts[id] = account;
                return account;
            }
        }

        public AccountsSetting(SettingManager settingManager) : base(settingManager,nameof(AccountSetting)) {
        }

        public override void ImportJson(JObject jObject) {
            JObject accounts = (JObject)jObject[nameof(Accounts)];
            foreach(var token in accounts.Properties()) {
                long id = long.Parse(token.Name);
                var account = new AccountSetting(SettingManager,MakeSettingName(id.ToString()));
                account.ImportJson((JObject)token.Value);
                Accounts[id] = account;
            }
        }
    }

    public class AccountSetting : BaseSetting {


        public AccountSetting(SettingManager settingManager,string prefix) : base(settingManager,prefix) {
        }

        public override void ImportJson(JObject jObject) {
            
        }
    }
}
