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
using CoreTweet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.App.Model;

namespace Twichirp.Core.App.ViewModel {
    public class StatusViewModel : BaseViewModel {

        public const int NormalTweet = 1;

        private StatusModel statusModel;
        public ReadOnlyReactiveProperty<long> Id { get; private set; }
        public ReadOnlyReactiveProperty<IEnumerable<TextPart>> Text { get; private set; }

        public StatusViewModel(ITwichirpApplication application,Status status) : base(application) {
            statusModel = new StatusModel(Application,status);
            Id = statusModel.ObserveProperty(x => x.Id).ToReadOnlyReactiveProperty().AddTo(Disposable);
            Text = statusModel.ObserveProperty(x => x.Text).ToReadOnlyReactiveProperty().AddTo(Disposable);
        }

        private int toStatusType() {
            return NormalTweet;
        }
    }
}
