namespace Sitegen.Domain.Model.HtmlPage.Content;

/// <summary>
/// タグページコンテンツ。
/// </summary>
public sealed class TagsContent : IMainContent
{
    /// <summary>リンク</summary>
    public static readonly string Link = "/tags/";

    /// <summary></summary>
    public bool AllowIndex => false;

    /// <summary></summary>
    public bool AllowFollow => false;

    /// <summary>パーマリンク</summary>
    public string Permalink => Link;

    public string PageTitle => "タグ一覧";

    private readonly TaggedGroups _groups;

    public IEnumerable<string> CssFileNames => new[] { "sitegen-tags.css" };

    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public TagsContent(TaggedGroups groups)
    {
        _groups = groups;
    }

    public string Build(HtmlTemplates templates)
    {
        return templates.TagsContentTemplate.Render(new
        {
            tagCounts = _groups,
        });
    }
}
