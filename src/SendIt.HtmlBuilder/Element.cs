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

using SendIt.HtmlBuilder.Helpers;
using System.Text;

namespace SendIt.HtmlBuilder
{
    public abstract class Element : Node
    {
        public string Id { get; set; }

        public string InnerHtml {
            get {
                var htmlString = new StringBuilder();

                foreach (var node in ChildNodes)
                {
                    node.ToHtml(htmlString);
                }

                return htmlString.ToString();
            }
            set {
                ChildNodes.Clear();

                if (!string.IsNullOrEmpty(value))
                {
                    var newChildNodes = HtmlParser.Parse(value);

                    foreach (var node in newChildNodes)
                    {
                        AppendChild(node);
                    }
                }
            }
        }

        public string TagName
        {
            get { return GetType().Name.ToLower(); }
        }

        protected Element(string nodeValue) : base(NodeType.ELEMENT_NODE, nodeValue)
        {
            if (!string.IsNullOrEmpty(nodeValue))
            {
                AppendChild(new Text(nodeValue));
            }
        }
    }
}
