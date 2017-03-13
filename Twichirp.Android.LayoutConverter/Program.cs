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
using System.IO;
using static System.Console;

namespace Twichirp.Android.LayoutConverter {

    public class Program {

        private static readonly string rootPath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string inputPath = Path.Combine(rootPath, "Input");
        private static readonly string outputPath = Path.Combine(rootPath, "Output");

        public static void Main(string[] args) {
            WriteLine("Start Program");
            var converter = new Converter();

            var inputDirectory = new DirectoryInfo(inputPath);
            Directory.CreateDirectory(outputPath);
            foreach (var file in inputDirectory.EnumerateFiles()) {
                WriteLine($"Read {file.Name}");
                string text;
                using (var fileStream = file.OpenRead())
                using (var stream = new StreamReader(fileStream)) {
                    text = stream.ReadToEnd();
                }

                WriteLine("Parsing...");
                text = converter.Convert(text);

                string path = Path.Combine(outputPath, file.Name);
                File.WriteAllText(path, text);
                WriteLine("Write");

            }
            WriteLine("End Program");
        }
    }
}
