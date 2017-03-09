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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Twichirp.Core.App.Service;
using Twichirp.Core.App.Setting;
using Twichirp.Core.App.ViewModel;
using Twichirp.Core.DataObjects;
using Twichirp.Core.DataRepositories;

namespace Twichirp.Core.UnityExtensions {

    public class ViewModelRegister : UnityContainerExtension, IRegister {

        public Type[] DependedTypes { get; } = {
            typeof(ITwitterEventService),typeof(IAccountRepository),typeof(SettingManager),typeof(ClientKey)
        };

        public Type[] ExcludeTypes { get; set; }

        public Type[] ManagedTypes { get; } = {
            typeof(LoginViewModel),typeof(SplashViewModel),typeof(StatusTimelineViewModel)
        };

        public ViewModelRegister() { }

        protected override void Initialize() {
            if(ExcludeTypes?.All(x => x != typeof(LoginViewModel)) ?? true) {
                Container.RegisterType<LoginViewModel>();
            }
            if(ExcludeTypes?.All(x => x != typeof(SplashViewModel)) ?? true) {
                Container.RegisterType<SplashViewModel>();
            }
            if(ExcludeTypes?.All(x => x != typeof(StatusTimelineViewModel)) ?? true) {
                Container.RegisterType<StatusTimelineViewModel>();
            }
        }
    }
}
