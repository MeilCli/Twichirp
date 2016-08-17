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
using Twichirp.Core.Constant;
using Twichirp.Core.TweetCount;
using static System.Console;

namespace Twichirp.Test.Console {
    class Program {

        static readonly string t1 = $"asa";
        static readonly string t2 = $"sffs"+t1;

        static void Main(string[] args) {
            {
                int count = "dshghigshkia".TweetCount();
                WriteLine($"count:{count} ?:12");
            }
            {
                int count = "afhyio google.com afhu http://google.com st".TweetCount();
                WriteLine($"count:{count} ?:62");
            }
            {
                int count = "afhyio google.com afhu https://google.com st".TweetCount();
                WriteLine($"count:{count} ?:62");
            }
            foreach(Match m in Core.TweetCount.Regex.ValidUrl.Matches("t.co google.com afhyio (google.com afhu http://google.com st")) {
                WriteLine(m.Groups[3].Value);
            }
            WriteLine(t2);
        }
    }
}
