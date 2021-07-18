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
    public class TD : HtmlElement
    {
        public int? ColSpan { get; set; }
        public string Headers { get; set; }
        public int? RowSpan { get; set; }

        public TD(string text = null, int? colSpan = null, string headers = null, int? rowSpan = null) : base(text)
        {
            ColSpan = colSpan;
            Headers = headers;
            RowSpan = rowSpan;
        }

        protected new void AttributesToHtml(StringBuilder sb)
        {

            if (ColSpan != null)
            {
                sb.Append($" colspan=\"{ColSpan}\"");
            }

            if (!string.IsNullOrEmpty(Headers))
            {
                sb.Append($" headers=\"{Headers}\"");
            }

            if (RowSpan != null)
            {
                sb.Append($" rowspan=\"{RowSpan}\"");
            }
        }

        public override StringBuilder ToHtml(StringBuilder sb)
        {
            OpenTag(sb);

            // Remove trailing '>'.
            sb.Remove(sb.Length - 1, 1);

            // Add custom attributes.
            AttributesToHtml(sb);

            // Add trailing '>'.
            sb.Append(">");

            // Add child nodes.
            foreach (var child in ChildNodes)
            {
                child.ToHtml(sb);
            }

            CloseTag(sb);

            return sb;
        }
    }
}
