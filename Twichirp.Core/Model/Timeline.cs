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
using CoreTweet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.Model {
    public class Timeline<TResult> {

        private Func<int,long?,Task<TResult>> load;

        public Timeline(Func<int,long?,Task<TResult>> load) {
            this.load = load;
        }

        public async Task<TResult> Load(int count,long? sinceId = null) => await load(count,sinceId);

        public static Timeline<IEnumerable<Status>> HomeTimeline(Account account) {
            Func<int,long?,Task<IEnumerable<Status>>> load = async (x,y) => {
                return await account.Token.Statuses.HomeTimelineAsync(count: x,since_id: y,include_entities: true,include_ext_alt_text: true,tweet_mode: TweetMode.extended);
            };
            return new Timeline<IEnumerable<Status>>(load);
        }
    }
}
