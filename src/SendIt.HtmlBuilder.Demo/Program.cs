using System;

namespace SendIt.HtmlBuilder.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var body = new Body();
            body.AppendChild(new P("Hello ")).AppendChild(new P("World") { Id = "p-with-id" });
            body.AppendChild(new Img("https://picsum.photos/400/300", width: 400, height: 300));
            var html = new Html(new Head(), body);

            Console.WriteLine(html.ToHtml());
            Console.WriteLine(html.ToText());
        }
    }
}
