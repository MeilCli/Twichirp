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
using Twichirp.Android.App.View;
using Twichirp.Core.App.ViewModel;

namespace Twichirp.Android.App.ViewController {
    public class LoadingViewController : BaseViewController<ILoadingView,LoadingViewModel> {

        public LoadingViewController(ILoadingView view,LoadingViewModel viewModel) : base(view,viewModel) {
            view.OnCreateEventHandler += onCreate;
        }

        private void onCreate(LifeCycleEventArgs e) {
            var loadingText = ViewModel.IsLoaing
                .Select(x => x == true ? Resource.String.Loading : Resource.String.LoadingLoad)
                .Select(x => View.ApplicationContext.GetString(x))
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            View.LoadingText.SetBinding(x => x.Text,loadingText);
            var progressBarVisible = ViewModel.IsLoaing
                .Select(x => x == true ? ViewStates.Visible : ViewStates.Invisible)
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            View.ProgressBar.SetBinding(x => x.Visibility,progressBarVisible);
            View.ClickableView.ClickAsObservable().SetCommand(ViewModel.LoadCommand);
        }
    }
}