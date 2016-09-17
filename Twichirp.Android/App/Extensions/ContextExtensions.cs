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

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Threading.Tasks;
using Square.Picasso;
using Twichirp.Core.App;
using Android.Util;

namespace Twichirp.Android.App.Extensions {
    public static class ContextExtensions {

        public static TwichirpApplication ToTwichirpApplication(this Context context) => ((TwichirpApplication)context.ApplicationContext);

        public static void ShowToast(this Context context,int resource) => Toast.MakeText(context.ApplicationContext,resource,ToastLength.Short).Show();

        public static void ShowToast(this Context context,string text) => Toast.MakeText(context.ApplicationContext,text,ToastLength.Short).Show();

        public static async void LoadIntoBitmap(this Context context,string url,ImageView imageView,Action<Bitmap> callback = null) {
            Bitmap bm = await Task.Run(() => {
                try {
                    return Picasso.With(context).Load(url).Get();
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
                callback?.Invoke(bm);
            }
        }

    }
}