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
using System.Reactive.Disposables;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Twichirp.Android.App.View;
using Twichirp.Core.App;

namespace Twichirp.Android.App.ViewController {
    public class BaseViewController<TView,TViewModel>:IDisposable where TView : IView,ILifeCycleView {

        public TView View { get; }
        public TViewModel ViewModel { get; }
        public ITwichirpApplication Application => View.TwichirpApplication;
        protected bool AutoDisposeViewModel { get; set; } = true;
        internal CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public BaseViewController(TView view,TViewModel viewModel) {
            View = view;
            ViewModel = viewModel;
            if(viewModel is IDisposable) {
                view.Destroyed += (x,y) => { if(AutoDisposeViewModel) (viewModel as IDisposable).Dispose(); };
            }
            view.Destroyed += (x,y) => { if(Disposable.IsDisposed == false) Disposable.Dispose(); };
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }

    public class BaseViewController<TView>:IDisposable where TView : IView, ILifeCycleView {

        public TView View { get; }
        public ITwichirpApplication Application => View.TwichirpApplication;
        internal CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public BaseViewController(TView view) {
            View = view;
            view.Destroyed += (x,y) => { if(Disposable.IsDisposed == false) Disposable.Dispose(); };
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}