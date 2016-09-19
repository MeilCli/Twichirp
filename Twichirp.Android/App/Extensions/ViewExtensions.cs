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
using Square.Picasso;
using AView = Android.Views.View;
using JString = Java.Lang.String;

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

        public static async void LoadImageUrlAcync(this ImageView imageView,string url,Action<Bitmap> callback = null) {
            if(imageView == null) {
                return;
            }
            if(imageView.Drawable != null && (imageView.Tag?.ToString().Equals(new JString(url)) ?? false)) {
                // 実行されるかどうかは確認していない
                return;
            }
            Bitmap bm = await Task.Run(() => {
                try {
                    return Picasso.With(imageView.Context.ApplicationContext).Load(url).Get();
                } catch(Exception) {
                    return null;
                }
            });
            // https://developer.xamarin.com/guides/android/advanced_topics/garbage_collection/#Helping_the_GC
            // こんなことしていいらしい
            using(bm) {
                if(bm == null) {
                    return;
                }
                if(imageView == null) {
                    bm?.Recycle();
                    return;
                }
                imageView.SetImageBitmap(bm);
                imageView.Tag = new JString(url);
                callback?.Invoke(bm);
            }
        }
    }
}