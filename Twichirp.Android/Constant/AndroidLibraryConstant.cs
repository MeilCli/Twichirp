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
using Twichirp.Core.Model;
using Twichirp.Core.Constant;

namespace Twichirp.Android.Constant {
    public class AndroidLibraryConstant {
        public static readonly Library SupportLibrary = new Library("Xamarin Android Support Library","Copyright (c) .NET Foundation Contributors",LicenseConstant.MITLicense);
        public static readonly Library SquareBindings = new Library("square-bindings","Copyright (c) mattleibow",LicenseConstant.ApacheLicenseV2);
        public static readonly Library MaterialDesignIcon = new Library("Material design icons","Copyright (c) Google",LicenseConstant.ApacheLicenseV2);

        public static readonly Library[] Libraries = { SupportLibrary,SquareBindings,MaterialDesignIcon };
    }
}