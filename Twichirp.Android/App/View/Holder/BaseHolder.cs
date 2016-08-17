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
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Twichirp.Core.App;
using Android.Support.V7.Widget;

namespace Twichirp.Android.App.View.Holder {
    abstract class BaseHolder<T> : RecyclerView.ViewHolder, IView,ILifeCycle,IBindable<T> {

        public event LifeCycleEvent OnCreateEventHandler;
        public event LifeCycleEvent OnDestoryEventHandler;
        public event LifeCycleEvent OnResumeEventHandler;
        public event LifeCycleEvent OnPauseEventHandler;
        public event LifeCycleEvent OnSaveInstanceStateEventHandler;
        public event LifeCycleEvent OnRestoreInstanceStateEventHandler;
        public event LifeCycleEvent OnNewIntentEventHandler;

        private IView view;

        public AppCompatActivity Activity => view.Activity;

        public Context ApplicationContext => view.ApplicationContext;

        public ITwichirpApplication TwichirpApplication => view.TwichirpApplication;

        public BaseHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup,int layoutResource) 
            : base(global::Android.Views.View.Inflate(viewGroup.Context,layoutResource,null)) {
            this.IsRecyclable = false;
            this.view = view;
            OnCreatedView();
        }

        public abstract void OnCreatedView();

        public void OnBind(T item,int position) {
            OnPreBind(item,position);
            OnCreateEventHandler?.Invoke(new LifeCycleEventArgs(nameof(OnBind)));
        }

        public abstract void OnPreBind(T item,int position);
    }
}