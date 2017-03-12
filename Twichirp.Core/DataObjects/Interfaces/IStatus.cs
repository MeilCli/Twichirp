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

namespace Twichirp.Core.DataObjects.Interfaces {

    interface IStatus<TStatus, TUser> {

        string Key { get; }

        long Id { get; }

        long UserId { get; }

        TUser OwnerUser { get; }

        TUser User { get; }

        TStatus RetweetedStatus { get; }

        TStatus QuotedStatus { get; }

        DateTimeOffset UpdatedAt { get; }

        DateTimeOffset CreatedAt { get; }

        string Json { get; }
    }
}
