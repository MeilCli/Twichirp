﻿// Copyright (c) 2016-2017 meil
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
using Twichirp.Core.Objects;

namespace Twichirp.Core.Constants {

    public class LibraryConstant {
        public static readonly Library CoreTweet = new Library("CoreTweet", @"CoreTweet - A .NET Twitter Library supporting Twitter API 1.1
Copyright (c) 2013-2016 CoreTweet Development Team", LicenseConstant.MITLicense);
        public static readonly Library CoreTweetSupplement = new Library("CoreTweetSupplement", "Copyright (c) 2014 azyobuzin", LicenseConstant.MITLicense);
        public static readonly Library CrossFormattedText = new Library("CrossFormattedText", "Copyright (c) 2016 meil", LicenseConstant.MITLicense);
        public static readonly Library NewtonsoftJson = new Library("Newtonsoft.Json", "Copyright (c) 2007 James Newton-King", LicenseConstant.MITLicense);
        public static readonly Library ReactiveProperty = new Library("ReactiveProperty", "Copyright (c) 2016 neuecc, xin9le, okazuki", LicenseConstant.MITLicense);
        public static readonly Library Realm = new Library("Realm", "Copyright 2016 Realm Inc", LicenseConstant.ApacheLicenseV2);
        public static readonly Library SettingsPlugin = new Library("Settings Plugin for Xamarin And Windows", "Copyright (c) 2016 James Montemagno / Refractored LLC", LicenseConstant.MITLicense);
        public static readonly Library Unity = new Library("Unity", "Copyright (c) Microsoft.  All rights reserved.", LicenseConstant.ApacheLicenseV2);

        public static readonly Library[] Libraries = { CoreTweet, CoreTweetSupplement, NewtonsoftJson, ReactiveProperty, Realm, SettingsPlugin, Unity };
    }
}
