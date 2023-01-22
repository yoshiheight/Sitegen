namespace Sitegen.Domain.Model.Core.Factory;

/// <summary>
/// 
/// </summary>
public sealed class ArticleFactory
{
    private readonly MarkdownOptions _markdownOptions;
    private readonly ArticleTitleOptions _articleTitleOptions;
    private readonly CategoryTagOptions _categoryTagOptions;

    /// <summary>
    /// 
    /// </summary>
    public ArticleFactory(
        MarkdownOptions markdownOptions,
        ArticleTitleOptions articleTitleOptions,
        CategoryTagOptions categoryTagOptions)
    {
        _markdownOptions = markdownOptions;
        _articleTitleOptions = articleTitleOptions;
        _categoryTagOptions = categoryTagOptions;
    }

    /// <summary>
    /// 
    /// </summary>
    public Article Create(Category category, string rawArticleName, string markdownText)
    {
        try
        {
            var markdown = new Markdown(markdownText, _markdownOptions);

            var metadata = markdown.GetMetadata<Article.Metadata>();
            Validation.Validate(metadata is not null, $"記事のメタ情報が不正です。");
            var parser = new RawArticleNameParser(rawArticleName);

            var id = new ArticleId(metadata);
            var tags = new Tags(metadata, _categoryTagOptions);
            var date = new ArticleDate(parser.Date);
            var title = new ArticleTitle(parser.Title, tags, _articleTitleOptions);

            return new(category, markdown, id, date, title, tags);
        }
        catch (ValidateException ex)
        {
            throw new ValidateException($"{ex.Message}\n -> 対象記事名: {rawArticleName}");
        }
    }
}

/// <summary>
/// 記事名の解析（但し各フィールドの詳細な検証はここでは行わない）
/// </summary>
file sealed class RawArticleNameParser
{
    private static readonly Regex ParseRegex = new(@"^(\d{4}-\d{2}-\d{2})-(.+)", RegexOptions.Compiled);

    private readonly Match _match;

    public string Date => _match.Groups[1].Value;
    public string Title => _match.Groups[2].Value;

    public RawArticleNameParser(string rawArticleName)
    {
        _match = ParseRegex.Match(rawArticleName);

        Validation.Validate(_match.Success, $"記事名が不正です。");
    }
}
