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
using System;
using System.Collections.Generic;
using Xunit;

namespace SendIt.HtmlBuilder.Tests.Helpers
{
    public class HtmlParserTests
    {
        [Fact]
        public void WhenProvidedEmptyString_ThenThrowsArgumentNullException()
        {
            var input = "";

            Assert.Throws<ArgumentNullException>(() => HtmlParser.Parse(input));
        }

        [Theory]
        [InlineData("<This is not a valid HTML code.>")]
        [InlineData("<This is not a valid HTML code.")]
        [InlineData("p>This is not a valid HTML code.</p>")]
        [InlineData("<p>This is not a valid HTML code.<p>")]
        [InlineData("</p>This is not a valid HTML code.</p>")]
        [InlineData("<h1>This is not a valid HTML code.</h2>")]
        public void WhenSyntaxIsInvalid_ThenThrowsFormatException(string input)
        {
            Assert.Throws<FormatException>(() => HtmlParser.Parse(input));
        }

        [Fact]
        public void ParsePWithText()
        {
            var p = new P("test");
            p.Style.Add(StyleProperty.Color, "#FFFFFF");
            p.Id = "id";
            var nodes = HtmlParser.Parse(p.ToHtml()) as List<Node>;

            var parsedP = Assert.IsType<P>(nodes[0]);
            Assert.Equal(p.Style[StyleProperty.Color], parsedP.Style[StyleProperty.Color]);
            Assert.Equal(p.InnerHtml, parsedP.InnerHtml);
            Assert.Equal(p.Id, parsedP.Id);
        }

        [Fact]
        public void ParseBodyWithPWithTextAndImg()
        {
            var p = new P("test");
            p.Style.Add(StyleProperty.Color, "#FFFFFF");
            p.Id = "id";
            var body = new Body();
            body.AppendChild(p).AppendChild(new Img("https://picsum.photos/400/300"));

            var nodes = HtmlParser.Parse(body.ToHtml()) as List<Node>;

            var parsedBody = Assert.IsType<Body>(nodes[0]);
            Assert.Equal(body.InnerHtml, parsedBody.InnerHtml);
        }

        [Fact]
        public void ParseBodyWithHeadings()
        {
            var h1 = new H1("test");
            h1.Style.Add(StyleProperty.Color, "#000000");
            h1.Id = "H1";
            var h2 = new H2("test");
            h2.Style.Add(StyleProperty.Color, "#00FF00");
            h2.Id = "H2";
            var h3 = new H3("test");
            h3.Style.Add(StyleProperty.Color, "#FF0000");
            h3.Id = "H3";
            var h4 = new H4("test");
            h4.Style.Add(StyleProperty.Color, "#0000FF");
            h4.Id = "H4";
            var h5 = new H5("test");
            h5.Style.Add(StyleProperty.Color, "#0F0F0F");
            h5.Id = "H5";
            var h6 = new H6("test");
            h6.Style.Add(StyleProperty.Color, "#F0F0F0");
            h6.Id = "H6";
            var body = new Body();
            body.AppendChild(h1)
                .AppendChild(h2)
                .AppendChild(h3)
                .AppendChild(h4)
                .AppendChild(h5)
                .AppendChild(h6);

            var nodes = HtmlParser.Parse(body.ToHtml()) as List<Node>;

            var parsedBody = Assert.IsType<Body>(nodes[0]);
            Assert.Equal(body.InnerHtml, parsedBody.InnerHtml);
        }

        [Fact]
        public void ParseTableWithHeaderBodyAndFooter()
        {
            var table = new Table()
                .AppendChild(new THead()
                    .AppendChild(new TR()
                        .AppendChild(new TH("Col 1 header"))
                        .AppendChild(new TH("Col 2 header"))))
                .AppendChild(new TBody()
                    .AppendChild(new TR()
                        .AppendChild(new TH("Row 1 header"))
                        .AppendChild(new TD("Col 2 row 1 content")))
                    .AppendChild(new TR()
                        .AppendChild(new TH("Row 2 content"))
                        .AppendChild(new TD("Col 2 row 2 content"))))
                .AppendChild(new TFoot()
                    .AppendChild(new TR()
                        .AppendChild(new TH("Row 3 header"))
                        .AppendChild(new TD("Col 2 row 3 content")))) as Table;

            var nodes = HtmlParser.Parse(table.ToHtml()) as List<Node>;

            var parsedTable = Assert.IsType<Table>(nodes[0]);
            Assert.Equal(table.InnerHtml, parsedTable.InnerHtml);
        }

        [Fact]
        public void ParseTableWithCaptionAndColGroup()
        {
            var table = new Table()
                .AppendChild(new Caption("Caption"))
                .AppendChild(new ColGroup()
                    .AppendChild(new Col())
                    .AppendChild(new Col()))
                .AppendChild(new TR()
                    .AppendChild(new TH("Col 1 header", scope: Scope.Col))
                    .AppendChild(new TH("Col 2 header", scope: Scope.Col)))
                .AppendChild(new TR()
                    .AppendChild(new TD("Col 1 row 2 content"))
                    .AppendChild(new TD("Col 2 row 2 content"))) as Table;

            var nodes = HtmlParser.Parse(table.ToHtml()) as List<Node>;

            var parsedTable = Assert.IsType<Table>(nodes[0]);
            Assert.Equal(table.InnerHtml, parsedTable.InnerHtml);
        }

        [Fact]
        public void GetFirstTag()
        {
            var nodes = new List<HtmlElement>
            {
                new P("test"),
                new H3(),
                new Img("")
            };

            foreach (var node in nodes)
            {
                Assert.Equal(node.GetType().Name.ToLower(), HtmlParser.GetFirstTag(node.ToHtml()));
            }
        }

        [Fact]
        public void GetFirstElement()
        {
            var examples = new List<List<HtmlElement>>()
            {
                new List<HtmlElement>
                {
                    new P("test"),
                    new H1(),
                    new Img("")
                },
                new List<HtmlElement>
                {
                    new H2(),
                    new Img(""),
                    new P("test")
                },
                new List<HtmlElement>
                {
                    new Img(""),
                    new P("test"),
                    new H3()
                },
            };
            
            foreach (var nodes in examples)
            {
                var nodesString = "";

                foreach (var node in nodes)
                {
                    nodesString += node.ToHtml();
                }

                Assert.Equal(nodes[0].ToHtml(), HtmlParser.GetFirstElement(nodesString));
            }
        }

        [Fact]
        public void WhenElmentStyleIsNull_ThenReturnsNull()
        {
            var element = new P("Text");

            var elementString = element.ToHtml();

            var style = HtmlParser.GetElementStyle(elementString);

            Assert.Null(style);
        }

        [Fact]
        public void WhenElmentStyleIsDefinedAsStyleProperties_ThenReturnsElementStyle()
        {
            var element = new P("Text");
            element.Style.Add(StyleProperty.Color, "#000000");
            element.Style.Add(StyleProperty.Background, "#FFFFFF");
            element.Style.Add(StyleProperty.BorderBottom, "#FFFFFF solid 1px");

            var elementString = element.ToHtml();

            var style = HtmlParser.GetElementStyle(elementString);

            Assert.NotNull(element.Style);
            Assert.Equal(element.Style.ToString(), style.ToString());
        }

        [Fact]
        public void WhenElmentStyleIsDefinedAsLowercasedStrings_ThenReturnsElementStyle()
        {
            var element = new P("Text");
            element.Style.Add("color", "#000000");
            element.Style.Add("background", "#FFFFFF");
            element.Style.Add("border-bottom", "#FFFFFF solid 1px");

            var elementString = element.ToHtml();

            var style = HtmlParser.GetElementStyle(elementString);

            Assert.NotNull(element.Style);
            Assert.Equal(element.Style.ToString(), style.ToString());
        }

        [Fact]
        public void WhenElmentStyleIsDefinedAsStringsWithCapitalLetters_ThenReturnsElementStyle()
        {
            var element = new P("Text");
            element.Style.Add("Color", "#000000");
            element.Style.Add("Background", "#FFFFFF");
            element.Style.Add("Border-Bottom", "#FFFFFF solid 1px");

            var elementString = element.ToHtml();

            var style = HtmlParser.GetElementStyle(elementString);

            Assert.NotNull(element.Style);
            Assert.Equal(element.Style.ToString(), style.ToString());
        }

        [Fact]
        public void WhenElementIdIsNull_ThenReturnsNull()
        {
            var element = new P("Text");

            var elementString = element.ToHtml();

            var id = HtmlParser.GetElementId(elementString);

            Assert.Null(element.Id);
            Assert.Equal(element.Id, id);
        }

        [Fact]
        public void WhenElementIdIsNotNull_ThenReturnsElementId()
        {
            var element = new P("Text")
            {
                Id = "id"
            };

            var elementString = element.ToHtml();

            var id = HtmlParser.GetElementId(elementString);

            Assert.NotNull(element.Id);
            Assert.Equal(element.Id, id);
        }

        [Fact]
        public void WhenElementOpeningTagIsMissing_ThenThrowsFormatException()
        {
            var input = "This is a paragraph.</p>";

            Assert.Throws<FormatException>(() => HtmlParser.GetElementContent(input));
        }

        [Fact]
        public void WhenElementClosingTagIsMissing_ThenThrowsFormatException()
        {
            var input = "<p>This is a paragraph.";

            Assert.Throws<FormatException>(() => HtmlParser.GetElementContent(input));
        }

        [Fact]
        public void WhenElementIsPAndContentIsText_ThenReturnsText()
        {
            var elementContent = "This is a paragraph.";
            var element = new P(elementContent);
            var elementString = element.ToHtml();

            var result = HtmlParser.GetElementContent(elementString);

            Assert.Equal(elementContent, result);
        }

        [Fact]
        public void WhenElementIsH3AndContentIsText_ThenReturnsText()
        {
            var elementContent = "This is a level 3 heading";
            var element = new H3(elementContent);

            var elementString = element.ToHtml();

            var result = HtmlParser.GetElementContent(elementString);

            Assert.Equal(elementContent, result);
        }

        [Fact]
        public void WhenElementIsImg_ThenReturnsEmptyString()
        {
            var element = new Img("https://picsum.photos/400/300");

            var elementString = element.ToHtml();

            var result = HtmlParser.GetElementContent(elementString);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void WhenElementIsBodyAndContentIsP_ThenReturnsP()
        {
            var element = new Body();
            var p = new P("This is a paragraph.");
            var elementContent = p.ToHtml();

            element.AppendChild(p);

            var elementString = element.ToHtml();

            var result = HtmlParser.GetElementContent(elementString);

            Assert.Equal(elementContent, result);
        }

        [Theory]
        [InlineData("area")]
        [InlineData("base")]
        [InlineData("br")]
        [InlineData("col")]
        [InlineData("embed")]
        [InlineData("hr")]
        [InlineData("img")]
        [InlineData("input")]
        [InlineData("link")]
        [InlineData("meta")]
        [InlineData("param")]
        [InlineData("source")]
        [InlineData("track")]
        [InlineData("wbr")]
        public void IsSelfClosingTag(string tag)
        {
            Assert.True(HtmlParser.IsSelfClosingTag(tag));
        }

        [Theory]
        [InlineData("body")]
        [InlineData("div")]
        [InlineData("span")]
        [InlineData("p")]
        [InlineData("table")]
        public void IsNotSelfClosingTag(string tag)
        {
            Assert.False(HtmlParser.IsSelfClosingTag(tag));
        }

        //[Fact]
        //public void GetFirstElementContent()
        //{
        //    var list = new List<HtmlElement>
        //        {
        //            new P("test"),
        //            new H3(),
        //            new Img("")
        //        };

        //    foreach (var nodes in list)
        //    {
        //        var nodesString = "";

        //        foreach (var node in nodes)
        //        {
        //            nodesString += node.ToHtml();
        //        }

        //        Assert.Equal(nodes[0].ToHtml(), HtmlParser.GetFirstElement(nodesString));
        //    }
        //}
    }
}
