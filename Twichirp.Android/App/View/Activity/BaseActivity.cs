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
using Android.Support.V7.App;
using Twichirp.Core.App;
using SFragment = Android.Support.V4.App.Fragment;
using Twichirp.Android.App.View.Fragment;

namespace Twichirp.Android.App.View.Activity {

    public abstract class BaseActivity : AppCompatActivity ,ILifeCycle,IView{ 

        public event EventHandler<LifeCycleEventArgs> OnCreateEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnDestroyEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnPauseEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnRestoreInstanceStateEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnResumeEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnSaveInstanceStateEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnNewIntentEventHandler;

        public ITwichirpApplication TwichirpApplication => this.Application as ITwichirpApplication;

        public AppCompatActivity Activity => this;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            OnViewCreate(savedInstanceState);
            OnCreateEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnCreate),savedInstanceState));
            if(savedInstanceState != null) {
                OnRestoreInstanceStateEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnCreate),savedInstanceState));
            }
        }

        protected abstract void OnViewCreate(Bundle savedInstanceState);

        protected override void OnDestroy() {
            base.OnDestroy();
            OnDestroyEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnDestroy)));
        }

        protected override void OnResume() {
            base.OnResume();
            OnResumeEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnResume)));
        }

        protected override void OnPause() {
            base.OnPause();
            OnPauseEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnPause)));
        }

        protected override void OnSaveInstanceState(Bundle outState) {
            base.OnSaveInstanceState(outState);
            OnSaveInstanceStateEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnSaveInstanceState),outState));
        }

        protected override void OnNewIntent(Intent intent) {
            base.OnNewIntent(intent);
            OnNewIntentEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnNewIntent),intent: intent));
            if(SupportFragmentManager.Fragments == null) {
                return;
            }
            foreach(SFragment fragment in SupportFragmentManager.Fragments) {
                if(fragment is BaseFragment) {
                    (fragment as BaseFragment).OnNewIntent(intent);
                }
            }
        }
    }
}