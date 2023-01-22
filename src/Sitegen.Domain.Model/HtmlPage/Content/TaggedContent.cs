namespace Sitegen.Domain.Model.HtmlPage.Content;

/// <summary>
/// タグページコンテンツ。
/// </summary>
public sealed class TaggedContent : IMainContent
{
    /// <summary></summary>
    public bool AllowIndex => false;

    /// <summary></summary>
    public bool AllowFollow => false;

    /// <summary>パーマリンク</summary>
    public string Permalink => _articles.Tag.Permalink;

    private readonly TaggedArticleCollection _articles;

    public string PageTitle { get; }

    public IEnumerable<string> CssFileNames => new[] { "sitegen-list.css" };

    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public TaggedContent(TaggedArticleCollection group)
    {
        _articles = group;
        PageTitle = "タグ " + _articles.Tag.Name;
    }

    public string Build(HtmlTemplates templates)
    {
        return templates.ListContentTemplate.Render(new
        {
            articles = _articles,
            currentName = _articles.Tag.HtmlEncodedName,
        });
    }
}
