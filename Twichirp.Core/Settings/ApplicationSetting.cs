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
using Newtonsoft.Json.Linq;

namespace Twichirp.Core.Settings {

    public class ApplicationSetting : BaseSetting {

        [JsonProperty]
        public bool IsCleanLaunch {
            get => SettingManager.AppSettings.GetValueOrDefault(MakeSettingName(nameof(IsCleanLaunch)), true);
            set {
                SettingManager.AppSettings.AddOrUpdateValue(MakeSettingName(nameof(IsCleanLaunch)), value);
                RaisePropertyChanged(nameof(IsCleanLaunch));
            }
        }

        public ApplicationSetting(SettingManager settingManager) : base(settingManager, nameof(ApplicationSetting)) {
        }

        public override void ImportJson(JObject jObject) {
            IsCleanLaunch = (bool)jObject[nameof(IsCleanLaunch)];
        }
    }
}
