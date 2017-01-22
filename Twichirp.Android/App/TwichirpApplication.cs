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
using Twichirp.Core.App;
using Twichirp.Core.App.Manager;
using Twichirp.Core.App.Setting;
using Twichirp.Android.App.Manager;
using Twichirp.Core.Constant;

namespace Twichirp.Android.App {
    [Application(Icon = "@drawable/icon",Label ="Twichirp",Theme = "@style/AppTheme",LargeHeap = true)]
    public class TwichirpApplication :Application,ITwichirpApplication{

        public SettingManager SettingManager { get; private set; }

        public FileManager FileManager { get; private set; }

        public DatabaseManager DatabaseManager { get; private set; }

        public AccountManager AccountManager { get; private set; }

        public ConsumerManager ConsumerManager { get; private set; }

        public IStringResource StringResource { get; private set; }

        public UserContainerManager UserContainerManager { get; private set; }

        public TwitterEvent TwitterEvent { get; private set; }

        public TwichirpApplication(IntPtr javaReference,JniHandleOwnership transfer) : base(javaReference,transfer) {
        }

        public override void OnCreate() {
            base.OnCreate();
            SettingManager = new SettingManager(this);
            SettingManager.Migrate();
            FileManager = new FileManager(new FileSystem());
            DatabaseManager = new DatabaseManager(this,new DatabaseSystem());
            AccountManager = new AccountManager(this);
            ConsumerManager = new ConsumerManager(this,ConsumerConstant.TwichirpForAndroid);
            UserContainerManager = new UserContainerManager(this);
            StringResource = new StringResource();
            TwitterEvent = new TwitterEvent();
        }

        //気休め
        ~TwichirpApplication() {
            DatabaseManager.Dispose();
        }

        public string GetLocalizedString(int resource) => GetString(resource);
    }
}