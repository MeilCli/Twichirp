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
using Microsoft.Practices.Unity;
using Twichirp.Core.App;
using Twichirp.Core.App.Service;
using Twichirp.Core.App.ViewModel;
using Twichirp.Core.DataObjects;
using CStatus = CoreTweet.Status;

namespace Twichirp.Android.App.ViewModel {

    /// <summary>
    /// Recommend to use UnityContainer
    /// </summary>
    public class ImageViewerViewModel : StatusViewModel {

        public const string TransitionName = "ImageViewer_transition";
        private const string constructorStatus = "status";
        private const string constructorAccount = "account";
        private const string constructorDefaultPage = "defaultPage";

        public new static void Register(UnityContainer unityContainer) {
            unityContainer.RegisterType<ImageViewerViewModel>();
        }

        public static ImageViewerViewModel Resolve(UnityContainer unityContainer, CStatus status, ImmutableAccount account, int defaultPage) {
            return unityContainer.Resolve<ImageViewerViewModel>(
                new ParameterOverride(constructorStatus,status),
                new ParameterOverride(constructorAccount,account),
                new ParameterOverride(constructorDefaultPage,defaultPage)
            );
        }

        public int DefaultPage { get; }

        public ImageViewerViewModel(ITwichirpApplication application,ITwitterEventService twitterEventService,CStatus status,ImmutableAccount account,int defaultPage) 
            : base(application,twitterEventService,status,account) {
            DefaultPage = defaultPage;
        }
    }
}