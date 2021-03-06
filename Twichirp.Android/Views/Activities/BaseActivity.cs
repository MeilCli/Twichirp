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
using Twichirp.Android.Views.Fragments;
using Twichirp.Core;
using SFragment = Android.Support.V4.App.Fragment;

namespace Twichirp.Android.Views.Activities {

    public abstract class BaseActivity : AppCompatActivity, ILifeCycleView, IView {

        public event EventHandler<LifeCycleEventArgs> Created;
        public event EventHandler<LifeCycleEventArgs> Destroyed;
        public event EventHandler<LifeCycleEventArgs> ViewDestroyed;
        public event EventHandler<LifeCycleEventArgs> Paused;
        public event EventHandler<LifeCycleEventArgs> InstanceStateRestored;
        public event EventHandler<LifeCycleEventArgs> Resumed;
        public event EventHandler<LifeCycleEventArgs> InstanceStateSaved;
        public event EventHandler<LifeCycleEventArgs> NewIntentRecieved;

        public ITwichirpApplication TwichirpApplication => this.Application as ITwichirpApplication;

        public AppCompatActivity Activity => this;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            OnViewCreate(savedInstanceState);
            Created?.Invoke(this, new LifeCycleEventArgs(nameof(OnCreate), savedInstanceState));
            if (savedInstanceState != null) {
                InstanceStateRestored?.Invoke(this, new LifeCycleEventArgs(nameof(OnCreate), savedInstanceState));
            }
        }

        protected abstract void OnViewCreate(Bundle savedInstanceState);

        protected override void OnDestroy() {
            base.OnDestroy();
            ViewDestroyed?.Invoke(this, new LifeCycleEventArgs(nameof(OnDestroy)));
            Destroyed?.Invoke(this, new LifeCycleEventArgs(nameof(OnDestroy)));
        }

        protected override void OnResume() {
            base.OnResume();
            Resumed?.Invoke(this, new LifeCycleEventArgs(nameof(OnResume)));
        }

        protected override void OnPause() {
            base.OnPause();
            Paused?.Invoke(this, new LifeCycleEventArgs(nameof(OnPause)));
        }

        protected override void OnSaveInstanceState(Bundle outState) {
            base.OnSaveInstanceState(outState);
            InstanceStateSaved?.Invoke(this, new LifeCycleEventArgs(nameof(OnSaveInstanceState), outState));
        }

        protected override void OnNewIntent(Intent intent) {
            base.OnNewIntent(intent);
            NewIntentRecieved?.Invoke(this, new LifeCycleEventArgs(nameof(OnNewIntent), intent: intent));
            if (SupportFragmentManager.Fragments == null) {
                return;
            }
            foreach (SFragment fragment in SupportFragmentManager.Fragments) {
                if (fragment is BaseFragment baseFragment) {
                    baseFragment.OnNewIntent(intent);
                }
            }
        }
    }
}