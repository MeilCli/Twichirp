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
using CoreTweet;
using Twichirp.Core.App;
using Twichirp.Core.App.ViewModel;
using Twichirp.Core.Model;

namespace Twichirp.Android.App.ViewModel {

    public class ImageViewerViewModel : StatusViewModel {

        public const string TransitionName = "ImageViewer_transition";

        public int DefaultPage { get; }

        public ImageViewerViewModel(ITwichirpApplication application,Status status,Account account,int defaultPage) : base(application,status,account) {
            DefaultPage = defaultPage;
        }
    }
}