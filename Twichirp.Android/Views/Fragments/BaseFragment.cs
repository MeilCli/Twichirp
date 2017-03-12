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
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Twichirp.Android.Events;
using Twichirp.Core;
using SFragment = Android.Support.V4.App.Fragment;

namespace Twichirp.Android.Views.Fragments {

    public abstract class BaseFragment : SFragment, ILifeCycleView, IView {

        public event EventHandler<LifeCycleEventArgs> Created;
        public event EventHandler<LifeCycleEventArgs> Destroyed;
        public event EventHandler<LifeCycleEventArgs> ViewDestroyed;
        public event EventHandler<LifeCycleEventArgs> Paused;
        public event EventHandler<LifeCycleEventArgs> InstanceStateRestored;
        public event EventHandler<LifeCycleEventArgs> Resumed;
        public event EventHandler<LifeCycleEventArgs> InstanceStateSaved;
        public event EventHandler<LifeCycleEventArgs> NewIntentRecieved;

        public Context ApplicationContext => Context.ApplicationContext;

        AppCompatActivity IView.Activity {
            get {
                if (base.Activity is AppCompatActivity == false) {
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
            Created?.Invoke(this, new LifeCycleEventArgs(nameof(OnActivityCreated), savedInstanceState));
            if (savedInstanceState != null) {
                InstanceStateRestored?.Invoke(this, new LifeCycleEventArgs(nameof(OnActivityCreated), savedInstanceState));
            }
        }

        public override void OnDestroyView() {
            base.OnDestroyView();
            ViewDestroyed?.Invoke(this, new LifeCycleEventArgs(nameof(OnDestroyView)));
        }

        public override void OnDestroy() {
            base.OnDestroy();
            Destroyed?.Invoke(this, new LifeCycleEventArgs(nameof(OnDestroy)));
        }

        public override void OnResume() {
            base.OnResume();
            Resumed?.Invoke(this, new LifeCycleEventArgs(nameof(OnResume)));
        }

        public override void OnPause() {
            base.OnPause();
            Paused?.Invoke(this, new LifeCycleEventArgs(nameof(OnPause)));
        }

        public override void OnSaveInstanceState(Bundle outState) {
            base.OnSaveInstanceState(outState);
            InstanceStateSaved?.Invoke(this, new LifeCycleEventArgs(nameof(OnSaveInstanceState), outState));
        }

        public void OnNewIntent(Intent intent) {
            NewIntentRecieved?.Invoke(this, new LifeCycleEventArgs(nameof(OnNewIntent), intent: intent));
        }

    }
}