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
using Microsoft.Practices.Unity;

namespace Twichirp.Core {

    public interface ITwichirpApplication {

        /// <summary>
        /// Gets the unity container.
        /// Unity container must registers with IRegister on UnityExtensions namespace
        /// </summary>
        /// <value>The unity container.</value>
        UnityContainer UnityContainer { get; }

        T Resolve<T>();

    }
}
