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

namespace SendIt.HtmlBuilder.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Define HTML tags as C# objects.

            var body = new Body();

            var h4 = new H4("This is a level 4 heading.");
            var p = new P("This is a paragraph.");
            var img = new Img("https://picsum.photos/400/300");

            var table = new Table();
            var tr1 = new TR();
            var td1 = new TD("Cell 1");
            var td2 = new TD("Cell 2");
            var tr2 = new TR();
            var td3 = new TD("Cell 3");
            var td4 = new TD("Cell 4");

            #endregion
            #region Create HTML DOM.

            tr1.AppendChild(td1);
            tr1.AppendChild(td2);
            tr2.AppendChild(td3);
            tr2.AppendChild(td4);

            table.AppendChild(tr1);
            table.AppendChild(tr2);

            body.AppendChild(h4);
            body.AppendChild(p);
            body.AppendChild(img);
            body.AppendChild(table);

            var html = new Html(new Head(), body);

            #endregion
            #region Generate HTML string

            var htmlString = html.ToHtml();
            Console.WriteLine(htmlString);

            #endregion
            #region Use fluent methods

            var htmlFluent = new Html(new Head(), new Body()
                .AppendChild(new H4("This is a level 4 heading."))
                .AppendChild(new P("This is a paragraph."))
                .AppendChild(new Img("https://picsum.photos/400/300"))
                .AppendChild(new Table()
                    .AppendChild(new TR()
                        .AppendChild(new TD("Cell 1"))
                        .AppendChild(new TD("Cell 2")))
                    .AppendChild(new TR()
                        .AppendChild(new TD("Cell 3"))
                        .AppendChild(new TD("Cell 4")))) as Body);

            #endregion
            #region Define attributes and styling

            var h3 = new H3("This is a level 3 heading and it is blue")
            {
                Id = "Heading3",
                Style = new Style()
                    .Add(StyleProperty.Color, "blue")
                    .Add(StyleProperty.FontFamily, "Arial")
                    .Add(StyleProperty.FontSize, "0.9rem")
            };

            h3.Style[StyleProperty.FontWeight] = "600";

            var h3String = h3.ToHtml();
            Console.WriteLine(h3String);

            #endregion
        }
    }
}
