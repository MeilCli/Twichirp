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
using Android.Util;
using Android.Views;
using Android.Widget;
using Twichirp.Android.App.ViewController;
using Twichirp.Core.App.ViewModel;
using AView = Android.Views.View;

namespace Twichirp.Android.App.View.Holder {
    public class LoadingHolder : BaseHolder<BaseViewModel> ,ILoadingView{

        private LoadingViewController loadingViewController;

        public TextView LoadingText { get; private set; }

        public ProgressBar ProgressBar { get; private set; }

        public AView ClickableView { get; private set; }

        public LoadingHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup) : base(view,lifeCycle,viewGroup,Resource.Layout.LoadingHolder) {
        }

        ~LoadingHolder() {
            LoadingText?.Dispose();
            ProgressBar?.Dispose();
            ClickableView?.Dispose();
        }

        public override void OnCreatedView() {
            LoadingText = ItemView.FindViewById<TextView>(Resource.Id.LoadingText);
            ProgressBar = ItemView.FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            ClickableView = ItemView;
        }

        public override void OnPreBind(BaseViewModel item,int position) {
            this.loadingViewController = new LoadingViewController(this,item as LoadingViewModel);
        }
    }
}