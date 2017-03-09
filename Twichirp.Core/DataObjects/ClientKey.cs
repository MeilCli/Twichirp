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
using Newtonsoft.Json;
using Realms;

namespace Twichirp.Core.DataObjects {

    public class ClientKey : RealmObject, Interfaces.IClientKey {

        [PrimaryKey]
        [JsonRequired]
        public string ConsumerKey { get; set; }

        [Required]
        [JsonRequired]
        public string ConsumerSecret { get; set; }

        [Required]
        [JsonRequired]
        public string ClientName { get; set; }

        [Ignored]
        [JsonIgnore]
        public bool IsValidObject {
            get {
                return ConsumerKey != null && ConsumerSecret != null && ClientName != null;
            }
        }

        /// <summary>
        /// Backlink property.
        /// use to check this client key containing other object when delete this client key. 
        /// </summary>
        [Backlink(nameof(Token.ClientKey))]
        public IQueryable<Token> Tokens { get; }

        public ClientKey() { }

        public ClientKey(string consumerKey,string consumerSecret,string clientName) {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            ClientName = clientName;
        }

        public ClientKey(ImmutableClientKey item) {
            ConsumerKey = item.ConsumerKey;
            ConsumerSecret = item.ConsumerSecret;
            ClientName = item.ClientName;
        }
    }

    public class ImmutableClientKey : Interfaces.IClientKey {

        public string ClientName { get; }

        public string ConsumerKey { get; }

        public string ConsumerSecret { get; }

        public ImmutableClientKey(ClientKey item) {
            ClientName = item.ClientName;
            ConsumerKey = item.ConsumerKey;
            ConsumerSecret = item.ConsumerSecret;
        }

        public ImmutableClientKey(string consumerKey,string consumerSecret,string clientName) {
            ClientName = clientName;
            ConsumerKey = consumerKey;
            ConsumerSecret = ConsumerSecret;
        }
    }
}
