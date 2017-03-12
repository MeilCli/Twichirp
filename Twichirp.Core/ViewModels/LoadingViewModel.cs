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
using Twichirp.Core.Models;

namespace Twichirp.Core.ViewModels {

    public class LoadingViewModel : BaseViewModel {

        public ReadOnlyReactiveProperty<bool> IsLoading { get; }
        public ReactiveCommand LoadCommand { get; } = new ReactiveCommand();
        internal LoadingModel LoadingModel { get; }

        public LoadingViewModel(LoadingModel model) {
            LoadingModel = model;
            IsLoading = LoadingModel.ObserveProperty(x => x.IsLoading).ToReadOnlyReactiveProperty().AddTo(Disposable);
        }

        public override void Dispose() {
            base.Dispose();
            LoadCommand.Dispose();
        }
    }
}
