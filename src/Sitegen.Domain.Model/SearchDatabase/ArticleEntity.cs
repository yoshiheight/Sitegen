namespace Sitegen.Domain.Model.SearchDatabase;

/// <summary>
/// 検索に使用する記事データ。
/// </summary>
/// <param name="Url">ページURL</param>
/// <param name="Title">ページタイトル</param>
/// <param name="Tags">タグ</param>
/// <param name="Date">ページ更新日</param>
/// <param name="PlainText">ページ内容</param>
public sealed record ArticleEntity(
    string Url,
    string Date,
    string Title,
    CategoryEntity Category,
    IReadOnlyList<TagEntity> Tags,
    string Content)
{
    // 連続する空白、タブ文字、改行文字の除去
    private static readonly Regex Spaces = new(@"\s+", RegexOptions.Compiled);

    /// <summary>ターミネート用</summary>
    public static readonly ArticleEntity Terminate = new(
        string.Empty,
        string.Empty,
        string.Empty,
        new(0, string.Empty, string.Empty),
        Array.Empty<TagEntity>(),
        string.Empty);

    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public ArticleEntity(Article article) : this(
        article.Permalink,
        article.Date.Value,
        article.Title.Value,
        new(article.Category),
        article.Tags.Select(tag => new TagEntity(tag)).ToArray(),
        Spaces.Replace(article.Markdown.ToPlainText(), " ").Trim())
    {
    }
}
