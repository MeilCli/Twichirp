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
using System.IO;
using System.Text;
using Twichirp.Core.Services;

namespace Twichirp.Android.Services {

    public class FileService : IFileService {

        public string GetPersonalFolderPath() => System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public string ReadFile(string fileName) => File.ReadAllText(fileName, Encoding.UTF8);

        public void WriteFile(string fileName, string text) => File.WriteAllText(fileName, text, Encoding.UTF8);

        public string[] GetDirectories(string path) => Directory.GetDirectories(path);

        public string[] GetFiles(string path) => Directory.GetFiles(path);

        public void CreateDirectory(string path) => Directory.CreateDirectory(path);

        public bool Exists(string path) => Directory.Exists(path);
    }
}