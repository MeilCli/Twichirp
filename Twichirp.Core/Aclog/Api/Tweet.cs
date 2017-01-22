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

namespace Twichirp.Core.Aclog.Api {
    public class Tweet : BaseApi {

        public Tweet(AclogClient aclogClient) : base(aclogClient) {
        }

        public async Task<List<Status>> LookUp(IEnumerable<long> ids) {
            string url = $"{HostUrl}/api/tweets/lookup.json?ids={string.Join(",",ids)}";
            var message = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await GetContent(message);
            return JsonConvert.DeserializeObject<List<Status>>(response);
        }

        public async Task<Status> Show(long id) {
            string url = $"{HostUrl}/api/tweets/show.json?id={id}";
            var message = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await GetContent(message);
            return JsonConvert.DeserializeObject<Status>(response);
        }

        public async Task<List<Status>> UserBest(long? userId=null,string screenName=null,int? count = 20,int? page =1,string recent = null) {
            var parameter = new Dictionary<string,object> { { "user_id",userId },{ "screen_name",screenName },{ "count",count },{ "page",page },{ "recent",recent } }
                .Where(x => x.Value != null)
                .Select(x => $"{x.Key}={x.Value}");
            var url = $"{HostUrl}/api/tweets/user_best.json?{string.Join("&",parameter)}";
            var message = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await GetContent(message);
            return JsonConvert.DeserializeObject<List<Status>>(response);
        }

        public async Task<List<Status>> UserFavoritedBy(long? userId=null,string screenName=null,long? sourceUserId=null,string sourceScreenName=null,
            int? count=20,int? page=0,long? sinceId=null,long? maxId=null,int? reactions=null) {
            var parameter = new Dictionary<string,object> { { "user_id",userId },{ "screen_name",screenName },{ "source_user_id",sourceUserId },{ "source_screen_name",sourceScreenName},
                {"count",count },{"page",page },{"since_id",sinceId },{"max_id" ,maxId},{"reactions" ,reactions}}
                .Where(x => x.Value != null)
                .Select(x => $"{x.Key}={x.Value}");
            var url = $"{HostUrl}/api/tweets/user_favorited_by.json?{string.Join("&",parameter)}";
            var message = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await GetContent(message);
            return JsonConvert.DeserializeObject<List<Status>>(response);
        }

        public async Task<List<Status>> UserFavorites(long? userId = null,string screenName = null,int? count = 20,int? page = 0,int? reactions = null) {
            var parameter = new Dictionary<string,object> { { "user_id",userId },{ "screen_name",screenName },{"count",count },{"page",page },{"reactions" ,reactions}}
                .Where(x => x.Value != null)
                .Select(x => $"{x.Key}={x.Value}");
            var url = $"{HostUrl}/api/tweets/user_favorites.json?{string.Join("&",parameter)}";
            var message = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await GetContent(message);
            return JsonConvert.DeserializeObject<List<Status>>(response);
        }

        public async Task<List<Status>> UserTimeline(long? userId = null,string screenName = null,int? count = 20,int? page = 0,long? sinceId = null,long? maxId = null,int? reactions = null) {
            var parameter = new Dictionary<string,object> { { "user_id",userId },{ "screen_name",screenName },{"count",count },{"page",page },{"since_id",sinceId },{"max_id" ,maxId},{"reactions" ,reactions}}
                .Where(x => x.Value != null)
                .Select(x => $"{x.Key}={x.Value}");
            var url = $"{HostUrl}/api/tweets/user_timeline.json?{string.Join("&",parameter)}";
            var message = new HttpRequestMessage(HttpMethod.Get,url);
            var response = await GetContent(message);
            return JsonConvert.DeserializeObject<List<Status>>(response);
        }
    }
}
