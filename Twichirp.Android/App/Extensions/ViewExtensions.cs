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
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;

namespace Twichirp.Android.App.Extensions {
    public static class ViewExtensions {

        public static void SetTransitionNameCompat(this AView view,string name) {
            if(Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) {
                view.TransitionName = name;
            }
        }

        public static void ReleaseImage(this ImageView imageView) {
            var drawable = imageView.Drawable;
            imageView.SetImageDrawable(null);
            imageView.Tag = null;
            if(drawable is BitmapDrawable) {
                //(drawable as BitmapDrawable)?.Bitmap?.Recycle();
            }
        }
    }
}