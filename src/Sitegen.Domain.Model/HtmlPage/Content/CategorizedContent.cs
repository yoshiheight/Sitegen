namespace Sitegen.Domain.Model.HtmlPage.Content;

/// <summary>
/// タグページコンテンツ。
/// </summary>
public sealed class CategorizedContent : IMainContent
{
    /// <summary></summary>
    public bool AllowIndex => false;

    /// <summary></summary>
    public bool AllowFollow => true;

    /// <summary>パーマリンク</summary>
    public string Permalink => _articles.Category.Permalink;

    private readonly CategorizedArticleCollection _articles;

    public string PageTitle { get; }

    public IEnumerable<string> CssFileNames => new[] { "sitegen-list.css" };

    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public CategorizedContent(CategorizedArticleCollection group)
    {
        _articles = group;
        PageTitle = "カテゴリ " + _articles.Category.Name;
    }

    public string Build(HtmlTemplates templates)
    {
        return templates.ListContentTemplate.Render(new
        {
            articles = _articles,
            currentName = _articles.Category.HtmlEncodedName,
        });
    }
}
