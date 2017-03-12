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

using Android.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Widget;

namespace Twichirp.Android.Views {

    public class AppCompatAutoCompleteTextView : AutoCompleteTextView {

        private static readonly int[] attr = { global::Android.Resource.Attribute.Background, global::Android.Resource.Attribute.PopupBackground };

        public AppCompatAutoCompleteTextView(Context context) :
            this(context, null) {
        }

        public AppCompatAutoCompleteTextView(Context context, IAttributeSet attrs) :
            this(context, attrs, global::Android.Resource.Attribute.AutoCompleteTextViewStyle) {
        }

        public AppCompatAutoCompleteTextView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle) {
            TintTypedArray a = TintTypedArray.ObtainStyledAttributes(context, attrs, attr, defStyle, 0);
            SetBackgroundResource(a.GetResourceId(0, global::Android.Resource.Attribute.Background));
            SetDropDownBackgroundDrawable(a.GetDrawable(1));
            a.Recycle();
        }
    }
}