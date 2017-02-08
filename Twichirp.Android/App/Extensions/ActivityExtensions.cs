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
using AView = Android.Views.View;

namespace Twichirp.Android.App.Extensions {
    public static class ActivityExtensions {

        public static void StartActivityCompat(this Activity activity,Type type,Tuple<AView,string> element = null) {
            var intent = new Intent(activity.ApplicationContext,type);
            StartActivityCompat(activity,intent,element);
        }

        public static void StartActivityCompat(this Activity activity,Intent intent,Tuple<AView,string> element = null) {
            if(Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && element != null) {
                ActivityOptions options = ActivityOptions.MakeSceneTransitionAnimation(activity,element.Item1,element.Item2);
                activity.StartActivity(intent,options.ToBundle());
            } else {
                activity.StartActivity(intent);
            }
        }
    }
}