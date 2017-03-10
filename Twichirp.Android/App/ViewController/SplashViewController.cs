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
using Twichirp.Core.App.ViewModel;
using Reactive.Bindings;
using Twichirp.Android.Extensions;
using Twichirp.Android.App.View.Activity;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;
using Twichirp.Android.Events;

namespace Twichirp.Android.App.ViewController {

    public class SplashViewController : BaseViewController<ISplashView,SplashViewModel> {

        public SplashViewController(ISplashView view,SplashViewModel viewModel) : base(view,viewModel) {
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Created += x,x => view.Created -= x)
                .Subscribe(x => onCreate(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            viewModel.StartMainPageCommand.Subscribe(x => {
                View.Activity.StartActivityCompat(typeof(MainActivity));
                View.Activity.Finish();
            }).AddTo(Disposable);
            viewModel.StartLoginPageCommand.Subscribe(x => {
                View.Activity.StartActivityCompat(typeof(LoginActivity));
                View.Activity.Finish();
            }).AddTo(Disposable);
        }

        private void onCreate(object sender,LifeCycleEventArgs e) {
            View.Message.SetBinding(x => x.Text,ViewModel.Message).AddTo(Disposable);
            ViewModel.ApplicationInitCommand.Execute();
        }
    }
}