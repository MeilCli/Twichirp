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
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CoreTweet;
using Twichirp.Android.Model;
using Twichirp.Core.App;
using Twichirp.Core.App.Model;
using Twichirp.Core.Model;

namespace Twichirp.Android.App.Model {
    public class MainModel : BaseModel {

        public const int NavigationMenuStandardFirstGroup = 1;
        public const int NavigationMenuStandardSecondGroup = 2;
        public const int NavigationMenuHidingFirstGroup = 3;
        public const int NavigationMenuHidingSecondGroup = 4;

        public const int NavigationMenuAddAccount = Android.Resource.Id.NavigationMenuAddAccount;
        public const int NavigationMenuSetting = Android.Resource.Id.NavigationMenuSetting;

        private List<NavigationMenu> _navigationMenus;
        public List<NavigationMenu> NavigationMenus {
            get {
                return _navigationMenus;
            }
            private set {
                SetValue(ref _navigationMenus,value,nameof(NavigationMenus));
            }
        }

        private bool _isHiding;
        public bool IsHiding {
            get {
                return _isHiding;
            }
            private set {
                SetValue(ref _isHiding,value,nameof(IsHiding));
                makeNavigationMenu();
            }
        }

        private UserModel _user;
        public UserModel User {
            get {
                return _user;
            }
            private set {
                SetValue(ref _user,value,nameof(User));
            }
        }

        private UserModel _firstSubUser;
        public UserModel FirstSubUser {
            get {
                return _firstSubUser;
            }
            private set {
                SetValue(ref _firstSubUser,value,nameof(FirstSubUser));
            }
        }

        private UserModel _secondSubUser;
        public UserModel SecondSubUser {
            get {
                return _secondSubUser;
            }
            private set {
                SetValue(ref _secondSubUser,value,nameof(SecondSubUser));
            }
        }


        public MainModel(ITwichirpApplication application) : base(application) {
            makeNavigationMenu();
            setUsers();
        }

        public void NavigationMenuGroupReverse() {
            IsHiding = IsHiding == false;
        }

        public void NotifyUserUpdate(Account account,User user) {
            // account情報は無視する
            // UserModel内のプロパティ変更イベントを(設計上)捕捉できないのでこちら側のプロパティ変更イベントを発生させる
            if(User.Id == user.Id) {
                User.SetUser(user);
                RaisePropertyChanged(nameof(User));
            }
            if(FirstSubUser?.Id == user.Id) {
                FirstSubUser.SetUser(user);
                RaisePropertyChanged(nameof(FirstSubUser));
            }
            if(SecondSubUser?.Id == user.Id) {
                SecondSubUser.SetUser(user);
                RaisePropertyChanged(nameof(SecondSubUser));
            }
        }

        public void SetDefaultUser(string screenName) {
            var account = Application.AccountManager[screenName];
            if(account == null) {
                return;
            }
            Application.SettingManager.Accounts.DefaultAccountId = account.Id;
            var orderdUserList = Application.SettingManager.Accounts.AccountUsedOrder;
            if(orderdUserList.Contains(account.Id)) {
                orderdUserList.Remove(account.Id);
            }
            orderdUserList.Insert(0,account.Id);

            IsHiding = false;
            makeNavigationMenu();
            setUsers();
        }

        private async void setUsers() {
            var list = getOrderdAccount();
            User = await toUserModel(list[0]);
            if(list.Count >= 2) {
                FirstSubUser = await toUserModel(list[1]);
            } else {
                FirstSubUser = null;
            }
            if(list.Count >= 3) {
                SecondSubUser = await toUserModel(list[2]);
            } else {
                SecondSubUser = null;
            }
        }

        private async Task<UserModel> toUserModel(Account account) {
            if(account.User == null) {
                try {
                    account.User = await account.Token.Users.ShowAsync(account.Id);
                    await Application.UserContainerManager.AddAsync(account.User);
                } catch(Exception) {
                    return null;
                }
            }
            return new UserModel(Application,account.User);
        }

        private void makeNavigationMenu() {
            var accountManager = Application.AccountManager;
            var nowAccount = accountManager[Application.SettingManager.Accounts.DefaultAccountId];

            var list = new List<NavigationMenu>();
            if(IsHiding) {
                foreach(var account in getOrderdAccount()) {
                    string title = account.ScreenName;
                    var menu = new NavigationMenu(NavigationMenuHidingFirstGroup,title.GetHashCode(),title,Android.Resource.Drawable.IconPersonGrey48dp);
                    if(account.Id == nowAccount.Id) {
                        menu.IsChecked = true;
                    }
                    list.Add(menu);
                }
                {
                    list.Add(new NavigationMenu(NavigationMenuHidingSecondGroup,NavigationMenuAddAccount,Android.Resource.String.MainAddAccount,Android.Resource.Drawable.IconPersonAddGrey48dp));
                }
            } else {
                {
                    int id = 0;
                    list.Add(new NavigationMenu(NavigationMenuStandardFirstGroup,id++,$"@{nowAccount.ScreenName}",Android.Resource.Drawable.IconPersonOutlineGrey48dp));
                }
                {
                    list.Add(new NavigationMenu(NavigationMenuStandardSecondGroup,NavigationMenuSetting,Android.Resource.String.Setting,Android.Resource.Drawable.IconSettingsApplicationsGrey48dp));
                }
            }
            NavigationMenus = list;
        }

        private List<Account> getOrderdAccount() {
            var result = new List<Account>();
            Action<Account> addAccount = x => {
                if(result.Contains(x) == false) {
                    result.Add(x);
                }
            };

            addAccount(Application.AccountManager[Application.SettingManager.Accounts.DefaultAccountId]);
            {
                var notContainUserId = new List<long>();
                foreach(var orderdUserId in Application.SettingManager.Accounts.AccountUsedOrder) {
                    var account = Application.AccountManager[orderdUserId];
                    if(account == null) {
                        notContainUserId.Add(orderdUserId);
                        continue;
                    }
                    addAccount(account);
                }
                foreach(var userId in notContainUserId) {
                    Application.SettingManager.Accounts.AccountUsedOrder.Remove(userId);
                }
            }
            foreach(var a in Application.AccountManager.Account) {
                addAccount(a);
            }
            return result;
        }

    }
}