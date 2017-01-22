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
using CoreTweet;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.Model {
    [Table("Accounts")]
    public class Account {

        [JsonProperty]
        [Column("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty]
        [Column("_id"),PrimaryKey]
        public long Id { get; set; }

        [JsonProperty]
        [Column("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty]
        [Column("access_token_secret")]
        public string AccessTokenSecret { get; set; }

        [JsonProperty]
        [Column("consumer_key")]
        public string ConsumerKey { get; set; }

        [JsonProperty]
        [Column("consumer_secret")]
        public string ConsumerSecret { get; set; }

        [JsonProperty]
        [Column("client_name")]
        public string ClientName { get; set; }

        [JsonIgnore]
        private Tokens _token;

        [JsonIgnore]
        [Ignore]
        public Tokens Token {
            get {
                if(_token == null) {
                    _token = toToken();
                }
                return _token;
            }
        }

        [JsonIgnore]
        [Ignore]
        public bool IsValid {
            get {
                return AccessToken != null && AccessTokenSecret != null && ConsumerKey != null && ConsumerSecret != null && ClientName != null;
            }
        }

        /// <summary>
        /// nullable
        /// </summary>
        [JsonIgnore]
        [Ignore]
        public User User { get; set; }

        public Account() { }

        public Account(Tokens token,Consumer consumer) {
            Id = token.UserId;
            ScreenName = token.ScreenName;
            ConsumerKey = consumer.ConsumerKey;
            ConsumerSecret = consumer.ConsumerSecret;
            ClientName = consumer.ClientName;
            AccessToken = token.AccessToken;
            AccessTokenSecret = token.AccessTokenSecret;
            _token = token;
        }

        private Tokens toToken() {
            return Tokens.Create(ConsumerKey,ConsumerSecret,AccessToken,AccessTokenSecret,Id,ScreenName);
        }
    }
}
