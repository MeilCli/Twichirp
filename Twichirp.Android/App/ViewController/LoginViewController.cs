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
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Twichirp.Android.App.View;
using Twichirp.Core.App.ViewModel;
using Twichirp.Android.App.Extensions;
using Twichirp.Android.App.View.Activity;
using Twichirp.Android.App.View.Fragment.Dialog;

namespace Twichirp.Android.App.ViewController {
    public class LoginViewController : BaseViewController<ILoginView,LoginViewModel> {

        public LoginViewController(ILoginView view,LoginViewModel viewModel) : base(view,viewModel) {
            view.OnCreateEventHandler += onCreate;
            viewModel.ShowMessageCommand.Subscribe(x => View.ApplicationContext.ShowToast(x));
            viewModel.StartNextPageCommand.Subscribe(x => {
                View.Activity.StartActivityCompat(typeof(MainActivity));
                View.Activity.Finish();
            });
            viewModel.StartLoginWebPageCommand.Subscribe(x => {
                var intent = new Intent(Intent.ActionView,global::Android.Net.Uri.Parse(x));
                View.Activity.StartActivity(intent);
            });
            viewModel.IsLoading.ObserveOnUIDispatcher().Subscribe(x => {
                if(x == true) {
                    if(View.Activity.SupportFragmentManager.FindFragmentByTag(ProgressDialogFragment.FragmentTag) != null) {
                        return;
                    }
                    var fragment = ProgressDialogFragment.NewInstance(string.Empty,string.Empty);
                    fragment.Show(View.Activity.SupportFragmentManager,ProgressDialogFragment.FragmentTag);
                }else {
                    var fragment = View.Activity.SupportFragmentManager.FindFragmentByTag(ProgressDialogFragment.FragmentTag);
                    if(fragment==null||fragment is ProgressDialogFragment == false) {
                        return;
                    }
                    (fragment as ProgressDialogFragment).Dialog.Dismiss();
                }
            });
        }

        private void onCreate(object sender,LifeCycleEventArgs e) {
            View.GoToWeb.ClickAsObservable().SetCommand(ViewModel.AuthorizeCommand);
            View.Login.ClickAsObservable().SetCommand(ViewModel.LoginCommand);
            View.Pin.SetBinding(x => x.Text,ViewModel.Pin,x=>x.TextChangedAsObservable().ToUnit());
        }
    }
}