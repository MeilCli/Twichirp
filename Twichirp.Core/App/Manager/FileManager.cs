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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.App.Manager {
    public class FileManager {

        private IFileSystem fileSystem;

        public string AccountDatabasePath => Path.Combine(fileSystem.GetPersonalFolderPath(),"Account.db");

        public string ConsumerDatabasePath => Path.Combine(fileSystem.GetPersonalFolderPath(),"Consumer.db");

        public string UserContainerDatabasePath => Path.Combine(fileSystem.GetPersonalFolderPath(),"UserContainer.db");

        public string BaseStatusContainerDatabasePath => Path.Combine(fileSystem.GetPersonalFolderPath(),"StatusContainer");

        public FileManager(IFileSystem fileSystem) {
            this.fileSystem = fileSystem;
        }
       
    }
}
