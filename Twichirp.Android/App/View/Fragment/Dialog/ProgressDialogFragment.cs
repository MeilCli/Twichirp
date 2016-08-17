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
using Android.Util;
using Android.Views;
using Android.Widget;
using SDialogFragment = Android.Support.V4.App.DialogFragment;

namespace Twichirp.Android.App.View.Fragment.Dialog {
    public class ProgressDialogFragment : SDialogFragment {

        public ProgressDialogFragment() {
        }

        public const string FragmentTag = "ProgressDialogFragment";

        public static ProgressDialogFragment NewInstance(string title,string message) {
            var fragment = new ProgressDialogFragment();
            var bundle = new Bundle();
            bundle.PutString("title",title);
            bundle.PutString("message",message);
            fragment.Arguments = bundle;
            return fragment;
        }

        public override global::Android.App.Dialog OnCreateDialog(Bundle savedInstanceState) {
            var progressDialog = new ProgressDialog(Activity);
            string title = Arguments.GetString("title",string.Empty);
            string message = Arguments.GetString("message",string.Empty);
            progressDialog.SetTitle(title);
            progressDialog.SetMessage(message);
            progressDialog.SetCancelable(false);
            progressDialog.SetCanceledOnTouchOutside(false);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            return progressDialog;
        }
    }
}