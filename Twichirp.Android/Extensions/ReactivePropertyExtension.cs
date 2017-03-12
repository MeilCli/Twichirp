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
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace Twichirp.Android.Extensions {

    public static class ReactivePropertyExtension {

        public static IDisposable SetCommand<T>(this IObservable<T> self, AsyncReactiveCommand command) =>
            self.Where(x => command.CanExecute()).Subscribe(x => command.Execute());

        public static IDisposable SetCommand<T>(this IObservable<T> self, AsyncReactiveCommand<T> command) =>
            self.Where(x => command.CanExecute()).Subscribe(x => command.Execute(x));
    }
}