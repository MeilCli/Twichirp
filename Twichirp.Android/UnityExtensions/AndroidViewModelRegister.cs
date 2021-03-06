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
using System.Linq;
using Microsoft.Practices.Unity;
using Twichirp.Android.ViewModels;
using Twichirp.Core.DataRepositories;
using Twichirp.Core.Services;
using Twichirp.Core.Settings;
using Twichirp.Core.UnityExtensions;

namespace Twichirp.Android.UnityExtensions {

    public class AndroidViewModelRegister : UnityContainerExtension, IRegister {

        public Type[] DependedTypes { get; } = {
            typeof(ITwitterEventService),typeof(IAccountRepository),typeof(SettingManager)
        };

        public Type[] ExcludeTypes { get; set; }

        public Type[] ManagedTypes { get; } = {
            typeof(ImageViewerViewModel),typeof(MainViewModel)
        };

        protected override void Initialize() {
            if (ExcludeTypes?.All(x => x != typeof(ImageViewerViewModel)) ?? true) {
                Container.RegisterType<ImageViewerViewModel>();
            }
            if (ExcludeTypes?.All(x => x != typeof(MainViewModel)) ?? true) {
                Container.RegisterType<MainViewModel>();
            }
        }
    }
}