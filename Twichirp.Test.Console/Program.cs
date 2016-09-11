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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreTweet;
using Twichirp.Core.Constant;
using Twichirp.Core.TweetCount;
using static System.Console;

namespace Twichirp.Test.Console {
    class Program {

        static readonly string t1 = $"asa";
        static readonly string t2 = $"sffs"+t1;

        static void Main(string[] args) {
            var session = OAuth.Authorize(ConsumerConstant.Twichirp.ConsumerKey,ConsumerConstant.Twichirp.ConsumerSecret);
            WriteLine(session.AuthorizeUri);
            var tokens = OAuth.GetTokens(session,ReadLine());
            
        }
    }
}
