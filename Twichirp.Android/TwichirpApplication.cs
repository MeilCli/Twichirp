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
using Twichirp.Core.Constants;
using Microsoft.Practices.Unity;
using Twichirp.Core.UnityExtensions;
using Twichirp.Core.DataObjects;
using Twichirp.Android.UnityExtensions;
using Twichirp.Core.Settings;
using Twichirp.Core;

namespace Twichirp.Android {

    [Application(Icon = "@drawable/icon",Label = "Twichirp",Theme = "@style/AppTheme",LargeHeap = true)]
    public class TwichirpApplication : Application, ITwichirpApplication {

        public UnityContainer UnityContainer { get; private set; }

        public TwichirpApplication(IntPtr javaReference,JniHandleOwnership transfer) : base(javaReference,transfer) {
        }

        public override void OnCreate() {
            base.OnCreate();
            UnityContainer = new UnityContainer();
            initUnity();
            var settingManager = Resolve<SettingManager>();
            settingManager.Migrate();
        }

        private void initUnity() {
            UnityContainer.RegisterInstance(ClientKeyConstant.TwichirpForAndroid);

            UnityContainer.AddNewExtension<ServiceRegister>();
            UnityContainer.AddNewExtension<AndroidServiceRegister>();
            UnityContainer.AddNewExtension<DataRepositoryRegister>();
            UnityContainer.AddNewExtension<SettingRegister>();
            UnityContainer.AddNewExtension<ViewModelRegister>();
            UnityContainer.AddNewExtension<AndroidViewModelRegister>();
        }

        public T Resolve<T>() {
            return UnityContainer.Resolve<T>();
        }

        //気休め
        ~TwichirpApplication() {
            UnityContainer.Dispose();
        }

    }
}