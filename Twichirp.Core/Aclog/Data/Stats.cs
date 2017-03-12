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

namespace Twichirp.Core.Aclog.Data {
    public class Stats {

        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("reactions_count")]
        public int ReactionsCount { get; set; }
        [JsonProperty("registered")]
        public bool IsRegistered { get; set; }
    }
}
