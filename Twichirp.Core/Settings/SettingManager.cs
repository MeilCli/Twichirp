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
using System.Linq;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ComponentModel;
using Twichirp.Core.DataRepositories;

namespace Twichirp.Core.Settings {

    public class SettingManager : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public ISettings AppSettings => CrossSettings.Current;

        [JsonIgnore]
        internal static IMigration[] Migrations = { };

        [JsonIgnore]
        public const int CurrentVersion = 1;

        [JsonProperty]
        public int SettingVersion {
            get {
                return AppSettings.GetValueOrDefault(nameof(SettingVersion),CurrentVersion);
            }
            set {
                AppSettings.AddOrUpdateValue(nameof(SettingVersion),value);
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(SettingVersion)));
            }
        }

        [JsonProperty]
        public AccountsSetting Accounts { get; }

        [JsonProperty]
        public ApplicationSetting Applications { get; }

        [JsonProperty]
        public TimelineSetting Timeline { get; }

        [JsonIgnore]
        public bool NeedMigrate => SettingVersion < CurrentVersion;

        public SettingManager(IAccountRepository accountRepository) {
            Accounts = new AccountsSetting(this,accountRepository);
            Applications = new ApplicationSetting(this);
            Timeline = new TimelineSetting(this);
        }

        public void Migrate() {
            foreach(var m in Migrations.OrderBy(x => x.MigrateVersion).Where(x => x.MigrateVersion >= SettingVersion)) {
                m.Migrate(this);
            }
            SettingVersion = CurrentVersion;
        }

        public void ImportJson(string json) {
            var jObject = JObject.Parse(json);
            SettingVersion = (int)jObject[nameof(SettingVersion)];
            Accounts.ImportJson((JObject)jObject[nameof(Accounts)]);
            Timeline.ImportJson((JObject)jObject[nameof(Timeline)]);
        }

        public string ExportJson() => JsonConvert.SerializeObject(this);

    }
}