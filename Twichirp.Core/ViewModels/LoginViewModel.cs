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
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Core.DataObjects;
using Twichirp.Core.DataRepositories;
using Twichirp.Core.Events;
using Twichirp.Core.Models;
using Twichirp.Core.Settings;

namespace Twichirp.Core.ViewModels {

    public class LoginViewModel : BaseViewModel {

        private LoginModel loginModel;

        public ReactiveProperty<string> Pin { get; } = new ReactiveProperty<string>();
        public ReadOnlyReactiveProperty<bool> IsLoading { get; }
        public AsyncReactiveCommand AuthorizeCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LoginCommand { get; } = new AsyncReactiveCommand();
        public ReactiveCommand<string> StartLoginWebPageCommand { get; } = new ReactiveCommand<string>();
        public ReactiveCommand StartNextPageCommand { get; } = new ReactiveCommand();
        public ReactiveCommand<string> ShowMessageCommand { get; } = new ReactiveCommand<string>();

        public LoginViewModel(IAccountRepository accountRepository, SettingManager settingManager, ImmutableClientKey clientKey) {
            loginModel = new LoginModel(accountRepository, settingManager);
            IsLoading = loginModel.ObserveProperty(x => x.IsLoading).ObserveOnUIDispatcher().ToReadOnlyReactiveProperty().AddTo(Disposable);

            Observable.FromEventPattern<EventArgs<string>>(x => loginModel.AuthorizeUriCreated += x, x => loginModel.AuthorizeUriCreated -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => StartLoginWebPageCommand.Execute(x.EventArgs.EventData))
                .AddTo(Disposable);
            Observable.FromEventPattern<EventArgs>(x => loginModel.LoginSucceeded += x, x => loginModel.LoginSucceeded -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => StartNextPageCommand.Execute())
                .AddTo(Disposable);
            Observable.FromEventPattern<EventArgs<string>>(x => loginModel.ErrorMessageCreated += x, x => loginModel.ErrorMessageCreated -= x)
                .ObserveOnUIDispatcher()
                .Subscribe(x => ShowMessageCommand.Execute(x.EventArgs.EventData))
                .AddTo(Disposable);
            AuthorizeCommand.Subscribe(x => loginModel.AuthorizeAsync(clientKey));
            LoginCommand.Subscribe(x => loginModel.LoginAsync(Pin.Value));
        }
    }
}
