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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.Model {
    public class Timeline<TResult> {

        public const int Undefined = 0;
        public const int Home = 1;
        public const int Mention = 2;
        public const int User = 3;
        public const int Favorite = 4;

        private Func<Account,int,long?,long?,Task<TResult>> load;
        public int Type { get; }
        public Account Account { get; }

        public Timeline(Func<Account,int,long?,long?,Task<TResult>> load,int type) {
            this.load = load;
            this.Type = type;
        }

        public async Task<TResult> Load(Account account,int count,long? sinceId = null,long? maxId = null) => await load(account,count,sinceId,maxId);

        public static Timeline<IEnumerable<Status>> HomeTimeline() {
            Func<Account,int,long?,long?,Task<IEnumerable<Status>>> load = async (account,x,y,z) => {
                return await account.Token.Statuses.HomeTimelineAsync(count: x,since_id: y,max_id: z,include_entities: true,include_ext_alt_text: true,tweet_mode: TweetMode.extended);
            };
            return new Timeline<IEnumerable<Status>>(load,Home);
        }

        public static Timeline<IEnumerable<Status>> MentionTimeline() {
            Func<Account,int,long?,long?,Task<IEnumerable<Status>>> load = async (account,x,y,z) => {
                return await account.Token.Statuses.MentionsTimelineAsync(count: x,since_id: y,max_id: z,include_entities: true,include_ext_alt_text: true,tweet_mode: TweetMode.extended);
            };
            return new Timeline<IEnumerable<Status>>(load,Mention);
        }

        public static Timeline<IEnumerable<Status>> UserTimeline(long userId) {
            Func<Account,int,long?,long?,Task<IEnumerable<Status>>> load = async (account,x,y,z) => {
                return await account.Token.Statuses.UserTimelineAsync(count: x,since_id: y,max_id: z,user_id: userId,include_rts: true,include_ext_alt_text: true,tweet_mode: TweetMode.extended);
            };
            return new Timeline<IEnumerable<Status>>(load,Mention);
        }

        public static Timeline<IEnumerable<Status>> FavoriteTimeline(long userId) {
            Func<Account,int,long?,long?,Task<IEnumerable<Status>>> load = async (account,x,y,z) => {
                return await account.Token.Favorites.ListAsync(count: x,since_id: y,max_id: z,id: userId,include_entities: true,include_ext_alt_text: true,tweet_mode: TweetMode.extended);
            };
            return new Timeline<IEnumerable<Status>>(load,Mention);
        }
    }
}
