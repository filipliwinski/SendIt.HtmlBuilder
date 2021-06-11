using SendIt.HtmlBuilder.Helpers;
using System.Collections.Generic;
using Xunit;

namespace SendIt.HtmlBuilder.Tests.Helpers
{
    public class HtmlParserTests
    {
        [Fact]
        public void Parse()
        {
            var p = new P("test");
            //p.Style.Add(StyleProperty.Color, "#FFFFFF");
            var htmlString = p.ToHtml();
            var nodes = HtmlParser.Parse(htmlString) as List<Node>;

            Assert.IsType<P>(nodes[0]);
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
                    new H3(),
                    new Img("")
                },
                new List<HtmlElement>
                {
                    new H3(),
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
