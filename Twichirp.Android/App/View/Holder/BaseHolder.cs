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
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Twichirp.Core.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Support.V4.View;

namespace Twichirp.Android.App.View.Holder {
    public abstract class BaseHolder<T> : RecyclerView.ViewHolder, IView,ILifeCycle,IBindable<T>,IRecyclable {

        public event EventHandler<LifeCycleEventArgs> OnCreateEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnDestoryEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnResumeEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnPauseEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnSaveInstanceStateEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnRestoreInstanceStateEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnNewIntentEventHandler;

        private IView view;

        public AppCompatActivity Activity => view.Activity;

        public Context ApplicationContext => view.ApplicationContext;

        public ITwichirpApplication TwichirpApplication => view.TwichirpApplication;

        public BaseHolder(IView view,ILifeCycle lifeCycle,ViewGroup viewGroup,int layoutResource) 
            : base(LayoutInflater.From(view.Activity).Inflate(layoutResource,null)) {
            this.IsRecyclable = false;
            this.view = view;
            ItemView.ViewDetachedFromWindow += onDetatchViewFromWindow;
            OnCreatedView();
        }

        public abstract void OnCreatedView();

        public void OnBind(T item,int position) {
            OnPreBind(item,position);
            OnCreateEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnBind)));
        }

        public abstract void OnPreBind(T item,int position);

        public void OnRecycled() {
            //OnDestoryEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnRecycled)));
        }

        private void onDetatchViewFromWindow(object sender,EventArgs e) {
            OnDestoryEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(onDetatchViewFromWindow)));
        }
    }
}