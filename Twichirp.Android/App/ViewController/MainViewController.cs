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

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Twichirp.Android.App.View;
using static Android.Support.Design.Widget.NavigationView;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Twichirp.Android.Extensions;
using Android.Support.V7.Graphics;
using Android.Support.V4.Graphics.Drawable;
using Android.Support.V4.Content;
using Twichirp.Android.App.View.Activity;
using Twichirp.Android.App.Setting;
using CoreTweet;
using Android.Util;
using FFImageLoading;
using FFImageLoading.Transformations;
using Twichirp.Android.App.ViewModel;
using Reactive.Bindings.Extensions;
using Twichirp.Android.Objects;
using System.Reactive.Linq;
using Reactive.Bindings;
using FFImageLoading.Views;
using Android.Animation;
using SFragment = Android.Support.V4.App.Fragment;
using Twichirp.Android.App.View.Fragment;
using BottomBarSharp;
using Twichirp.Core.DataObjects;
using Twichirp.Core.DataRepositories;
using Microsoft.Practices.Unity;
using Twichirp.Core.App.Setting;
using Twichirp.Android.Events;

namespace Twichirp.Android.App.ViewController {

    public class MainViewController : BaseViewController<IMainView,MainViewModel> {

        private static string getFragmentTag(int position) {
            return $"fragment_{position}";
        }

        private bool isTabInited;

        public MainViewController(IMainView view,MainViewModel viewModel) : base(view,viewModel) {
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Created += x,x => view.Created -= x)
                .Subscribe(x => onCreate(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Resumed += x,x => view.Resumed -= x)
                .Subscribe(x => onResume(x.Sender,x.EventArgs))
                .AddTo(Disposable);
        }

        private void onCreate(object sender,LifeCycleEventArgs args) {
            View.Navigation.NavigationItemSelected += (s,e) => {
                ViewModel.NavigationMenuSelectedCommand.Execute(Tuple.Create(e.MenuItem.GroupId,e.MenuItem.ItemId));
            };
            View.Subtitle.ClickAsObservable().Subscribe(x => ViewModel.NavigationMenuGroupReverseCommand.Execute()).AddTo(Disposable);
            ViewModel.NavigationMenus.Subscribe(x => setNavigationMenus(x)).AddTo(Disposable);
            ViewModel.IsNavigationHidingGroup.Subscribe(x => {
                setDrop(x);
                setSubIconsVisible(x);
            }).AddTo(Disposable);

            View.BottomBar.TabSelect += tabSelect;
            ViewModel.NavigationTabs.Subscribe(x => setNavigationTabs(x)).AddTo(Disposable);

            ViewModel.UserIcon.Subscribe(x => setIcon(x)).AddTo(Disposable);
            View.IconClickable.ClickAsObservable().SetCommand(ViewModel.StartUserProfileActivityCommand).AddTo(Disposable);
            ViewModel.UserBanner.CombineLatest(ViewModel.UserLinkColor,(x,y) => Tuple.Create(x,y)).Subscribe(x => setUserBanner(x.Item1,x.Item2)).AddTo(Disposable);
            View.Name.SetBinding(x => x.Text,ViewModel.UserName).AddTo(Disposable);
            View.ScreenName.SetBinding(x => x.Text,ViewModel.UserScreenName).AddTo(Disposable);
            ViewModel.FirsrSubUserIcon.Subscribe(x => setSubIcon(View.FirstSubIconClickable,View.FirstSubIcon,x)).AddTo(Disposable);
            ViewModel.SecondSubUserIcon.Subscribe(x => setSubIcon(View.SecondSubIconClickable,View.SecondSubIcon,x)).AddTo(Disposable);
            View.FirstSubIconClickable.ClickAsObservable().SetCommand(ViewModel.FirstSubUserIconClickedCommand).AddTo(Disposable);
            View.SecondSubIconClickable.ClickAsObservable().SetCommand(ViewModel.SecondSubUserIconClickedCommand).AddTo(Disposable);

            ViewModel.StartLoginActivityCommand
                .Subscribe(x => {
                    View.Activity.StartActivityCompat(typeof(LoginActivity));
                    View.DrawerLayout.CloseDrawers();
                })
                .AddTo(Disposable);
            ViewModel.StartSettingActivityCommand
                .Subscribe(x => {
                    SettingActivity.StartMain(View.Activity);
                    View.DrawerLayout.CloseDrawers();
                })
                .AddTo(Disposable);
            ViewModel.StartUserProfileActivityCommand
                .Subscribe(x => {
                    UserProfileActivity.Start(View.Activity,ViewModel.UserId.Value,ViewModel.UserJson,null,View.Icon);
                })
                .AddTo(Disposable);
        }

        private void onResume(object sender,LifeCycleEventArgs args) {
            ViewModel.UpdateDefaultAccountIfChangedCommand.Execute();
        }

        private void setNavigationMenus(List<NavigationMenu> menus) {
            IMenu menu = View.Navigation.Menu;
            menu.Clear();
            foreach(var m in menus) {
                IMenuItem item;
                if(m.TextId != null) {
                    item = menu.Add(m.GroupId,m.Id,Menu.None,View.ApplicationContext.GetString(m.TextId.Value));
                } else {
                    item = menu.Add(m.GroupId,m.Id,Menu.None,m.Text);
                }
                item.SetIcon(m.Icon);
                if(m.IsChecked) {
                    item.SetChecked(true);
                }
            }
        }

        private void setUserBanner(string banner,string linkColor) {
            if(banner != null) {
                ImageService.Instance.LoadUrl($"{banner}/mobile_retina").IntoAsync(View.Background);
            } else if(banner != null) {
                try {
                    View.Background.SetImageDrawable(new ColorDrawable((Color.ParseColor($"#{linkColor}"))));
                } catch(Exception) {
                    View.Background.SetImageDrawable(new ColorDrawable((Color.Green)));
                }
            } else {
                View.Background.SetImageDrawable(new ColorDrawable((Color.Green)));
            }
        }

        private void setDrop(bool isHiding) {
            if(isHiding) {
                View.Drop.SetImageResource(Resource.Drawable.IconArrowDropUpGrey24dp);
            } else {
                View.Drop.SetImageResource(Resource.Drawable.IconArrowDropDownGrey24dp);
            }
            Drawable d = DrawableCompat.Wrap(View.Drop.Drawable);
            DrawableCompat.SetTint(d,Color.White);
            View.Drop.SetImageDrawable(d);
        }

        private async void setIcon(string url) {
            ImageService.Instance.LoadUrl(url).Transform(new CircleTransformation()).Into(View.Icon);
            var drawable = await ImageService.Instance.LoadUrl(url).DownSampleInDip(24,24).Transform(new CircleTransformation()).AsBitmapDrawableAsync();
            View.Toolbar.Logo = drawable;
            View.Toolbar.TitleMarginStart = 100;
        }

        private void setSubIcon(FrameLayout container,ImageViewAsync imageview,string url) {
            setSubIconVisible(container,url != null);
            if(url != null) {
                ImageService.Instance.LoadUrl(url).Transform(new CircleTransformation()).Into(imageview);
            }
        }

        private void setSubIconVisible(FrameLayout container,bool isVisible) {
            if(isVisible) {
                container.Visibility = ViewStates.Visible;
            } else {
                container.Visibility = ViewStates.Gone;
            }
        }

        private void setSubIconsVisible(bool isHiding) {
            Action<FrameLayout> hideAnimation = container => {
                var animation = ValueAnimator.OfFloat(1f,0f);
                animation.SetDuration(500);
                animation.Update += (s,e) => {
                    container.Alpha = (float)e.Animation.AnimatedValue;
                };
                animation.AnimationEnd += (s,e) => {
                    container.Visibility = ViewStates.Invisible;
                };
                animation.Start();
            };
            if(isHiding == true && View.FirstSubIconClickable.Visibility == ViewStates.Visible) {
                hideAnimation(View.FirstSubIconClickable);
            }
            if(isHiding == true && View.SecondSubIconClickable.Visibility == ViewStates.Visible) {
                hideAnimation(View.SecondSubIconClickable);
            }

            Action<FrameLayout> showAnimation = container => {
                var animation = ValueAnimator.OfFloat(0f,1f);
                animation.SetDuration(500);
                animation.AnimationStart += (s,e) => {
                    container.Visibility = ViewStates.Visible;
                };
                animation.Update += (s,e) => {
                    container.Alpha = (float)e.Animation.AnimatedValue;
                };
                animation.Start();
            };
            if(isHiding == false && View.FirstSubIconClickable.Visibility == ViewStates.Invisible) {
                showAnimation(View.FirstSubIconClickable);
            }
            if(isHiding == false && View.SecondSubIconClickable.Visibility == ViewStates.Invisible) {
                showAnimation(View.SecondSubIconClickable);
            }
        }

        private void setNavigationTabs(List<NavigationTab> tabs) {
            var accountRepository = View.TwichirpApplication.Resolve<IAccountRepository>();
            var settingManager = View.TwichirpApplication.Resolve<SettingManager>();
            var account = accountRepository[settingManager.Accounts.DefaultAccountId];
            var trasaction = View.Activity.SupportFragmentManager.BeginTransaction();
            var tabList = new List<BottomBarTab>();

            bool changed = false;
            SFragment firstFragment = null;
            for(int i = 0;i < 5 && i < tabs.Count;i++) {
                var tab = tabs[i];

                tabList.Add(View.BottomBar.NewTab(getTabId(i),tab.Icon,tab.Text));

                SFragment fragment = View.Activity.SupportFragmentManager.FindFragmentByTag(getFragmentTag(i));
                if(fragment != null && wasAddedFragment(fragment,tab,account)) {
                    continue;
                } else if(fragment != null) {
                    trasaction.Remove(fragment);
                }

                changed = true;
                fragment = createFragment(tab,account);
                if(fragment == null) {
                    continue;
                }
                trasaction.Add(Resource.Id.Content,fragment,getFragmentTag(i));

                if(i == 0) {
                    firstFragment = fragment;
                }

                if(isTabInited == false && i == 0) {
                    isTabInited = true;
                } else {
                    trasaction.Hide(fragment);
                }
            }
            if(changed && firstFragment != null) {
                // Fragment追加時、BottomBarのイベントで表示しようとしてもタイミングが合わない
                trasaction.Show(firstFragment);
            }
            trasaction.Commit();

            View.BottomBar.SetItems(tabList);
            if(changed) {
                View.BottomBar.SelectTabAtPosition(0);
            }

        }

        private bool wasAddedFragment(SFragment fragment,NavigationTab tab,ImmutableAccount account) {
            switch(tab.Id) {
                case Resource.Id.TabHome:
                    return (fragment as StatusTimelineFragment)?.Equals(StatusTimelineFragmentType.Home,account) ?? false;
                case Resource.Id.TabMention:
                    return (fragment as StatusTimelineFragment)?.Equals(StatusTimelineFragmentType.Mention,account) ?? false;
            }
            return false;
        }

        private SFragment createFragment(NavigationTab tab,ImmutableAccount account) {
            switch(tab.Id) {
                case Resource.Id.TabHome:
                    return StatusTimelineFragment.Make(new StatusTimelineFragment.HomeParameter(account));
                case Resource.Id.TabMention:
                    return StatusTimelineFragment.Make(new StatusTimelineFragment.MentionParameter(account));
            }
            return null;
        }

        private void tabSelect(object sender,TabEventArgs args) {
            showOrHideTab(View.BottomBar.FindPositionForTabWithId(args.TabId));
        }

        private void showOrHideTab(int showPosition) {
            var trasaction = View.Activity.SupportFragmentManager.BeginTransaction();
            for(int i = 0;i < 5;i++) {
                SFragment fragment = View.Activity.SupportFragmentManager.FindFragmentByTag(getFragmentTag(i));
                if(fragment == null) {
                    continue;
                }
                if(i == showPosition) {
                    trasaction.Show(fragment);
                } else {
                    trasaction.Hide(fragment);
                }
            }
            trasaction.Commit();
        }

        private int getTabId(int position) {
            switch(position) {
                default:
                case 0:
                    return Resource.Id.Tab1;
                case 1:
                    return Resource.Id.Tab2;
                case 2:
                    return Resource.Id.Tab3;
                case 3:
                    return Resource.Id.Tab4;
                case 4:
                    return Resource.Id.Tab5;
            }
        }
    }
}