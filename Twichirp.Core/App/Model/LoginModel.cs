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
using CoreTweet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.Model;
using Twichirp.Core.App.Event;

namespace Twichirp.Core.App.Model {
    class LoginModel : BaseModel {

        public event EventHandler<EventArgs<string>> AuthorizeUriCreated;
        public event EventHandler<EventArgs> LoginSucceeded;
        public event EventHandler<EventArgs<string>> ErrorMessageCreated;

        private OAuth.OAuthSession oAuthSession;
        private Consumer consumer;

        private bool _isLoding;
        public bool IsLoading {
            get {
                return _isLoding;
            }
            private set {
                SetValue(ref _isLoding,value,nameof(IsLoading));
            }
        }

        public LoginModel(ITwichirpApplication application) : base(application) {
        }

        public async void Authorize(Consumer consumer) {
            this.consumer = consumer;
            try {
                oAuthSession = await OAuth.AuthorizeAsync(consumer.ConsumerKey,consumer.ConsumerSecret);
                AuthorizeUriCreated?.Invoke(this,new EventArgs<string>(oAuthSession.AuthorizeUri.AbsoluteUri));
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            }
        }

        public async void Login(string pin) {
            if(consumer == null) {
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
                Tokens token = await oAuthSession.GetTokensAsync(pin);
                var account = new Account(token,consumer);
                account.User = await token.Users.ShowAsync(token.UserId);
                await Application.AccountManager.AddAsync(account);
                await Application.UserContainerManager.AddAsync(account.User);
                LoginSucceeded?.Invoke(this,new EventArgs());
            } catch(Exception e) {
                ErrorMessageCreated?.Invoke(this,new EventArgs<string>(e.Message));
            }
            IsLoading = false;
        }

    }
}
