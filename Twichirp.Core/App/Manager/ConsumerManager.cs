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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.Model;

namespace Twichirp.Core.App.Manager {
    public class ConsumerManager {

        private ITwichirpApplication application;

        public List<Consumer> Consumer { get; private set; } = new List<Consumer>();
        public Consumer DefaultConsumer { get; private set; }

        public ConsumerManager(ITwichirpApplication application,Consumer defaultConsumer) {
            this.application = application;
            application.DatabaseManager.ConsumerConnection.CreateTableAsync<Consumer>().Wait();
            DefaultConsumer = defaultConsumer;
        }

        public async Task InitAsync() {
            Consumer = await application.DatabaseManager.ConsumerConnection.Table<Consumer>().ToListAsync();
        }

        public async Task AddAcync(Consumer consumer) {
            await application.DatabaseManager.ConsumerConnection.InsertOrReplaceAsync(consumer);
            if(Consumer.Any(x => x.ConsumerKey == consumer.ConsumerKey)) {
                Consumer.Add(consumer);
            }
        }

        public async Task RemoveAsync(Consumer consumer) {
            await application.DatabaseManager.ConsumerConnection.DeleteAsync(consumer);
            Consumer.Remove(Consumer.First(x => x.ConsumerKey == consumer.ConsumerKey));
        }

        public async Task ImportJsonAsync(string json) {
            List<Consumer> consumer = JsonConvert.DeserializeObject<List<Consumer>>(json);
            if(consumer.Count == 0) {
                return;
            }
            if(consumer.Any(x => x.IsValid == false)) {
                throw new ArgumentException("Invalid Consumer Object");
            }
            await application.DatabaseManager.ConsumerConnection.DeleteAllAsync<Consumer>();
            await application.DatabaseManager.ConsumerConnection.InsertAllAsync(consumer);
            Consumer = consumer;
        }

        public string ExportJson() => JsonConvert.SerializeObject(Consumer);
    }
}
