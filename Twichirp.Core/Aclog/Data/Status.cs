// Copyright (c) 2016 meil
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
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.Aclog.Data {
    public class Status {

        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("user_id")]
        public long UserId { get; set; }
        [JsonProperty("favorites_count")]
        public int FavoritesCount { get; set; }
        [JsonProperty("retweets_count")]
        public int RetweetsCount { get; set; }
        [JsonProperty("favoriters")]
        public long[] Favoriters { get; set; }
        [JsonProperty("retweeters")]
        public long[] Retweeters { get; set; }
    }
}
