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
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Twichirp.Core.Settings {

    // バックアップの対象外としておく
    public class SettingList<T> : IEnumerable<T> {

        private List<T> list;
        private string key;
        private SettingManager settingManager;

        public int Count => list.Count;

        public T this[int index] {
            get {
                return list[index];
            }
            set {
                list[index] = value;
                SaveValues();
            }
        }

        public SettingList(SettingManager settingManager, string key) {
            this.settingManager = settingManager;
            this.key = key;

            list = JsonConvert.DeserializeObject<List<T>>(settingManager.AppSettings.GetValueOrDefault(key, "[]"));
        }

        public void Add(T value) {
            list.Add(value);
            SaveValues();
        }

        public void Insert(int index, T value) {
            list.Insert(index, value);
            SaveValues();
        }

        public void Remove(T value) {
            list.Remove(value);
            SaveValues();
        }

        public void Remove(int index) {
            list.RemoveAt(index);
            SaveValues();
        }

        public bool Contains(T value) => list.Contains(value);

        public void SaveValues() {
            settingManager.AppSettings.AddOrUpdateValue(key, JsonConvert.SerializeObject(list));
        }

        public IEnumerator<T> GetEnumerator() {
            foreach (var v in list) {
                yield return v;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
