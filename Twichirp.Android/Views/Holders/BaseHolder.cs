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
using AView = Android.Views.View;
using Twichirp.Android.Events;

// 未使用イベントの警告非表示
#pragma warning disable 0067

namespace Twichirp.Android.Views.Holders {
    
    public abstract class BaseHolder<T> : RecyclerView.ViewHolder, IView, ILifeCycleView, IBindable<T>, IRecyclable {

        protected static AView InflateView(IView view, int layoutResource) {
            return LayoutInflater.From(view.Activity).Inflate(layoutResource, null);
        }

        public event EventHandler<LifeCycleEventArgs> Created;
        public event EventHandler<LifeCycleEventArgs> Destroyed;
        public event EventHandler<LifeCycleEventArgs> ViewDestroyed;
        public event EventHandler<LifeCycleEventArgs> Resumed;
        public event EventHandler<LifeCycleEventArgs> Paused;
        public event EventHandler<LifeCycleEventArgs> InstanceStateSaved;
        public event EventHandler<LifeCycleEventArgs> InstanceStateRestored;
        public event EventHandler<LifeCycleEventArgs> NewIntentRecieved;

        private IView view;

        public AppCompatActivity Activity => view.Activity;

        public Context ApplicationContext => view.ApplicationContext;

        public ITwichirpApplication TwichirpApplication => view.TwichirpApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Twichirp.Android.App.View.Holder.BaseHolder`1"/> class.
        /// Must call <see cref="OnCreatedView"/> after initializes this instance 
        /// </summary>
        /// <param name="itemView">Item view.</param>
        /// <param name="view">View.</param>
        protected BaseHolder(AView itemView,IView view)
            : base(itemView) {
            this.IsRecyclable = false;
            this.view = view;
            ItemView.ViewDetachedFromWindow += onDetatchViewFromWindow;
        }

        public abstract void OnCreatedView();

        public void OnBind(T item,int position) {
            OnPreBind(item,position);
            Created?.Invoke(this,new LifeCycleEventArgs(nameof(OnBind)));
        }

        public abstract void OnPreBind(T item,int position);

        private void onDetatchViewFromWindow(object sender,EventArgs e) {
            ItemView.ViewDetachedFromWindow -= onDetatchViewFromWindow;
            ViewDestroyed?.Invoke(this,new LifeCycleEventArgs(nameof(onDetatchViewFromWindow)));
            Destroyed?.Invoke(this,new LifeCycleEventArgs(nameof(onDetatchViewFromWindow)));
            OnDestroyView();
        }

        protected abstract void OnDestroyView();

        public void OnRecycled() {
        }
    }
}