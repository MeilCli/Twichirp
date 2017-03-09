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
using CoreTweet;
using Newtonsoft.Json;
using Realms;

namespace Twichirp.Core.DataObjects {

    public class Account : RealmObject, Interfaces.IAccount<Token,User> {

        [PrimaryKey]
        [JsonRequired]
        public long Id { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [JsonRequired]
        public Token Token { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [JsonRequired]
        public User User { get; set; }

        [Ignored]
        [JsonIgnore]
        public string ScreenName => User.ScreenName;

        [Ignored]
        [JsonIgnore]
        public string Name => User.Name;

        [Ignored]
        [JsonIgnore]
        public Tokens CoreTweetToken => Token.CoreTweetToken;

        [Ignored]
        [JsonIgnore]
        public bool IsValidObject {
            get {
                return Token?.IsValidObject == true && User?.IsValidObject == true;
            }
        }

        public Account() { }

        public Account(User user,Token token) {
            Id = user.Id;
            User = user;
            Token = token;
        }

        public Account(ImmutableAccount item) {
            Id = item.Id;
            User = new User(item.User);
            Token = new Token(item.Token);
        }

    }

    public class ImmutableAccount : Interfaces.IAccount<ImmutableToken,ImmutableUser> {

        public long Id { get; }

        public ImmutableToken Token { get; }

        public ImmutableUser User { get; }

        [JsonIgnore]
        public string ScreenName => User.ScreenName;

        [JsonIgnore]
        public string Name => User.Name;

        [JsonIgnore]
        public Tokens CoreTweetToken => Token.CoreTweetToken;

        public ImmutableAccount(Account item) {
            Id = item.Id;
            Token = new ImmutableToken(item.Token);
            User = new ImmutableUser(item.User);
        }
    }
}
