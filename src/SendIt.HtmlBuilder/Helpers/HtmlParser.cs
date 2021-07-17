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
        private static readonly string[] selfClosingTags = new string[] { "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr" };

        public static ICollection<Node> Parse(string htmlString)
        {
            var nodes = new List<Node>();

            if (string.IsNullOrEmpty(htmlString))
            {
                throw new ArgumentNullException(htmlString, "Null or empty string cannot be parsed.");
            }
            if (htmlString[0] != '<' && htmlString[htmlString.Length - 1] != '>')
            {
                nodes.Add(new Text(htmlString));
                return nodes;
            }

            var tag = GetFirstTag(htmlString);

            var elementString = GetFirstElement(htmlString);

            var elementContent = GetElementContent(elementString);

            HtmlElement element = CreateHtmlElement(tag);

            element.InnerHtml = elementContent;

            element.Id = GetElementId(elementString);

            element.Style = GetElementStyle(elementString);

            if (element is Img img)
            {
                img.Src = GetElementAttribute("src", elementString);
                img.Alt = GetElementAttribute("alt", elementString);
                var height = GetElementAttribute("height", elementString);
                if (height != null)
                {
                    img.Height = int.Parse(height);
                }
                var width = GetElementAttribute("width", elementString);
                if (width != null)
                {
                    img.Width = int.Parse(width);
                }
            }

            if (element is TH th)
            {
                th.Abbr = GetElementAttribute("abbr", elementString);
                var colSpan = GetElementAttribute("colspan", elementString);
                if (colSpan != null)
                {
                    th.ColSpan = int.Parse(colSpan);
                }
                th.Headers = GetElementAttribute("headers", elementString);
                var rowSpan = GetElementAttribute("rowspan", elementString);
                if (rowSpan != null)
                {
                    th.RowSpan = int.Parse(rowSpan);
                }
                var scope = GetElementAttribute("scope", elementString);
                if (scope != null)
                {
                    th.Scope = (Scope)Enum.Parse(typeof(Scope), scope, ignoreCase: true); ;
                }
            }

            if (element is TD td)
            {
                var colSpan = GetElementAttribute("colspan", elementString);
                if (colSpan != null)
                {
                    td.ColSpan = int.Parse(colSpan);
                }
                td.Headers = GetElementAttribute("headers", elementString);
                var rowSpan = GetElementAttribute("rowspan", elementString);
                if (rowSpan != null)
                {
                    td.RowSpan = int.Parse(rowSpan);
                }
            }

            nodes.Add(element);

            var remainigHtmlString = htmlString.Substring(elementString.Length);

            if (!string.IsNullOrEmpty(remainigHtmlString))
            {
                var remainingNodes = Parse(remainigHtmlString);

                foreach (var node in remainingNodes)
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        public static string GetElementFullTag(string htmlString)
        {
            return htmlString.Substring(0, htmlString.IndexOf('>') + 1);
        }

        public static Style GetElementStyle(string htmlString)
        {
            var styleString = GetElementAttribute("style", htmlString);

            if (string.IsNullOrEmpty(styleString))
            {
                return null;
            }

            var stypeProperties = styleString.Split(';');

            var style = new Style();

            foreach (var s in stypeProperties)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    var property = s.Split(':');

                    style.Add(property[0], property[1]);
                }
            }

            return style;
        }

        public static string GetElementId(string htmlString)
        {
            return GetElementAttribute("id", htmlString);
        }

        public static string GetElementAttribute(string attribute, string htmlString)
        {
            var elementFullTag = GetElementFullTag(htmlString);

            var attributeIndex = elementFullTag.IndexOf($"{attribute}=");

            if (attributeIndex > 0)
            {
                var openingQuotationMarkIndex = attributeIndex + attribute.Length + 2;
                var closingQuotationMarkIndex = elementFullTag.IndexOf("\"", openingQuotationMarkIndex);
                return htmlString.Substring(openingQuotationMarkIndex, closingQuotationMarkIndex - openingQuotationMarkIndex);
            }

            return null;
        }

        public static HtmlElement CreateHtmlElement(string tag)
        {
            HtmlElement element;
            switch (tag.ToLower())
            {
                case "body":
                    element = new Body();
                    break;
                case "caption":
                    element = new Caption();
                    break;
                case "col":
                    element = new Col();
                    break;
                case "colgroup":
                    element = new ColGroup();
                    break;
                case "h1":
                    element = new H1();
                    break;
                case "h2":
                    element = new H2();
                    break;
                case "h3":
                    element = new H3();
                    break;
                case "h4":
                    element = new H4();
                    break;
                case "h5":
                    element = new H5();
                    break;
                case "h6":
                    element = new H6();
                    break;
                case "head":
                    element = new Head();
                    break;
                // TODO: support for parsing Html tag
                //case "html":
                //    element = new Html();
                //    break;
                case "img":
                    element = new Img("");
                    break;
                case "p":
                    element = new P();
                    break;
                case "table":
                    element = new Table();
                    break;
                case "tbody":
                    element = new TBody();
                    break;
                case "td":
                    element = new TD();
                    break;
                case "tfoot":
                    element = new TFoot();
                    break;
                case "th":
                    element = new TH();
                    break;
                case "thead":
                    element = new THead();
                    break;
                case "tr":
                    element = new TR();
                    break;
                default:
                    throw new NotSupportedException($"<{tag.ToLower()}> is not supported.");
            }

            return element;
        }

        public static string GetFirstTag(string htmlString)
        {
            if (!htmlString.Contains(">"))
            {
                throw new FormatException("Invalid syntax.");
            }
            return htmlString.Substring(1, htmlString.IndexOf('>') - 1).Split(' ')[0];
        }

        public static string GetFirstElement(string htmlString)
        {
            var tag = GetFirstTag(htmlString);

            var closeTag = $"</{tag}>";
            if (IsSelfClosingTag(tag))
            {
                closeTag = ">";
            }

            return htmlString.Substring(0, htmlString.IndexOf(closeTag) + closeTag.Length);
        }

        public static string GetElementContent(string htmlString)
        {
            var tag = GetFirstTag(htmlString);

            if (IsSelfClosingTag(tag))
            {
                return "";
            }

            var openTag = $"<{tag}";
            var openTagClose = htmlString.IndexOf('>');
            var closeTag = $"</{tag}>";

            if (!htmlString.StartsWith(openTag))
            {
                throw new FormatException("Opening tag is missing.");
            }

            if (!htmlString.EndsWith(closeTag))
            {
                throw new FormatException("Closing tag is missing.");
            }

            return htmlString.Substring(openTagClose + 1, htmlString.Length - openTagClose - 1 - closeTag.Length);
        }

        public static bool IsSelfClosingTag(string tag)
        {
            return Array.IndexOf(selfClosingTags, tag) != -1;
        }
    }
}
