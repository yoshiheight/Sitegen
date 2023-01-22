namespace Sitegen.Domain.Model.HtmlPage.Content;

/// <summary>
/// トップページコンテンツ。
/// </summary>
public sealed class TopContent : IMainContent
{
    /// <summary></summary>
    public bool AllowIndex => true;

    /// <summary></summary>
    public bool AllowFollow => true;

    /// <summary>
    /// パーマリンク
    /// </summary>
    public string Permalink => "/";

    private readonly CategorizedArticleCollection _articles;

    public string PageTitle => string.Empty;

    public IEnumerable<string> CssFileNames => new[] { "sitegen-list.css" };

    public TopContent(CategorizedArticleCollection articles)
    {
        _articles = articles;
    }

    public string Build(HtmlTemplates templates)
    {
        return templates.ListContentTemplate.Render(new
        {
            articles = _articles,
            currentName = string.Empty,
        });
    }
}
