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
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;

namespace Twichirp.Android.Views {

    public class DividerItemDecoration : RecyclerView.ItemDecoration {

        private static readonly int[] attributes = new int[] { global::Android.Resource.Attribute.ListDivider };

        public const int Vertical = LinearLayoutManager.Vertical;
        public const int Horizontal = LinearLayoutManager.Horizontal;

        private int _orientation;
        public int Orientation {
            get {
                return _orientation;
            }
            set {
                if(value != Vertical && value != Horizontal) {
                    throw new ArgumentException("not support orientation type");
                }
                _orientation = value;
            }
        }

        public int? Size { get; set; }

        private Drawable divider;

        public DividerItemDecoration(Context context,int orientation = Vertical) {
            var array = context.ObtainStyledAttributes(attributes);
            divider = array.GetDrawable(0);
            array.Recycle();
            Orientation = orientation;
        }

        public override void OnDraw(Canvas cValue,RecyclerView parent,RecyclerView.State state) {
            if(Orientation == Vertical) {
                drawVertical(cValue,parent);
            } else {
                drawHorizontal(cValue,parent);
            }
        }

        private void drawVertical(Canvas c,RecyclerView parent) {
            int left = parent.PaddingLeft;
            int right = parent.Width - parent.PaddingRight;
            int childCount = parent.ChildCount;
            //最初の要素は表示しなくていい
            for(int i = 1;i < childCount;i++) {
                var child = parent.GetChildAt(i);
                var parameter = child.LayoutParameters as RecyclerView.LayoutParams;
                int top = child.Top + parameter.TopMargin;
                int bottom = top + (Size ?? divider.IntrinsicHeight);
                divider.SetBounds(left,top,right,bottom);
                divider.Draw(c);
            }
        }

        private void drawHorizontal(Canvas c,RecyclerView parent) {
            int top = parent.PaddingTop;
            int bottom = parent.Height - parent.PaddingBottom;
            int childCount = parent.ChildCount;
            //最初の要素は表示しなくていい
            for(int i = 1;i < childCount;i++) {
                var child = parent.GetChildAt(i);
                var parameter = child.LayoutParameters as RecyclerView.LayoutParams;
                int left = child.Left + parameter.LeftMargin;
                int right = left + (Size ?? divider.IntrinsicWidth);
                divider.SetBounds(left,top,right,bottom);
                divider.Draw(c);
            }
        }

        public override void GetItemOffsets(Rect outRect,AView view,RecyclerView parent,RecyclerView.State state) {
            if(Orientation == Vertical) {
                outRect.Top = Size ?? divider.IntrinsicHeight;
            } else {
                outRect.Left = Size ?? divider.IntrinsicWidth;
            }
        }
    }
}