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
using Android.Support.V7.Widget;
using Android.Util;
using Android.Graphics;
using Android.Graphics.Drawables;
using static Android.Graphics.PorterDuff;

namespace Twichirp.Android.App.View {
    public class CircleImageView :AppCompatImageView{

        //‚Ç‚±‚Å‰ð•ú‚µ‚æ‚¤‚©
        private Bitmap bitmap;
        private int accentColor;
        private static readonly int[] attr = { global::Android.Resource.Attribute.ColorAccent };

        public CircleImageView(Context context) :
            this(context,null) {
        }

        public CircleImageView(Context context,IAttributeSet attrs) :
            this(context,attrs,global::Android.Resource.Attribute.AutoCompleteTextViewStyle) {
        }

        public CircleImageView(Context context,IAttributeSet attrs,int defStyle) :
            base(context,attrs,defStyle) {
            TintTypedArray a = TintTypedArray.ObtainStyledAttributes(context,attrs,attr,defStyle,0);
            accentColor =  a.GetColor(0,Color.Aqua);
            a.Recycle();
        }

        public override void SetImageDrawable(Drawable drawable) {
            setCircleBitmap(((BitmapDrawable)drawable).Bitmap);
        }

        public override void SetImageBitmap(Bitmap bm) {
            setCircleBitmap(bm);
        }

        public override void SetImageResource(int resId) {
            setCircleBitmap(BitmapFactory.DecodeResource(Resources,resId));
        }

        private void setCircleBitmap(Bitmap bm) {
            if(this.bitmap != null) {
                this.bitmap.Recycle();
                this.bitmap = null;
            }

            int size = Math.Min(bm.Height,bm.Width);

            var paint = new Paint();
            paint.AntiAlias = true;
            paint.Color = new Color(accentColor);
            var rect = new Rect(0,0,size,size);
            bitmap = Bitmap.CreateBitmap(size,size,Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(bitmap);
            canvas.DrawARGB(0,0,0,0);
            canvas.DrawCircle(size / 2,size / 2, size / 2,paint);
            paint.SetXfermode(new PorterDuffXfermode(Mode.SrcIn));
            canvas.DrawBitmap(bm,rect,rect,paint);

            base.SetImageDrawable(new BitmapDrawable(Resources,bitmap));
        }

    }
}