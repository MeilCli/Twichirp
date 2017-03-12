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
using Twichirp.Core.DataObjects;
using Twichirp.Core.DataRepositories;
using CoreTweet;
using DUser = Twichirp.Core.DataObjects.User;
using Twichirp.Core.Events;
using Twichirp.Core.Settings;

namespace Twichirp.Core.App.Model {

    class LoginModel : BaseModel {

        public event EventHandler<EventArgs<string>> AuthorizeUriCreated;
        public event EventHandler<EventArgs> LoginSucceeded;
        public event EventHandler<EventArgs<string>> ErrorMessageCreated;

        private OAuth.OAuthSession oAuthSession;
        private ImmutableClientKey clientKey;
        private IAccountRepository accountRepository;
        private SettingManager settingManager;

        private bool _isLoding;
        public bool IsLoading {
            get {
                return _isLoding;
            }
            private set {
                SetValue(ref _isLoding,value,nameof(IsLoading));
            }
        }

        public LoginModel(IAccountRepository accountRepository,SettingManager settingManager) {
            this.accountRepository = accountRepository;
            this.settingManager = settingManager;
        }

        public async Task AuthorizeAsync(ImmutableClientKey clientKey) {
            this.clientKey = clientKey;
            try {
                oAuthSession = await OAuth.AuthorizeAsync(clientKey.ConsumerKey,clientKey.ConsumerSecret);
                AuthorizeUriCreated?.Invoke(this,new EventArgs<string>(oAuthSession.AuthorizeUri.AbsoluteUri));
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            }
        }

        public async Task LoginAsync(string pin) {
            if(clientKey == null) {
                return;
            }
            if(oAuthSession == null) {
                return;
            }
            if(pin == null) {
                return;
            }
            IsLoading = true;
            try {
                var coreTweetToken = await oAuthSession.GetTokensAsync(pin);
                var token = new Token(new ClientKey(clientKey),coreTweetToken.AccessToken,coreTweetToken.AccessTokenSecret);
                var coreTweetUser = await coreTweetToken.Account.VerifyCredentialsAsync(skip_status: true,include_entities: true);
                var user = new DUser(coreTweetUser);
                var account = new Account(user,token);
                await Task.Run(() => accountRepository.AddOrUpdate(account));
                settingManager.Accounts.DefaultAccountId = coreTweetToken.UserId;
                LoginSucceeded?.Invoke(this,new EventArgs());
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            }
            IsLoading = false;
        }

    }
}
