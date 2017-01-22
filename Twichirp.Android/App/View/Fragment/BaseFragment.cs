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
using Android.Util;
using Android.Views;
using Android.Widget;
using Twichirp.Core.App;
using SFragment = Android.Support.V4.App.Fragment;

namespace Twichirp.Android.App.View.Fragment {
    public abstract class BaseFragment : SFragment ,ILifeCycle,IView{
        
        public event EventHandler<LifeCycleEventArgs> OnCreateEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnDestoryEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnPauseEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnRestoreInstanceStateEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnResumeEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnSaveInstanceStateEventHandler;
        public event EventHandler<LifeCycleEventArgs> OnNewIntentEventHandler;

        public Context ApplicationContext => Context.ApplicationContext;

        AppCompatActivity IView.Activity {
            get {
                if(base.Activity is AppCompatActivity == false) {
                    throw new InvalidProgramException();
                }
                return base.Activity as AppCompatActivity;
            }
        }

        public ITwichirpApplication TwichirpApplication => ApplicationContext as TwichirpApplication;

        public BaseFragment() {
            RetainInstance = true;
        }

        public override void OnActivityCreated(Bundle savedInstanceState) {
            base.OnActivityCreated(savedInstanceState);
            OnCreateEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnActivityCreated),savedInstanceState));
            if(savedInstanceState != null) {
                OnRestoreInstanceStateEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnActivityCreated),savedInstanceState));
            }
        }

        public override void OnDestroy() {
            base.OnDestroy();
            OnDestoryEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnDestroy)));
        }

        public override void OnResume() {
            base.OnResume();
            OnResumeEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnResume)));
        }

        public override void OnPause() {
            base.OnPause();
            OnPauseEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnPause)));
        }

        public override void OnSaveInstanceState(Bundle outState) {
            base.OnSaveInstanceState(outState);
            OnSaveInstanceStateEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnSaveInstanceState),outState));
        }

        public void OnNewIntent(Intent intent) {
            OnNewIntentEventHandler?.Invoke(this,new LifeCycleEventArgs(nameof(OnNewIntent),intent: intent));
        }

    }
}