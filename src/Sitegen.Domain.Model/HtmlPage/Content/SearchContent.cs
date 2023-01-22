using Sitegen.Domain.Model.SearchDatabase;

namespace Sitegen.Domain.Model.HtmlPage.Content;

/// <summary>
/// 検索ページコンテンツ。
/// </summary>
public sealed class SearchContent : IMainContent
{
    /// <summary>リンク</summary>
    public static readonly string Link = "/search/";

    /// <summary></summary>
    public bool AllowIndex => false;

    /// <summary></summary>
    public bool AllowFollow => false;

    /// <summary>パーマリンク</summary>
    public string Permalink => Link;

    public string PageTitle => "検索";

    public IEnumerable<string> CssFileNames => new[] { "sitegen-list.css", "sitegen-search.css" };

    public string Build(HtmlTemplates templates)
    {
        return templates.SearchContentTemplate.Render(new
        {
            dbLinkFormatWithCacheBusting = SearchDatabaseChunk.DbLinkFormatWithCacheBusting,
            searchContentPermalink = Link,
            webFilesDirName = WebFilesCacheBusting.DirName,
        });
    }
}
