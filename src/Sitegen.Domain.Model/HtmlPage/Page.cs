using System.Net;
using Sitegen.Domain.Model.HtmlPage.Content;

namespace Sitegen.Domain.Model.HtmlPage;

public sealed class Page
{
    public string Url { get; }
    public string Html { get; }

    private Page(IMainContent content, string html)
    {
        Url = content.Permalink + "index.html";
        Html = html;
    }

    public static Page Build(HtmlTemplates templates, IMainContent content, Categories categories)
    {
        var html = templates.PageTemplate.Render(new
        {
            builded = content.Build(templates),
            pageContent = content,
            categories = categories,
            cssFileNames = new[] { "sitegen.css" }.Concat(content.CssFileNames).ToArray(),
            pageTitle = WebUtility.HtmlEncode(content.PageTitle),
            tagsContentLink = TagsContent.Link,
            searchContentLink = SearchContent.Link,
            webFilesDirName = WebFilesCacheBusting.DirName,
        });

        return new(content, html);
    }
}
