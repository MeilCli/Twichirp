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
using System.Net.Http;
using CoreTweet;
using Twichirp.Core.Aclog.Api;

namespace Twichirp.Core.Aclog {
    public class AclogClient : IDisposable {

        internal Tokens Token { get; set; }
        public string HostUrl { get; set; } = "https://aclog.rhe.jp";

        private static Dictionary<string, HttpClientHandler> clientHandlerMap = new Dictionary<string, HttpClientHandler>();
        internal HttpClientHandler HttpClientHandler {
            get {
                var key = Token.ToString();
                if (clientHandlerMap.ContainsKey(key)) {
                    return clientHandlerMap[key];
                }
                var clientHandler = new HttpClientHandler { UseCookies = true };
                clientHandlerMap[key] = clientHandler;
                return clientHandler;
            }
        }

        private static Dictionary<string, HttpClient> clientMap = new Dictionary<string, HttpClient>();
        internal HttpClient HttpClient {
            get {
                var key = Token.ToString();
                if (clientMap.ContainsKey(key)) {
                    return clientMap[key];
                }
                var client = new HttpClient(HttpClientHandler);
                clientMap[key] = client;
                return client;
            }
        }

        public Tweet Tweets { get; }
        public Api.User Users { get; }

        public AclogClient(Tokens token) {
            this.Token = token;
            Tweets = new Tweet(this);
            Users = new Api.User(this);
        }

        public void Dispose() {
            foreach (var c in clientMap) {
                c.Value.Dispose();
            }
            foreach (var c in clientHandlerMap) {
                c.Value.Dispose();
            }
        }
    }
}
