using Sitegen.Domain.Model.Core;

namespace Sitegen.Domain.Model.HtmlPage.Content;

/// <summary>
/// 記事ページコンテンツ。
/// </summary>
public sealed class ArticleContent : IMainContent
{
    /// <summary></summary>
    private readonly Article _article;

    /// <summary></summary>
    public bool AllowIndex => _article.Category.AllowRobot;

    /// <summary></summary>
    public bool AllowFollow => _article.Category.AllowRobot;

    /// <summary>パーマリンク</summary>
    public string Permalink => _article.Permalink;

    public string PageTitle => _article.Title.Value;

    public IEnumerable<string> CssFileNames => new[] { "sitegen-article.css" };

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ArticleContent(Article article)
    {
        _article = article;
    }

    public string Build(HtmlTemplates templates)
    {
        return templates.ArticleContentTemplate.Render(new
        {
            targetArticle = _article,
            builded = _article.Markdown.ToHtml(),
        });
    }
}
