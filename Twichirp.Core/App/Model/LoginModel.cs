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

namespace Twichirp.Core.App.Model {
    class LoginModel : BaseModel {

        private OAuth.OAuthSession oAuthSession;
        private Consumer consumer;

        private string _authorizeUri;
        public string AuthorizeUri {
            get {
                return _authorizeUri;
            }
            private set {
                _authorizeUri = value;
                RaisePropertyChanged(nameof(AuthorizeUri));
            }
        }

        private bool _isLoding;
        public bool IsLoading {
            get {
                return _isLoding;
            }
            private set {
                _isLoding = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        private bool _isLoginFinish;
        public bool IsLoginFinish {
            get {
                return _isLoginFinish;
            }
            private set {
                _isLoginFinish = value;
                RaisePropertyChanged(nameof(IsLoginFinish));
            }
        }

        private string _errorMessage;
        public string ErrorMessage {
            get {
                return _errorMessage;
            }
            private set {
                _errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }

        public LoginModel(ITwichirpApplication application) : base(application) {
        }

        public async void Authorize(Consumer consumer) {
            this.consumer = consumer;
            ErrorMessage = null;
            AuthorizeUri = null;
            try {
                oAuthSession = await OAuth.AuthorizeAsync(consumer.ConsumerKey,consumer.ConsumerSecret);
                AuthorizeUri = oAuthSession.AuthorizeUri.AbsoluteUri;
            } catch(Exception e) {
                ErrorMessage = e.Message;
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
            ErrorMessage = null;
            try {
                Tokens token = await oAuthSession.GetTokensAsync(pin);
                var account = new Account(token,consumer);
                account.User = await token.Users.ShowAsync(token.UserId);
                await Application.AccountManager.AddAsync(account);
                await Application.UserContainerManager.AddAsync(account.User);
                IsLoginFinish = true;
            }catch(Exception e) {
                ErrorMessage = e.Message;
            }
            IsLoading = false;
        }
        
    }
}
