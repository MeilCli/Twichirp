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
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Graphics;
using Android.Support.V4.Graphics.Drawable;
using Android.Views;
using Android.Widget;
using FFImageLoading.Transformations;

namespace Twichirp.Android.App.View {
    public class PlayCircleTransformation : TransformationBase {

        public override string Key => "PlayCircleTransformation";
        private Context context;

        public PlayCircleTransformation(Context context = null) {
            this.context = context ?? Application.Context;
        }

        protected override Bitmap Transform(Bitmap source) {
            var playCircle = BitmapFactory.DecodeResource(context.Resources,Resource.Drawable.IconPlayCircleOutlineGrey36dp);

            var config = source.GetConfig() ?? Bitmap.Config.Argb8888;
            int width = source.Width;
            int height = source.Height;

            var bitmap = Bitmap.CreateBitmap(width,height,config);
            using(Canvas canvas = new Canvas(bitmap))
            using(Paint paint = new Paint()) {                
                canvas.DrawBitmap(source,0,0,paint);
                PorterDuffColorFilter cf = new PorterDuffColorFilter(Color.White,PorterDuff.Mode.SrcAtop);
                paint.SetColorFilter(cf);
                canvas.DrawBitmap(playCircle,width / 2 - playCircle.Width / 2,height / 2 - playCircle.Height / 2,paint);
            }
            return bitmap;
        }
    }
}