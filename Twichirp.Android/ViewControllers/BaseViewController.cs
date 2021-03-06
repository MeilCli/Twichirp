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
using System.Reactive.Disposables;
using Twichirp.Android.Views;
using Twichirp.Core;

namespace Twichirp.Android.ViewControllers {

    public class BaseViewController<TView, TViewModel> : IDisposable where TView : IView, ILifeCycleView {

        public TView View { get; }
        public TViewModel ViewModel { get; }
        public ITwichirpApplication Application => View.TwichirpApplication;
        protected bool AutoDisposeViewModel { get; set; } = true;
        internal CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public BaseViewController(TView view, TViewModel viewModel) {
            View = view;
            ViewModel = viewModel;
            if (viewModel is IDisposable disposable) {
                view.Destroyed += (x, y) => { if (AutoDisposeViewModel) disposable.Dispose(); };
            }
            view.Destroyed += (x, y) => { if (Disposable.IsDisposed == false) Disposable.Dispose(); };
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }

    public class BaseViewController<TView> : IDisposable where TView : IView, ILifeCycleView {

        public TView View { get; }
        public ITwichirpApplication Application => View.TwichirpApplication;
        internal CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public BaseViewController(TView view) {
            View = view;
            view.Destroyed += (x, y) => { if (Disposable.IsDisposed == false) Disposable.Dispose(); };
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}