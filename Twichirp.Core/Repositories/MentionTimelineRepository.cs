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
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreTweet;
using Twichirp.Core.DataObjects;

namespace Twichirp.Core.Repositories {

    public class MentionTimelineRepository : ITimelineRepository {

        public MentionTimelineRepository() { }

        public async Task<IEnumerable<CoreTweet.Status>> Load(ImmutableAccount account, int count, long? sinceId = null, long? maxId = null) {
            return await account.CoreTweetToken.Statuses.MentionsTimelineAsync(
                    count: count,
                    since_id: sinceId,
                    max_id: maxId,
                    include_entities: true,
                    include_ext_alt_text: true,
                    tweet_mode: TweetMode.extended
                );
        }
    }
}
