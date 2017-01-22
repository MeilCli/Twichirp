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

namespace Twichirp.Android.App.View {

    public class LifeCycleEventArgs :EventArgs{

        public string EventName { get; }

        public Bundle State { get; }

        public Intent Intent { get; }

        public LifeCycleEventArgs(string eventName,Bundle state = null,Intent intent = null) {
            EventName = eventName;
            State = state;
        }
    }

    public interface ILifeCycle {

        event EventHandler<LifeCycleEventArgs> OnCreateEventHandler;

        event EventHandler<LifeCycleEventArgs> OnDestoryEventHandler;

        event EventHandler<LifeCycleEventArgs> OnResumeEventHandler;

        event EventHandler<LifeCycleEventArgs> OnPauseEventHandler;

        event EventHandler<LifeCycleEventArgs> OnSaveInstanceStateEventHandler;

        event EventHandler<LifeCycleEventArgs> OnRestoreInstanceStateEventHandler;

        event EventHandler<LifeCycleEventArgs> OnNewIntentEventHandler;
    }
}