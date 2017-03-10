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
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.App.Model;
using Twichirp.Core.DataRepositories;
using Twichirp.Core.Settings;

namespace Twichirp.Core.App.ViewModel {

    public class SplashViewModel : BaseViewModel {

        private SplashModel splashModel;
        public ReadOnlyReactiveProperty<bool> IsRunning { get; }
        public ReadOnlyReactiveProperty<bool> IsAccountExist { get; }
        public ReadOnlyReactiveProperty<string> Message { get; }
        public ReactiveCommand StartLoginPageCommand { get; } = new ReactiveCommand();
        public ReactiveCommand StartMainPageCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ApplicationInitCommand { get; } = new ReactiveCommand();

        public SplashViewModel(ITwichirpApplication application,IAccountRepository accountRepository,SettingManager settingManager) : base(application) {
            this.splashModel = new SplashModel(application,accountRepository,settingManager);
            IsRunning = splashModel.ObserveProperty(x => x.IsRunning).ToReadOnlyReactiveProperty().AddTo(Disposable);
            IsAccountExist = splashModel.ObserveProperty(x => x.IsAccountExist).ToReadOnlyReactiveProperty().AddTo(Disposable);
            Message = splashModel.ObserveProperty(x => x.Message).ToReadOnlyReactiveProperty().AddTo(Disposable);

            Observable.FromEventPattern<EventArgs>(x => splashModel.ApplicationInitialized += x,x => splashModel.ApplicationInitialized -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => startNextPage())
                .AddTo(Disposable);
            ApplicationInitCommand.Subscribe(async x => await splashModel.ApplicationInitAsync());
        }

        private void startNextPage() {
            if(IsAccountExist.Value) {
                StartMainPageCommand.Execute();
            } else {
                StartLoginPageCommand.Execute();
            }
        }
    }
}
