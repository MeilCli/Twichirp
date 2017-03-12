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
using CoreTweet;
using Newtonsoft.Json;
using Realms;

namespace Twichirp.Core.DataObjects {

    public class Token : RealmObject, Interfaces.IToken<ClientKey> {

        public static string MakePrimaryKey(ClientKey clientKey, string accessToken) {
            return $"{clientKey.ConsumerKey} - {accessToken}";
        }

        [PrimaryKey]
        [JsonRequired]
        public string Key { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [JsonRequired]
        public ClientKey ClientKey { get; set; }

        [Required]
        [JsonRequired]
        public string AccessToken { get; set; }

        [Required]
        [JsonRequired]
        public string AccessTokenSecret { get; set; }

        [Ignored]
        [JsonIgnore]
        public bool IsValidObject {
            get {
                return AccessToken != null && AccessTokenSecret != null && ClientKey?.IsValidObject == true;
            }
        }

        [JsonIgnore]
        private Tokens _coreTweetToken;
        [Ignored]
        [JsonIgnore]
        public Tokens CoreTweetToken {
            get {
                if (_coreTweetToken == null) {
                    _coreTweetToken = Tokens.Create(ClientKey.ConsumerKey, ClientKey.ConsumerSecret, AccessToken, AccessTokenSecret);
                }
                return _coreTweetToken;
            }
        }

        /// <summary>
        /// Backlink property.
        /// use to check this token containing other object when delete this token. 
        /// </summary>
        [Backlink(nameof(Account.Token))]
        public IQueryable<Account> Accounts { get; }

        public Token() { }

        public Token(ClientKey clientKey, string accessToken, string accessTokenSecret) {
            ClientKey = clientKey;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
            Key = MakePrimaryKey(clientKey, accessToken);
        }

        public Token(ImmutableToken item) {
            ClientKey = new ClientKey(item.ClientKey);
            AccessToken = item.AccessToken;
            AccessTokenSecret = item.AccessTokenSecret;
            Key = item.Key;
        }

    }

    public class ImmutableToken : Interfaces.IToken<ImmutableClientKey> {

        public string AccessToken { get; }

        public string AccessTokenSecret { get; }

        public ImmutableClientKey ClientKey { get; }

        public string Key { get; }

        [JsonIgnore]
        private Tokens _coreTweetToken;
        [JsonIgnore]
        public Tokens CoreTweetToken {
            get {
                if (_coreTweetToken == null) {
                    _coreTweetToken = Tokens.Create(ClientKey.ConsumerKey, ClientKey.ConsumerSecret, AccessToken, AccessTokenSecret);
                }
                return _coreTweetToken;
            }
        }

        public ImmutableToken(Token item) {
            AccessToken = item.AccessToken;
            AccessTokenSecret = item.AccessTokenSecret;
            ClientKey = new ImmutableClientKey(item.ClientKey);
            Key = item.Key;
        }
    }
}
