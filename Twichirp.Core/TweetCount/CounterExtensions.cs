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
using System.Text.RegularExpressions;

namespace Twichirp.Core.TweetCount {
    public static class CounterExtensions {

        private const int httpUrlSize = 23;
        private const int httpsUrlSize = 23;

        public static int TweetCount(this string text) {
            if (text.Contains(".") == false) {
                return text.Length;
            }
            var regex = Regex.ValidUrl;
            var match = regex.Matches(text);
            int count = text.Length;
            foreach (Match m in match) {
                count -= m.Groups[3].Length;
                count += m.Groups[3].Value.StartsWith("https") ? httpsUrlSize : httpUrlSize;
            }
            return count;
        }
    }
}
