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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.Model {
    public class Library {
        public string LibraryName { get; private set; }
        public string Copyright { get; private set; }
        public License LibraryLicense { get; private set; }

        public Library(string libraryName,string copyright,License libraryLicense) {
            LibraryName = libraryName;
            Copyright = copyright;
            LibraryLicense = libraryLicense;
        }

        public string ToNoticeText() {
            return $"{Copyright}\n\n{LibraryLicense.LicenseText}";
        }
    }
}
