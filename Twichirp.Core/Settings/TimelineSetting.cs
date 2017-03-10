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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Twichirp.Core.Settings {

    public class TimelineSetting : BaseSetting {

        [JsonProperty]
        public int Count {
            get {
                return SettingManager.AppSettings.GetValueOrDefault(MakeSettingName(nameof(Count)),50);
            }
            set {
                SettingManager.AppSettings.AddOrUpdateValue(MakeSettingName(nameof(Count)),value);
                RaisePropertyChanged(nameof(Count));
            }
        }

        [JsonProperty]
        public int OwnedNumber {
            get {
                return SettingManager.AppSettings.GetValueOrDefault(MakeSettingName(nameof(OwnedNumber)),600);
            }
            set {
                SettingManager.AppSettings.AddOrUpdateValue(MakeSettingName(nameof(OwnedNumber)),value);
                RaisePropertyChanged(nameof(OwnedNumber));
            }
        }

        public TimelineSetting(SettingManager settingManager) : base(settingManager,nameof(TimelineSetting)) {
        }

        public override void ImportJson(JObject jObject) {
            Count = (int)jObject[nameof(Count)];
            OwnedNumber = (int)jObject[nameof(OwnedNumber)];
        }
    }
}
