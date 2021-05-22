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

using System.Collections.Generic;
using System.Text;

namespace SendIt.HtmlBuilder
{
    public abstract class Node
    {
        private string nodeValue;
        public ICollection<Node> ChildNodes { get; private set; }
        public NodeType NodeType { get; private set; }
        public string NodeValue
        {
            get { return nodeValue; }
            set
            {
                switch (NodeType)
                {
                    case NodeType.ATTRIBUTE_NODE:
                    case NodeType.CDATA_SECTION_NODE:
                    case NodeType.COMMENT_NODE:
                    case NodeType.PROCESSING_INSTRUCTION_NODE:
                    case NodeType.TEXT_NODE:
                        nodeValue = value;
                        break;
                    case NodeType.DOCUMENT_FRAGMENT_NODE:
                    case NodeType.DOCUMENT_NODE:
                    case NodeType.DOCUMENT_TYPE_NODE:
                    case NodeType.ELEMENT_NODE:
                    case NodeType.ENTITY_NODE:
                    case NodeType.ENTITY_REFERENCE_NODE:
                    case NodeType.NOTATION_NODE:
                        nodeValue = null;
                        break;
                }
            }
        }

        protected Node(NodeType nodeType, string nodeValue)
        {
            NodeType = nodeType;
            NodeValue = nodeValue;
            ChildNodes = new List<Node>();
        }

        public Node AppendChild(Node node)
        {
            ChildNodes.Add(node);
            return this;
        }

        public abstract StringBuilder ToHtml(StringBuilder sb);

        public abstract StringBuilder ToText(StringBuilder sb);
    }
}
