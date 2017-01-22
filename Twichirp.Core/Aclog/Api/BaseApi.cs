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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.Aclog.Api {
    public abstract class BaseApi {

        private AclogClient aclogClient;
        private string verifyUrl = "https://api.twitter.com/1.1/account/verify_credentials.json";
        protected string HostUrl => aclogClient.HostUrl;

        public BaseApi(AclogClient aclogClient) {
            this.aclogClient = aclogClient;
        }

        protected async Task<string> GetContent(HttpRequestMessage message) {
            string authorizationHeader = aclogClient.Token.CreateAuthorizationHeader(CoreTweet.MethodType.Get,new Uri(verifyUrl),null);
            message.Headers.Add("X-Verify-Credentials-Authorization",authorizationHeader);
            message.Headers.Add("X-Auth-Service-Provider",verifyUrl);
            var response = await aclogClient.HttpClient.SendAsync(message);
            if(response.IsSuccessStatusCode == false) {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
