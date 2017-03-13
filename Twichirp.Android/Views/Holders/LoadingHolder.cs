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

using Android.Views;
using Android.Widget;
using Twichirp.Android.ViewControllers;
using Twichirp.Android.Views.Interfaces;
using Twichirp.Core.ViewModels;
using AView = Android.Views.View;

// 未使用フィールドの警告非表示
#pragma warning disable 0414

namespace Twichirp.Android.Views.Holders {

    public class LoadingHolder : BaseHolder<BaseViewModel>, ILoadingView {

        public static LoadingHolder Make(IView view, ViewGroup parent) {
            var itemView = InflateView(view, Resource.Layout.LoadingHolder, parent);
            var holder = new LoadingHolder(itemView, view);
            holder.OnCreatedView();
            return holder;
        }

        private LoadingViewController loadingViewController;

        public TextView LoadingText { get; private set; }

        public ProgressBar ProgressBar { get; private set; }

        public AView ClickableView { get; private set; }

        private LoadingHolder(AView itemView, IView view) : base(itemView, view) {
        }

        protected override void OnDestroyView() {
            LoadingText?.Dispose();
            ProgressBar?.Dispose();
            ClickableView?.Dispose();
        }

        public override void OnCreatedView() {
            LoadingText = ItemView.FindViewById<TextView>(Resource.Id.LoadingText);
            ProgressBar = ItemView.FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            ClickableView = ItemView;
        }

        public override void OnPreBind(BaseViewModel item, int position) {
            this.loadingViewController = new LoadingViewController(this, item as LoadingViewModel);
        }
    }
}