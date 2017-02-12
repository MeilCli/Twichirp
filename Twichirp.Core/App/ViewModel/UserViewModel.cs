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
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Core.App.Model;
using Twichirp.Core.Model;

namespace Twichirp.Core.App.ViewModel {

    public class UserViewModel : BaseViewModel {

        public Account Account { get; }
        private UserModel userModel;

        public ReactiveProperty<string> IconUrl { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ScreenName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> IsProtected { get; private set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsVerified { get; private set; } = new ReactiveProperty<bool>();

        public UserViewModel(ITwichirpApplication application,User user,Account account) : this(application,new UserModel(application,user),account) { }

        public UserViewModel(ITwichirpApplication application,UserModel userModel,Account account) : base(application) {
            Account = account;
            this.userModel = userModel;

            initStaticValue();
            Observable.FromEventPattern<EventArgs>(x => userModel.UserChanged += x,x => userModel.UserChanged -= x)
                .Subscribe(x => initStaticValue())
                .AddTo(Disposable);
        }

        private void initStaticValue() {
            IconUrl.Value = userModel.ProfileImageUrl.Replace("_normal","_bigger");
            Name.Value = userModel.Name;
            ScreenName.Value = $"@{userModel.ScreenName}";
            IsProtected.Value = userModel.IsProtected;
            IsVerified.Value = userModel.IsVerified;
        }
    }
}
