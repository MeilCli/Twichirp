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
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Twichirp.Core.DataRepositories;
using Twichirp.Core.Services;

namespace Twichirp.Core.UnityExtensions {

    public sealed class DataRepositoryRegister : UnityContainerExtension, IRegister {

        public Type[] ManagedTypes { get; } = {
            typeof(IAccountRepository),typeof(IClientKeyRepository),typeof(IListCollectionCacheRepository),typeof(IListRepository),
            typeof(IStatusRepository),typeof(IStatusTimelineCacheRepository),typeof(ITokenRepository),typeof(IUserRepository),
            typeof(IDirectMessageRepository)
        };

        public Type[] ExcludeTypes { get; set; }

        public Type[] DependedTypes { get; } = {
            typeof(IRealmService)
        };

        public DataRepositoryRegister() { }

        protected override void Initialize() {
            if(ExcludeTypes?.All(x => x != typeof(IAccountRepository)) ?? true) {
                Container.RegisterType<IAccountRepository,AccountRepository>(new ContainerControlledLifetimeManager());
            }
            if(ExcludeTypes?.All(x => x != typeof(IClientKeyRepository)) ?? true) {
                Container.RegisterType<IClientKeyRepository,ClientKeyRepository>(new ContainerControlledLifetimeManager());
            }
            if(ExcludeTypes?.All(x => x != typeof(IListCollectionCacheRepository)) ?? true) {
                Container.RegisterType<IListCollectionCacheRepository,ListCollectionCacheRepository>(new ContainerControlledLifetimeManager());
            }
            if(ExcludeTypes?.All(x => x != typeof(IListRepository)) ?? true) {
                Container.RegisterType<IListRepository,ListRepository>(new ContainerControlledLifetimeManager());
            }
            if(ExcludeTypes?.All(x => x != typeof(IStatusRepository)) ?? true) {
                Container.RegisterType<IStatusRepository,StatusRepository>(new ContainerControlledLifetimeManager());
            }
            if(ExcludeTypes?.All(x => x != typeof(IStatusTimelineCacheRepository)) ?? true) {
                Container.RegisterType<IStatusTimelineCacheRepository,StatusTimelineCacheRepository>(new ContainerControlledLifetimeManager());
            }
            if(ExcludeTypes?.All(x => x != typeof(ITokenRepository)) ?? true) {
                Container.RegisterType<ITokenRepository,TokenRepository>(new ContainerControlledLifetimeManager());
            }
            if(ExcludeTypes?.All(x => x != typeof(IUserRepository)) ?? true) {
                Container.RegisterType<IUserRepository,UserRepository>(new ContainerControlledLifetimeManager());
            }
            if(ExcludeTypes?.All(x => x != typeof(IDirectMessageRepository)) ?? true) {
                Container.RegisterType<IDirectMessageRepository,DirectMessageRepository>(new ContainerControlledLifetimeManager());
            }
        }
    }
}
