﻿// Copyright (c) 2016-2017 meil
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.App.ViewModel {
    public class LoadingViewModel : BaseViewModel {

        public ReactiveProperty<bool> IsLoaing { get; } = new ReactiveProperty<bool>();
        public ReactiveCommand LoadCommand { get; } = new ReactiveCommand();

        public LoadingViewModel(ITwichirpApplication application) : base(application) {
        }

        public override void Dispose() {
            base.Dispose();
            LoadCommand.Dispose();
        }
    }
}
