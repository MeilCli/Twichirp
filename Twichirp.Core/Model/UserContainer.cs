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

    [Table("Users")]
    public class UserContainer {

        [Column("_id"),PrimaryKey]
        public long Id { get; set; }

        [Column("screen_name")]
        public string ScreenName { get; set; }

        [Column("user_json")]
        public string UserJson { get; set; }

        [Column("updated_At")]
        public DateTime UpdatedAt { get; set; }

        private User _user;
        [Ignore]
        public User User {
            get {
                if(_user == null) {
                    _user = JsonConvert.DeserializeObject<User>(UserJson);
                    _user.Status = null; //軽量化
                }
                return _user;
            }
        }

        public UserContainer() { }

        public UserContainer(User user) {
            user.Status = null; //軽量化
            Id = user.Id ?? -1;
            ScreenName = user.ScreenName;
            UserJson = JsonConvert.SerializeObject(user);
            UpdatedAt = DateTime.Now;
            _user = user;
        }

    }
}
