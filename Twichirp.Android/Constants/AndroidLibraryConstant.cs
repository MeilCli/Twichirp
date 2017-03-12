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
using Twichirp.Core.Constants;
using Twichirp.Core.Objects;

namespace Twichirp.Android.Constants {

    public class AndroidLibraryConstant {

        public static readonly Library AndroidPageLayout = new Library("AndroidPageLayout", "Copyright (c) 2017 meil", LicenseConstant.MITLicense);
        public static readonly Library AndroidSlideLayout = new Library("AndroidSlideLayout", "Copyright (c) 2017 meil", LicenseConstant.MITLicense);
        public static readonly Library SupportLibrary = new Library("Xamarin Android Support Library", "Copyright (c) .NET Foundation Contributors", LicenseConstant.MITLicense);
        public static readonly Library FFImageLoading = new Library("FFImageLoading", "Copyright (c) 2015 Fabien Molinet", LicenseConstant.MITLicense);
        public static readonly Library MaterialDesignIcon = new Library("Material design icons", "Copyright (c) Google", LicenseConstant.ApacheLicenseV2);
        public static readonly Library BottomBar = new Library("BottomBarSharp", "Copyright (c) 2016 meil", LicenseConstant.MITLicense);

        public static readonly Library[] Libraries = { AndroidPageLayout, AndroidSlideLayout, SupportLibrary, FFImageLoading, MaterialDesignIcon, BottomBar };
    }
}