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

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.Model;
using Twichirp.Android.Model;
using Twichirp.Core.App;
using Twichirp.Core.App.Model;
using Twichirp.Core.App.ViewModel;

namespace Twichirp.Android.App.ViewModel {
    public class MainViewModel : BaseViewModel {

        private MainModel mainModel;

        public ReadOnlyReactiveProperty<List<NavigationMenu>> NavigationMenus { get; }
        public ReadOnlyReactiveProperty<bool> IsNavigationHidingGroup { get; }
        public ReactiveCommand NavigationMenuGroupReverseCommand { get; } = new ReactiveCommand();
        public ReactiveCommand<Tuple<int,int>> NavigationMenuSelectedCommand { get; } = new ReactiveCommand<Tuple<int,int>>();

        public ReactiveProperty<string> UserIcon { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> UserBanner { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> UserLinkColor { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> UserName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> UserScreenName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> FirsrSubUserIcon { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> SecondSubUserIcon { get; } = new ReactiveProperty<string>();
        public ReactiveCommand FirstSubUserIconClickedCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SecondSubUserIconClickedCommand { get; } = new ReactiveCommand();

        public ReactiveCommand StartSettingActivityCommand { get; } = new ReactiveCommand();
        public ReactiveCommand StartLoginActivityCommand { get; } = new ReactiveCommand();

        public MainViewModel(ITwichirpApplication application) : base(application) {
            mainModel = new MainModel(application);

            NavigationMenus = mainModel.ObserveProperty(x => x.NavigationMenus).ToReadOnlyReactiveProperty().AddTo(Disposable);
            IsNavigationHidingGroup = mainModel.ObserveProperty(x => x.IsHiding).ToReadOnlyReactiveProperty().AddTo(Disposable);
            NavigationMenuGroupReverseCommand.Subscribe(x => mainModel.NavigationMenuGroupReverse()).AddTo(Disposable);
            NavigationMenuSelectedCommand.Subscribe(x => navigationMenuSelected(x.Item1,x.Item2)).AddTo(Disposable);

            mainModel.ObserveProperty(x => x.User).Subscribe(x => setUserValues(x)).AddTo(Disposable);
            mainModel.ObserveProperty(x => x.FirstSubUser).Subscribe(x => setFirstSubUserValue(x)).AddTo(Disposable);
            mainModel.ObserveProperty(x => x.SecondSubUser).Subscribe(x => setSecondSubUserValue(x)).AddTo(Disposable);
            FirstSubUserIconClickedCommand.Subscribe(x => {
                if(mainModel.FirstSubUser != null) {
                    mainModel.SetDefaultUser(mainModel.FirstSubUser.ScreenName);
                }
            }).AddTo(Disposable);
            SecondSubUserIconClickedCommand.Subscribe(x => {
                if(mainModel.SecondSubUser != null) {
                    mainModel.SetDefaultUser(mainModel.SecondSubUser.ScreenName);
                }
            }).AddTo(Disposable);

            Observable.FromEventPattern<UserEventArgs>(x => application.TwitterEvent.UserUpdated += x,x => application.TwitterEvent.UserUpdated -= x)
                .Subscribe(x => mainModel.NotifyUserUpdate(x.EventArgs.Account,x.EventArgs.User))
                .AddTo(Disposable);
        }

        private void navigationMenuSelected(int group,int id) {
            if(group == MainModel.NavigationMenuHidingFirstGroup) {
                var screenName = NavigationMenus.Value.FirstOrDefault(x => x.GroupId == group && x.Id == id)?.Text;
                if(screenName != null) {
                    mainModel.SetDefaultUser(screenName);
                }
                return;
            }
            switch(id) {
                case MainModel.NavigationMenuSetting: {
                        StartSettingActivityCommand.Execute();
                        break;
                    }
                case MainModel.NavigationMenuAddAccount: {
                        StartLoginActivityCommand.Execute();
                        break;
                    }
            }
        }

        private void setUserValues(UserModel user) {
            UserIcon.Value = user.ProfileImageUrl;
            UserBanner.Value = user.ProfileBannerUrl;
            UserLinkColor.Value = user.ProfileLinkColor;
            UserName.Value = user.Name;
            UserScreenName.Value = $"@{user.ScreenName}";
        }

        private void setFirstSubUserValue(UserModel user) {
            FirsrSubUserIcon.Value = user?.ProfileImageUrl;
        }

        private void setSecondSubUserValue(UserModel user) {
            SecondSubUserIcon.Value = user?.ProfileImageUrl;
        }
    }
}