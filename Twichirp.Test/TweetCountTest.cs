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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twichirp.Core.TweetCount;

namespace Twichirp.Test {
    [TestClass]
    public class TweetCountTest {
        [TestMethod]
        public void TestMethod1() {
            int count = "dshghigshkia".TweetCount();
            Assert.AreEqual(count,12);
        }

        [TestMethod]
        public void TestMethod2() {
            int count = "afhyio google.com afhu http://google.com st".TweetCount();
            Assert.AreEqual(count,62);
        }

        [TestMethod]
        public void TestMethod3() {
            int count = "afhyio google.com afhu https://google.com st".TweetCount();
            Assert.AreEqual(count,62);
        }
    }
}
