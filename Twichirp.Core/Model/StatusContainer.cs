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
using SQLite.Net.Attributes;

namespace Twichirp.Core.Model {

    [Table("Statuses")]
    public class StatusContainer {

        [Column("_id"), PrimaryKey]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("created_At")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_At")]
        public DateTime UpdatedAt { get; set; }

        [Column("status_json")]
        public string StatusJson { get; set; }

        private Status _status;
        [Ignore]
        public Status Status {
            get {
                if(_status == null) {
                    _status = JsonConvert.DeserializeObject<Status>(StatusJson);
                }
                return _status;
            }
        }

        public StatusContainer() { }

        public StatusContainer(Status status) {
            Id = status.Id;
            UserId = status?.User.Id ?? -1;
            CreatedAt = status.CreatedAt.DateTime;
            UpdatedAt = DateTime.Now;
            StatusJson = JsonConvert.SerializeObject(status);
            _status = status;
        }
    }
}
