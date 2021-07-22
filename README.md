# SendIt.HtmlBuilder

Build HTML code with C# using fluent methods.

[![Build Status](https://filipliwinski.visualstudio.com/GitHub.Public/_apis/build/status/SendIt.HtmlBuilder/SendIt.HtmlBuilder.Build.CI?repoName=filipliwinski%2FSendIt.HtmlBuilder&branchName=master)](https://filipliwinski.visualstudio.com/GitHub.Public/_build/latest?definitionId=11&repoName=filipliwinski%2FSendIt.HtmlBuilder&branchName=master)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=filipliwinski_SendIt.HtmlBuilder&metric=coverage)](https://sonarcloud.io/dashboard?id=filipliwinski_SendIt.HtmlBuilder)

Download from [NuGet](https://www.nuget.org/packages/SendIt.HtmlBuilder/).

[![NuGet](https://img.shields.io/nuget/v/SendIt.HtmlBuilder.svg)](https://www.nuget.org/packages/SendIt.HtmlBuilder/)
[![NuGet](https://img.shields.io/nuget/dt/SendIt.HtmlBuilder.svg)](https://www.nuget.org/packages/SendIt.HtmlBuilder/) 

## How to use

### Define HTML tags as C# objects

```csharp
var html = new Html(new Head(), new Body());

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
```

### Create HTML DOM

```csharp
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
```

### Generate HTML string

```csharp
var htmlString = html.ToHtml();
Console.WriteLine(htmlString);
```

Output (formatted manually for readability):

```html
<!DOCTYPE html>
<html>
    <head></head>
    <body>
        <h4>This is a level 4 heading.</h4>
        <p>This is a paragraph.</p>
        <img alt="" src="https://picsum.photos/400/300">
        <table>
            <tr>
                <td>Cell 1</td>
                <td>Cell 2</td>
            </tr>
            <tr>
                <td>Cell 3</td>
                <td>Cell 4</td>
            </tr>
        </table>
    </body>
</html>
```

### Use fluent methods

```csharp
var htmlFluent = new Html(
    new Head(),
    new Body()
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
```

### Define attributes and styling

```csharp
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
```

Output:

```html
<h3 id="Heading3" style="color: blue; font-family: Arial; font-size: 0.9rem; font-weight: 600;">This is a level 3 heading and it is blue</h3>
```

### Parse HTML

```csharp
var htmlStringToParse = "<div><h3>Hello World!</h3><p>This HTML code can be parsed by HtmlParser.</p><div>";

var htmlParsed = HtmlParser.Parse(htmlStringToParse);
```
