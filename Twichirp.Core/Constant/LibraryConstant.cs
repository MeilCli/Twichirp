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
using Twichirp.Core.Model;

namespace Twichirp.Core.Constant {
    public class LibraryConstant {
        public static readonly Library CoreTweet = new Library("CoreTweet",@"CoreTweet - A .NET Twitter Library supporting Twitter API 1.1
Copyright (c) 2013-2016 CoreTweet Development Team",LicenseConstant.MITLicense);
        public static readonly Library CoreTweetSupplement = new Library("CoreTweetSupplement","Copyright (c) 2014 azyobuzin",LicenseConstant.MITLicense);
        public static readonly Library NewtonsoftJson = new Library("Newtonsoft.Json","Copyright (c) 2007 James Newton-King",LicenseConstant.MITLicense);
        public static readonly Library ReactiveProperty = new Library("ReactiveProperty","Copyright (c) 2016 neuecc, xin9le, okazuki",LicenseConstant.MITLicense);
        public static readonly Library SettingsPlugin = new Library("Settings Plugin for Xamarin And Windows","Copyright (c) 2016 James Montemagno / Refractored LLC",LicenseConstant.MITLicense);
        public static readonly Library SQLiteNetPCL = new Library("SQLite.Net-PCL","Copyright (c) 2012 Krueger Systems, Inc.\nCopyright (c) 2013 Øystein Krog (oystein.krog@gmail.com)",LicenseConstant.MITLicense);

        public static readonly Library[] Libraries = { CoreTweet,CoreTweetSupplement,NewtonsoftJson,ReactiveProperty,SettingsPlugin,SQLiteNetPCL };
    }
}
