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
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;

namespace Twichirp.Android.App.View {
    public class DividerItemDecoration :RecyclerView.ItemDecoration {

        private int dividerHeght = 2;
        private int dividerColor = Color.Gray;

        public override void OnDraw(Canvas cValue,RecyclerView parent,RecyclerView.State state) {
            base.OnDraw(cValue,parent,state);
            int left = parent.PaddingLeft;
            int right = parent.Width - parent.PaddingRight;
            Paint paint = new Paint() {
                Color = new Color(dividerColor)
            };
            for(int i = 0;i < parent.ChildCount;i++) {
                var child = parent.GetChildAt(i);
                var param = child.LayoutParameters as RecyclerView.LayoutParams;
                int top = child.Bottom + param.BottomMargin;
                int bottom = top + dividerHeght;
                cValue.DrawRect(left,top,right,bottom,paint);
            }
        }

        public override void GetItemOffsets(Rect outRect,AView view,RecyclerView parent,RecyclerView.State state) {
            outRect.Set(0,0,0,dividerHeght);
        }
    }
}