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
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Twichirp.Core.DataObjects;
using Twichirp.Core.Repositories;
using CStatus = CoreTweet.Status;

namespace Twichirp.Android.Repositories {

    public class JsonConvertedTimelineRepository : ITimelineRepository {

        private List<string> jsonList;

        public JsonConvertedTimelineRepository(List<string> jsonList) {
            this.jsonList = jsonList;
        }

        public async Task<IEnumerable<CStatus>> Load(ImmutableAccount account, int count, long? sinceId = null, long? maxId = null) {
            return await Task.Run(() => {
                var temp = new List<CStatus>();
                foreach (var s in jsonList.Take(count)) {
                    temp.Add(JsonConvert.DeserializeObject<CStatus>(s));
                }
                // maxId以下かつsinceIdより上のを取ってくる
                var list = temp.TakeWhile(x => x.Id <= (maxId ?? long.MaxValue)).SkipWhile(x => x.Id <= (sinceId ?? long.MinValue)).ToList();
                return list;
            });
        }
    }
}