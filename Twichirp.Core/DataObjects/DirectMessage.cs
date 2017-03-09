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
using CDirectMessage = CoreTweet.DirectMessage;

namespace Twichirp.Core.DataObjects {

    public class DirectMessage : RealmObject, Interfaces.IDirectMessage<User> {

        [PrimaryKey]
        [JsonRequired]
        public long Id { get; set; }

        [Required]
        [JsonRequired]
        public string Json { get; set; }

        [JsonRequired]
        public User Recipient { get; set; }

        [Indexed]
        [JsonRequired]
        public long RecipientId { get; set; }

        [JsonRequired]
        public User Sender { get; set; }

        [Indexed]
        [JsonRequired]
        public long SenderId { get; set; }

        [JsonRequired]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonRequired]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonIgnore]
        private CDirectMessage _coreTweetDirectMessage;
        [Ignored]
        [JsonIgnore]
        public CDirectMessage CoreTweetDirectMessage {
            get {
                if(_coreTweetDirectMessage == null) {
                    _coreTweetDirectMessage = JsonConvert.DeserializeObject<CDirectMessage>(Json);
                    _coreTweetDirectMessage.Recipient = Recipient.CoreTweetUser;
                    _coreTweetDirectMessage.Sender = Sender.CoreTweetUser;
                }
                return _coreTweetDirectMessage;
            }
        }

        [Ignored]
        [JsonIgnore]
        public bool IsValidObject {
            get {
                return Recipient?.IsValidObject == true && Sender?.IsValidObject == true && Json != null;
            }
        }

        public DirectMessage() { }

        public DirectMessage(CDirectMessage directMessage) {
            Id = directMessage.Id;
            {
                // 軽量化
                var tempRecipient = directMessage.Recipient;
                var tempSender = directMessage.Sender;
                directMessage.Recipient = null;
                directMessage.Sender = null;
                Json = JsonConvert.SerializeObject(directMessage);
                directMessage.Recipient = tempRecipient;
                directMessage.Sender = tempSender;
            }
            Recipient = new User(directMessage.Recipient);
            RecipientId = Recipient.Id;
            Sender = new User(directMessage.Sender);
            SenderId = Sender.Id;
            UpdatedAt = DateTimeOffset.Now;
            CreatedAt = directMessage.CreatedAt;
        }

        public DirectMessage(ImmutableDirectMessage item) {
            Id = item.Id;
            Json = item.Json;
            Recipient = new User(item.Recipient);
            RecipientId = item.RecipientId;
            Sender = new User(item.Sender);
            SenderId = item.SenderId;
            UpdatedAt = item.UpdatedAt;
            CreatedAt = item.CreatedAt;
        }
    }

    public class ImmutableDirectMessage : Interfaces.IDirectMessage<ImmutableUser> {

        public long Id { get; }

        public string Json { get; }

        public ImmutableUser Recipient { get; }

        public long RecipientId { get; }

        public ImmutableUser Sender { get; }

        public long SenderId { get; }

        public DateTimeOffset UpdatedAt { get; }

        public DateTimeOffset CreatedAt { get; }

        [JsonIgnore]
        private CDirectMessage _coreTweetDirectMessage;
        [Ignored]
        [JsonIgnore]
        public CDirectMessage CoreTweetDirectMessage {
            get {
                if(_coreTweetDirectMessage == null) {
                    _coreTweetDirectMessage = JsonConvert.DeserializeObject<CDirectMessage>(Json);
                    _coreTweetDirectMessage.Recipient = Recipient.CoreTweetUser;
                    _coreTweetDirectMessage.Sender = Sender.CoreTweetUser;
                }
                return _coreTweetDirectMessage;
            }
        }

        public ImmutableDirectMessage(DirectMessage item) {
            Id = item.Id;
            Json = item.Json;
            Recipient = new ImmutableUser(item.Recipient);
            RecipientId = item.RecipientId;
            Sender = new ImmutableUser(item.Sender);
            SenderId = item.SenderId;
            UpdatedAt = item.UpdatedAt;
            CreatedAt = item.CreatedAt;
        }

    }
}
