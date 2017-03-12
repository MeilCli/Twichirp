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
using System.Threading.Tasks;
using Twichirp.Core.DataObjects;
using Twichirp.Core.DataRepositories;
using Twichirp.Core.Resources;
using Twichirp.Core.Settings;

namespace Twichirp.Core.Models {

    class SplashModel : BaseModel {

        public event EventHandler<EventArgs> ApplicationInitialized;

        private IAccountRepository accountRepository;
        private SettingManager settingManager;

        private bool _isRunning;
        public bool IsRunning {
            get => _isRunning;
            private set => SetValue(ref _isRunning, value, nameof(IsRunning));
        }

        private bool _isAccountExist;
        public bool IsAccountExist {
            get => _isAccountExist;
            private set => SetValue(ref _isAccountExist, value, nameof(IsAccountExist));
        }

        private string _message = string.Empty;
        public string Message {
            get => _message;
            private set => SetValue(ref _message, value, nameof(Message));
        }

        public SplashModel(IAccountRepository accountRepository, SettingManager settingManager) {
            this.accountRepository = accountRepository;
            this.settingManager = settingManager;
        }

        public async Task ApplicationInitAsync() {
            IsRunning = true;
            Message = StringResources.SplashAccountDownLoading;
            try {
                await accountInitAsync();
            } catch (Exception) { }
            if (accountRepository[settingManager.Accounts.DefaultAccountId] == null) {
                settingManager.Accounts.DefaultAccountId = accountRepository.FirstOrDefault()?.Id ?? -1;
            }
            Message = string.Empty;
            IsAccountExist = accountRepository.Count() > 0;
            IsRunning = false;
            ApplicationInitialized?.Invoke(this, new EventArgs());
        }

        private async Task accountInitAsync() {
            var accounts = await Task.Run(() => accountRepository.Get());
            foreach (var account in accounts) {
                if (account.User != null && settingManager.Applications.IsCleanLaunch == false) {
                    continue;
                }
                var coreTweetUser = await account.CoreTweetToken.Users.ShowAsync(account.Id);
                var mutableAccount = new Account(new User(coreTweetUser), new Token(account.Token));
                await Task.Run(() => accountRepository.AddOrUpdate(mutableAccount));
            }
        }
    }
}
