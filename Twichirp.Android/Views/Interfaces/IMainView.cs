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

using SToolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using FFImageLoading.Views;
using BottomBarSharp;

namespace Twichirp.Android.Views.Interfaces {

    public interface IMainView : IView, ILifeCycleView {

        SToolbar Toolbar { get; }

        BottomBar BottomBar { get; }

        DrawerLayout DrawerLayout { get; }

        NavigationView Navigation { get; }

        CoordinatorLayout Coordinator { get; }

        FrameLayout IconClickable { get; }

        ImageViewAsync Icon { get; }

        FrameLayout FirstSubIconClickable { get; }

        ImageViewAsync FirstSubIcon { get; }

        FrameLayout SecondSubIconClickable { get; }

        ImageViewAsync SecondSubIcon { get; }

        RelativeLayout Subtitle { get; }

        TextView Name { get; }

        TextView ScreenName { get; }

        ImageView Drop { get; }

        ImageViewAsync Background { get; }
    }
}