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
using System.Collections.Generic;
using System.Text;

namespace Twichirp.Android.LayoutConverter {

    class Converter {

        private static readonly string[] lineCodes = { "\r\n", "\r", "\n" };

        public string Convert(string input) {
            var elements = new List<Element>();
            Element currentElement = null;
            foreach (var line in input.Split(lineCodes, StringSplitOptions.None)) {
                if (line.IndexOf('<') >= 0) {
                    if (currentElement != null) {
                        elements.Add(currentElement);
                    }
                    currentElement = new Element();
                    currentElement.Lines.Add(line);
                } else {
                    if (currentElement == null) {
                        currentElement = new Element();
                    }
                    currentElement.Lines.Add(line);
                }
                if (line.IndexOf('>') >= 0) {
                    elements.Add(currentElement);
                    currentElement = null;
                }
            }
            if (currentElement != null) {
                elements.Add(currentElement);
            }
            var replaces = new List<ReplaceElement>();
            var sb = new StringBuilder();
            foreach (var e in elements) {
                var replace = parseReplaceElement(e);
                if (replace != null) {
                    replaces.Add(replace);
                    continue;
                }
                if (replaces.Count == 0) {
                    foreach (var line in e.Lines) {
                        sb.AppendLine(line);
                    }
                    continue;
                }
                foreach (var line in e.Lines) {
                    string newLine = line;
                    foreach (var r in replaces) {
                        newLine = newLine.Replace(r.FromText, r.ToText);
                    }
                    sb.AppendLine(newLine);
                }
                replaces.Clear();
            }

            return sb.ToString();
        }

        private ReplaceElement parseReplaceElement(Element element) {
            if (element.Lines.Count != 1) {
                return null;
            }

            string tag = element.Lines[0];

            string startTag = "<!-- ";
            int startIndex = tag.IndexOf(startTag);
            if (startIndex < 0) {
                return null;
            }
            startIndex = startIndex + startTag.Length;

            string endTag = " -->";
            int endIndex = tag.IndexOf(endTag, startIndex);
            if (endIndex < 0) {
                return null;
            }

            string target = tag.Substring(startIndex, endIndex - startIndex);
            if (target.IndexOf(':') < 0) {
                return null;
            }

            var targets = target.Split(':');
            return new ReplaceElement(targets[0], targets[1]);
        }

    }

    class Element {

        public List<string> Lines { get; } = new List<string>();

        public Element() { }
    }

    class ReplaceElement {

        public string FromText { get; }

        public string ToText { get; }

        public ReplaceElement(string fromText, string toText) {
            FromText = fromText;
            ToText = toText;
        }
    }
}
