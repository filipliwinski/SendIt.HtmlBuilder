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
using System.Text;

namespace SendIt.HtmlBuilder
{
    public class Style
    {
        readonly IDictionary<StyleProperty, string> styleProperties = new Dictionary<StyleProperty, string>();

        public int Count { get { return styleProperties.Count; } }
        public ICollection<StyleProperty> Keys { get { return styleProperties.Keys; } }
        public ICollection<string> Values { get { return styleProperties.Values; } }

        public Style Add(string property, string value)
        {
            return Add((StyleProperty)Enum.Parse(typeof(StyleProperty), property.Trim().Replace("-", ""), ignoreCase: true), value.Trim());
        }

        public Style Add(StyleProperty property, string value)
        {
            styleProperties[property] = value;

            return this;
        }

        public string this[StyleProperty index]
        {
            get => styleProperties[index];
            set => styleProperties[index] = value;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var property in styleProperties)
            {
                sb.Append($"{property.Key.ToString().ToLower()}: {property.Value}; ");
            }

            return sb.ToString().Trim();
        }
    }
}
