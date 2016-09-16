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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.Extensions {
    public static class StatusExtensions {

        public static bool IsValid(this Status status) {
            if(status.User == null) {
                return false;
            }
            if(status.User.IsValid() == false) {
                return false;
            }
            if(status.RetweetedStatus != null && status.RetweetedStatus.IsValid() == false) {
                return false;
            }
            if(status.QuotedStatus != null && status.QuotedStatus.IsValid() == false) {
                return false;
            }
            return true;
        }

        public static IEnumerable<Status> DeploymentStatus(this Status status) {
            yield return status;
            if(status.RetweetedStatus != null) {
                yield return status.RetweetedStatus;
            }
            if(status.RetweetedStatus?.QuotedStatus != null) {
                yield return status.RetweetedStatus.QuotedStatus;
            }
            if(status.QuotedStatus != null) {
                yield return status.QuotedStatus;
            }
        }

    }
}
