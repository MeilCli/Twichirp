﻿// Copyright (c) 2016 meil
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.App.Model {
    class SplashModel : BaseModel {

        public event EventHandler<EventArgs> ApplicationInitialized;

        private bool _isRunning;
        public bool IsRunning {
            get {
                return _isRunning;
            }
            private set {
                _isRunning = value;
                RaisePropertyChanged(nameof(IsRunning));
            }
        }

        private bool _isAccountExist;
        public bool IsAccountExist {
            get {
                return _isAccountExist;
            }
            private set {
                _isAccountExist = value;
                RaisePropertyChanged(nameof(IsAccountExist));
            }
        }

        private string _message = string.Empty;
        public string Message {
            get {
                return _message;
            }
            private set {
                _message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }

        public SplashModel(ITwichirpApplication application) : base(application) {
        }

        public async void ApplicationInit() {
            IsRunning = true;
            Message = Application.GetLocalizedString(Application.StringResource.SplashAccountLoading);
            await Application.AccountManager.InitAsync();
            Message = Application.GetLocalizedString(Application.StringResource.SplashConsumerLoading);
            await Application.ConsumerManager.InitAsync();
            Message = Application.GetLocalizedString(Application.StringResource.SplashAccountDownLoading);
            try {
                await accountInitAsync();
            } catch(Exception) { }
            Message = string.Empty;
            IsAccountExist = Application.AccountManager.Account.Count > 0;
            IsRunning = false;
            ApplicationInitialized?.Invoke(this,new EventArgs());
        }

        private async Task accountInitAsync() {
            foreach(var account in Application.AccountManager.Account) {
                account.User = (await Application.UserContainerManager.FindAsync(account.Id))?.User;
                if(account.User != null && Application.SettingManager.Applications.IsCleanLaunch == false) {
                    continue;
                }
                var user = await account.Token.Users.ShowAsync(account.Id);
                await Application.UserContainerManager.AddAsync(user);
                account.User = user;
                if(account.ScreenName != user.ScreenName) {
                    account.ScreenName = user.ScreenName;
                    await Application.AccountManager.AddAsync(account);
                }
            }
        }
    }
}
