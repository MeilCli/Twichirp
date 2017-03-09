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

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Practices.Unity;
using Twichirp.Android.Services;
using Twichirp.Core.Services;
using Twichirp.Core.UnityExtensions;

namespace Twichirp.Android.UnityExtensions {

    public class AndroidServiceRegister : UnityContainerExtension, IRegister {

        public Type[] DependedTypes { get; } = { };

        public Type[] ExcludeTypes { get; set; }

        public Type[] ManagedTypes { get; } = {
            typeof(IFileService)
        };

        public AndroidServiceRegister() { }

        protected override void Initialize() {
            if(ExcludeTypes?.All(x => x != typeof(IFileService)) ?? true) {
                Container.RegisterType<IFileService,FileService>(new ContainerControlledLifetimeManager());
            }
        }
    }
}