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

using System.Text;

namespace SendIt.HtmlBuilder
{
    public abstract class HtmlElement : Element
    {
        public Style Style { get; set; } = new Style();

        protected HtmlElement(string nodeValue) : base(nodeValue) { }

        protected void OpenTag(StringBuilder sb)
        {
            if (TagName == "html")
            {
                sb.Append("<!DOCTYPE html>");
            }

            sb.Append($"<{TagName}");

            AttributesToHtml(sb);

            sb.Append(">");
        }

        protected void CloseTag(StringBuilder sb)
        {
            sb.Append($"</{TagName}>");
        }

        protected void AttributesToHtml(StringBuilder sb)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                sb.Append($" id=\"{Id}\"");
            }

            if (Style != null && Style.Count > 0)
            {
                sb.Append($" style=\"");

                foreach (var key in Style.Keys)
                {
                    sb.Append($"{key}".ToLower() + $": {Style[key]}; ");
                }

                // Remove the space after the last attribute.
                sb.Remove(sb.Length - 1, 1);

                sb.Append($"\"");
            }
        }
        
        public override StringBuilder ToHtml(StringBuilder sb)
        {
            OpenTag(sb);

            foreach (var child in ChildNodes)
            {
                child.ToHtml(sb);
            }

            CloseTag(sb);

            return sb;
        }

        public string ToHtml()
        {
            return ToHtml(new StringBuilder()).ToString();
        }

        public override StringBuilder ToText(StringBuilder sb)
        {
            foreach (var child in ChildNodes)
            {
                child.ToText(sb);
            }

            return sb;
        }

        public string ToText()
        {
            return ToText(new StringBuilder()).ToString();
        }
    }
}
