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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Twichirp.Android.App.View;
using static Android.Support.Design.Widget.NavigationView;
using Square.Picasso;
using Android.Graphics;
using Twichirp.Core.Model;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Twichirp.Android.App.Extensions;
using Android.Support.V7.Graphics;
using Android.Support.V4.Graphics.Drawable;
using Android.Support.V4.Content;
using Twichirp.Android.App.View.Activity;
using Twichirp.Android.App.Setting;
using CoreTweet;
using Android.Util;

namespace Twichirp.Android.App.ViewController {
    public class MainViewController : BaseViewController<IMainView> {

        private bool isShowAccounts = false;
        private string addAccountText;
        private string settingText;
        //private BottomBar bottomBar;

        public MainViewController(IMainView view) : base(view) {
            view.OnCreateEventHandler += onCreate;
            view.OnResumeEventHandler += onResume;
            view.OnSaveInstanceStateEventHandler += onSaveState;
            addAccountText = view.ApplicationContext.GetString(Resource.String.MainAddAccount);
            settingText = view.ApplicationContext.GetString(Resource.String.Setting);
        }

        private void onCreate(object sender,LifeCycleEventArgs args) {
            View.Navigation.NavigationItemSelected += selectItem;
            View.Subtitle.Click += (s,e)=> {
                isShowAccounts = isShowAccounts == false;
                resetMenu();
            };
            /*
            bottomBar = AppCompatBottomBar.Attach(View.Coordinator,args.State);
            bottomBar.NoTabletGoodness();
            bottomBar.SetOnTabClickListener(this);
            bottomBar.SetItems(new BottomBarTab[] {
                new BottomBarTab(Resource.Drawable.IconHomeGrey24dp,"Home"),
                new BottomBarTab(Resource.Drawable.IconNotificationsGrey24dp,"Mention"),
                new BottomBarTab(Resource.Drawable.IconMailGrey24dp,"DM"),
                new BottomBarTab(Resource.Drawable.IconPersonGrey24dp,"User"),
                new BottomBarTab(Resource.Drawable.IconCommentGrey24dp,"Tweet")
            });
            */
            
        }

        private void onResume(object sender,LifeCycleEventArgs args) {
            resetMenu();
        }

        private void onSaveState(object sender,LifeCycleEventArgs args) {
            //bottomBar?.OnSaveInstanceState(args.State);
        }

        public void OnTabSelected(int position) {
        }

        public void OnTabReSelected(int position) {
        }

        private void resetMenu() {
            var accountManager = Application.AccountManager;
            var nowAccount = accountManager[Application.SettingManager.Accounts.DefaultAccountId];

            setDrop();
            setUser(nowAccount);

            IMenu menu = View.Navigation.Menu;
            menu.Clear();
            if(isShowAccounts) {
                foreach(var account in accountManager.Account) {
                    IMenuItem item = menu.Add(1,account.Id.GetHashCode(),Menu.None,account.ScreenName);
                    if(account.Id == nowAccount.Id) {
                        item.SetChecked(true);
                    }
                    item.SetIcon(Resource.Drawable.IconPersonGrey48dp);
                }
                {
                    IMenuItem item = menu.Add(2,addAccountText.GetHashCode(),Menu.None,addAccountText);
                    item.SetIcon(Resource.Drawable.IconPersonAddGrey48dp);
                }
            } else {
                {
                    int id =1;
                    IMenuItem item = menu.Add(1,id,Menu.None,$"@{nowAccount.ScreenName}");
                    item.SetIcon(Resource.Drawable.IconPersonOutlineGrey48dp);
                }
                {
                    IMenuItem item = menu.Add(2,settingText.GetHashCode(),Menu.None,settingText);
                    item.SetIcon(Resource.Drawable.IconSettingsApplicationsGrey48dp);
                }
            }
        }

        private async void setUser(Account account) {
            View.ScreenName.Text = $"@{account.ScreenName}";
            if(account.User == null) {
                try {
                    account.User = await account.Token.Users.ShowAsync(account.Id);
                    await Application.UserContainerManager.AddAsync(account.User);
                } catch(Exception) {
                    View.Background.SetImageDrawable(new ColorDrawable(Color.Green));
                    return;
                }
            }
            View.Icon.LoadImageUrlAcync(account.User.GetProfileImageUrl("bigger").AbsoluteUri);
            View.Name.Text = account.User.Name;
            View.ScreenName.Text = $"@{account.User.ScreenName}";
            if(account.User.ProfileBannerUrl != null) {
                View.Background.LoadImageUrlAcync($"{account.User.ProfileBannerUrl}/mobile_retina",adaptBackgroundColor);
            }else if(account.User.ProfileBackgroundColor != null) {
                try {
                    View.Background.SetImageDrawable(new ColorDrawable((Color.ParseColor($"#{account.User.ProfileLinkColor}"))));
                } catch(Exception) {
                    View.Background.SetImageDrawable(new ColorDrawable((Color.Green)));
                }
            }else {
                View.Background.SetImageDrawable(new ColorDrawable((Color.Green)));
            }
            if(View.Background.Drawable is BitmapDrawable) {
                Bitmap bm = ((BitmapDrawable)View.Background.Drawable).Bitmap;
                adaptBackgroundColor(bm);
            }else {
                adaptBackgroundColor(Color.Black.ToArgb());
            }
        }

        private async void adaptBackgroundColor(Bitmap bm) {
            Palette palette = await Task.Run(() => {
               try {
                    return Palette.From(bm).Generate();
                } catch(Exception) {
                    return null;
                }
            });
            if(palette == null) {
                return;
            }
            if(palette.MutedSwatch == null) {
                return;
            }
            adaptBackgroundColor(palette.MutedSwatch.BodyTextColor);
        }

        private void adaptBackgroundColor(int color) {
            View.Name.SetTextColor(new Color(color));
            View.ScreenName.SetTextColor(new Color(color));
            if(View.Drop.Drawable == null) {
                return;
            }
            Drawable d = DrawableCompat.Wrap(View.Drop.Drawable);
            DrawableCompat.SetTint(d,color);
            View.Drop.SetImageDrawable(d);
        }

        private void setDrop() {
            if(isShowAccounts) {
                View.Drop.SetImageResource(Resource.Drawable.IconArrowDropUpGrey24dp);
            } else {
                View.Drop.SetImageResource(Resource.Drawable.IconArrowDropDownGrey24dp);
            }
        }

        private void selectItem(object sender,NavigationItemSelectedEventArgs e) {
            IMenuItem item = e.MenuItem;
            if(isShowAccounts) {
                if(item.GroupId == 1) {
                    selectAccount1Item(item);
                }
                if(item.GroupId == 2) {
                    selectAccount2Item(item);
                }
            }else {
                if(item.GroupId == 1) {
                    selectGeneral1Item(item);
                }
                if(item.GroupId == 2) {
                    selectGeneral2Item(item);
                }
            }
            
        }

        private void selectAccount1Item(IMenuItem item) {
            if(Application.AccountManager.Account.Any(x => x.Id.GetHashCode() == item.ItemId)) {
                Application.SettingManager.Accounts.DefaultAccountId = Application.AccountManager.Account.First(x => x.Id.GetHashCode() == item.ItemId).Id;
                resetMenu();
            }
        }

        private void selectAccount2Item(IMenuItem item) {
            if(item.ItemId == addAccountText.GetHashCode()) {
                View.Activity.StartActivityCompat(typeof(LoginActivity));
                View.DrawerLayout.CloseDrawers();
            }
        }

        private void selectGeneral1Item(IMenuItem item) {

        }

        private void selectGeneral2Item(IMenuItem item) {
            if(item.ItemId == settingText.GetHashCode()) {
                SettingActivity.StartMain(View.Activity);
                View.DrawerLayout.CloseDrawers();
            }
        }

    }
}