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
using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Twichirp.Core.App.Manager {
    public class DatabaseManager : IDisposable {

        private IDatabaseSystem databaseSystem;

        public SQLiteAsyncConnection AccountConnection { get; }

        public SQLiteAsyncConnection ConsumerConnection { get; }

        public SQLiteAsyncConnection UserContainerConnection { get; }

        public StatusContainer StatusContainerConnections { get; }

        public DatabaseManager(ITwichirpApplication appilication,IDatabaseSystem databaseSystem) {
            this.databaseSystem = databaseSystem;
            AccountConnection = databaseSystem.GetSQLiteAsyncConnection(appilication.FileManager.AccountDatabasePath);
            ConsumerConnection = databaseSystem.GetSQLiteAsyncConnection(appilication.FileManager.ConsumerDatabasePath);
            UserContainerConnection = databaseSystem.GetSQLiteAsyncConnection(appilication.FileManager.UserContainerDatabasePath);
            StatusContainerConnections = new StatusContainer(appilication,databaseSystem);
        }

        public void Dispose() {
            foreach(var d in databaseSystem.CashConnectionList) {
                d.Dispose();
            }
        }
    }

    public class StatusContainer {

        private Dictionary<long,SQLiteAsyncConnection> connections = new Dictionary<long,SQLiteAsyncConnection>();
        private ITwichirpApplication application;
        private IDatabaseSystem databaseSystem;

        internal StatusContainer(ITwichirpApplication application,IDatabaseSystem databaseSystem) {
            this.application = application;
            this.databaseSystem = databaseSystem;
        }

        public SQLiteAsyncConnection this[long id] {
            get {
                if(connections.ContainsKey(id)) {
                    return connections[id];
                }
                var connection = databaseSystem.GetSQLiteAsyncConnection(Path.Combine(application.FileManager.BaseStatusContainerDatabasePath,$"{id}.db"));
                connections[id] = connection;
                return connection;
            }
        }

    }
}
