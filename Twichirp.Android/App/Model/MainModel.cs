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
using Twichirp.Android.Objects;
using Twichirp.Core.App;
using Twichirp.Core.App.Model;
using Twichirp.Core.DataObjects;
using Twichirp.Core.DataRepositories;
using Twichirp.Core.Resources;
using Twichirp.Core.Settings;
using CUser = CoreTweet.User;

namespace Twichirp.Android.App.Model {

    public class MainModel : BaseModel {

        public const int NavigationMenuStandardFirstGroup = 1;
        public const int NavigationMenuStandardSecondGroup = 2;
        public const int NavigationMenuHidingFirstGroup = 3;
        public const int NavigationMenuHidingSecondGroup = 4;

        public const int NavigationMenuAddAccount = Resource.Id.NavigationMenuAddAccount;
        public const int NavigationMenuSetting = Resource.Id.NavigationMenuSetting;

        private IAccountRepository accountRepository;
        private SettingManager settingManager;

        private List<NavigationMenu> _navigationMenus;
        public List<NavigationMenu> NavigationMenus {
            get {
                return _navigationMenus;
            }
            private set {
                SetValue(ref _navigationMenus,value,nameof(NavigationMenus));
            }
        }

        private List<NavigationTab> _navigationTabs;
        public List<NavigationTab> NavigationTabs {
            get {
                return _navigationTabs;
            }
            private set {
                SetValue(ref _navigationTabs,value,nameof(NavigationTabs));
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


        public MainModel(ITwichirpApplication application,IAccountRepository accountRepository,SettingManager settingManager) : base(application) {
            this.accountRepository = accountRepository;
            this.settingManager = settingManager;
            makeNavigationMenu();
            makeNavigationTab();
            setUsers();
        }

        public void NavigationMenuGroupReverse() {
            IsHiding = IsHiding == false;
        }

        public void NotifyUserUpdate(ImmutableAccount account,CUser user) {
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

        public void UpdateDefaultAccountIfChanged() {
            long defaultAccountId = settingManager.Accounts.DefaultAccountId;
            if(User.Id != defaultAccountId) {
                SetDefaultUser(defaultAccountId);
            }
        }

        public void SetDefaultUser(string screenName) {
            var account = accountRepository[screenName];
            if(account == null) {
                return;
            }
            SetDefaultUser(account);
        }

        public void SetDefaultUser(long userId) {
            var account = accountRepository[userId];
            if(account == null) {
                return;
            }
            SetDefaultUser(account);
        }

        public void SetDefaultUser(ImmutableAccount account) {
            settingManager.Accounts.DefaultAccountId = account.Id;
            var orderdUserList = settingManager.Accounts.AccountUsedOrder;
            if(orderdUserList.Contains(account.Id)) {
                orderdUserList.Remove(account.Id);
            }
            orderdUserList.Insert(0,account.Id);

            IsHiding = false;
            makeNavigationMenu();
            makeNavigationTab();
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

        private async Task<UserModel> toUserModel(ImmutableAccount account) {
            if(account.User == null) {
                try {
                    var coreTweetUser = await account.CoreTweetToken.Users.ShowAsync(account.Id);
                    var mutableAccount = new Account(new User(coreTweetUser),new Token(account.Token));
                    await Task.Run(() => accountRepository.AddOrUpdate(mutableAccount));
                } catch(Exception) {
                    return null;
                }
                account = await Task.Run(() => accountRepository.Find(account.Id));
            }
            return new UserModel(Application,account.User.CoreTweetUser);
        }

        private void makeNavigationMenu() {
            var nowAccount = accountRepository[settingManager.Accounts.DefaultAccountId];

            var list = new List<NavigationMenu>();
            if(IsHiding) {
                foreach(var account in getOrderdAccount()) {
                    string title = account.ScreenName;
                    var menu = new NavigationMenu(NavigationMenuHidingFirstGroup,title.GetHashCode(),title,Resource.Drawable.IconPersonGrey48dp);
                    if(account.Id == nowAccount.Id) {
                        menu.IsChecked = true;
                    }
                    list.Add(menu);
                }
                {
                    list.Add(new NavigationMenu(NavigationMenuHidingSecondGroup,NavigationMenuAddAccount,Resource.String.MainAddAccount,Resource.Drawable.IconPersonAddGrey48dp));
                }
            } else {
                {
                    int id = 0;
                    list.Add(new NavigationMenu(NavigationMenuStandardFirstGroup,id++,$"@{nowAccount.ScreenName}",Resource.Drawable.IconPersonOutlineGrey48dp));
                }
                {
                    list.Add(new NavigationMenu(NavigationMenuStandardSecondGroup,NavigationMenuSetting,Resource.String.Setting,Resource.Drawable.IconSettingsApplicationsGrey48dp));
                }
            }
            NavigationMenus = list;
        }

        private void makeNavigationTab() {
            var list = new List<NavigationTab>();
            {
                list.Add(new NavigationTab(Resource.Id.TabHome,StringResources.TabHome,Resource.Drawable.IconHomeGrey24dp));
                list.Add(new NavigationTab(Resource.Id.TabMention,StringResources.TabMention,Resource.Drawable.IconNotificationsGrey24dp));
                list.Add(new NavigationTab(Resource.Id.TabDM,StringResources.TabDM,Resource.Drawable.IconMailGrey24dp));
                list.Add(new NavigationTab(Resource.Id.TabUser,StringResources.TabUser,Resource.Drawable.IconPersonGrey24dp));
            }
            NavigationTabs = list;
        }

        private List<ImmutableAccount> getOrderdAccount() {
            var result = new List<ImmutableAccount>();
            Action<ImmutableAccount> addAccount = x => {
                if(result.All(y => y.Id != x.Id)) {
                    result.Add(x);
                }
            };

            addAccount(accountRepository[settingManager.Accounts.DefaultAccountId]);
            {
                var notContainUserId = new List<long>();
                foreach(var orderdUserId in settingManager.Accounts.AccountUsedOrder) {
                    var account = accountRepository[orderdUserId];
                    if(account == null) {
                        notContainUserId.Add(orderdUserId);
                        continue;
                    }
                    addAccount(account);
                }
                foreach(var userId in notContainUserId) {
                    settingManager.Accounts.AccountUsedOrder.Remove(userId);
                }
            }
            foreach(var a in accountRepository.Get()) {
                addAccount(a);
            }
            return result;
        }

    }
}