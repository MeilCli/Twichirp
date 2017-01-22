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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.Aclog.Data;
using AUser = Twichirp.Core.Aclog.Data.User;

namespace Twichirp.Core.Aclog.Api {
    public class User : BaseApi {

        public User(AclogClient aclogClient) : base(aclogClient) {
        }

        public async Task<List<AUser>> DiscoveredBy(long? id=null,string screenName=null) {
            var parameter = new Dictionary<string,object> { { "id",id },{ "screen_name",screenName } }
                .Where(x=>x.Value!=null)
                .Select(x=>$"{x.Key}={x.Value}");
            var url = $"{HostUrl}/api/users/discovered_by.json?{string.Join("&",parameter)}";
            var message = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await GetContent(message);
            return JsonConvert.DeserializeObject<List<AUser>>(response);
        }

        public async Task<List<AUser>> DiscoveredUsers(long? id = null,string screenName = null) {
            var parameter = new Dictionary<string,object> { { "id",id },{ "screen_name",screenName } }
                .Where(x => x.Value != null)
                .Select(x => $"{x.Key}={x.Value}");
            var url = $"{HostUrl}/api/users/discovered_users.json?{string.Join("&",parameter)}";
            var message = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await GetContent(message);
            return JsonConvert.DeserializeObject<List<AUser>>(response);
        }

        public async Task<Stats> Stats(long? id = null,string screenName = null) {
            var parameter = new Dictionary<string,object> { { "id",id },{ "screen_name",screenName } }
                .Where(x => x.Value != null)
                .Select(x => $"{x.Key}={x.Value}");
            var url = $"{HostUrl}/api/users/stats.json?{string.Join("&",parameter)}";
            var message = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await GetContent(message);
            return JsonConvert.DeserializeObject<Stats>(response);
        }
    }
}
