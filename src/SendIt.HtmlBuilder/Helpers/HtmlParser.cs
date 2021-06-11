// MIT License
//  
//  Copyright (c) 2021 Filip Liwiński
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
//

using System;
using System.Collections.Generic;

namespace SendIt.HtmlBuilder.Helpers
{
    public static class HtmlParser
    {
        public static ICollection<Node> Parse(string htmlString)
        {
            var nodes = new List<Node>();

            if (string.IsNullOrEmpty(htmlString))
            {
                throw new FormatException("Null or empty string cannot be parsed.");
            }
            if (htmlString[0] != '<' && htmlString[htmlString.Length - 1] != '>')
            {
                nodes.Add(new Text(htmlString));
                return nodes;
            }
            if (htmlString[0] != '<' || htmlString[htmlString.Length - 1] != '>')
            {
                throw new FormatException("Invalid syntax.");
            }

            var tag = GetFirstTag(htmlString);

            var elementString = GetFirstElement(htmlString);

            HtmlElement element;

            switch (tag)
            {
                case "p":
                    element = new P();
                    break;
                default:
                    throw new Exception("Not supported.");
            }

            element.InnerHtml = elementString;

            nodes.Add(element);

            return nodes;
        }

        public static string GetFirstTag(string htmlString)
        {
            return htmlString.Substring(1, htmlString.IndexOf('>') - 1).Split(' ')[0];
        }

        public static string GetFirstElement(string htmlString)
        {
            var tag = GetFirstTag(htmlString);

            var closeTag = $"</{tag}>";
            if (tag == "img" || tag == "br" || tag == "hr")
            {
                closeTag = ">";
            }

            return htmlString.Substring(0, htmlString.IndexOf(closeTag) + closeTag.Length);
        }

        public static string GetFirstElementContent(string htmlString)
        {
            var tag = GetFirstTag(htmlString);

            if (tag == "img" || tag == "br" || tag == "hr")
            {
                return "";
            }

            var closeTag = $"</{tag}>";

            return htmlString.Substring(htmlString.IndexOf('>') + 1, htmlString.IndexOf(closeTag));
        }
    }
}
