using YamlDotNet.Serialization;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 記事ページコンテンツ。
/// </summary>
public sealed class Article
{
    /// <summary>リンク</summary>
    private static readonly string LinkFormat = "/articles/{0}/";

    /// <summary></summary>
    public Category Category { get; }

    /// <summary></summary>
    public Markdown Markdown { get; }

    /// <summary>記事ID</summary>
    public ArticleId Id { get; }

    /// <summary></summary>
    public Tags Tags { get; }

    /// <summary>作成日</summary>
    public ArticleDate Date { get; }

    /// <summary>タイトル</summary>
    public ArticleTitle Title { get; }

    /// <summary>パーマリンク</summary>
    public string Permalink { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    internal Article(
        Category category,
        Markdown markdown,
        ArticleId id,
        ArticleDate date,
        ArticleTitle title,
        Tags tags)
    {
        Category = category;
        Markdown = markdown;
        Id = id;
        Date = date;
        Title = title;
        Tags = tags;
        Permalink = string.Format(LinkFormat, id.Value);
    }

    [Mutable]
    internal sealed class Metadata
    {
        [YamlMember(Alias = "id")]
        public string? Id { get; set; }

        [YamlMember(Alias = "tags")]
        public string?[]? Tags { get; set; }
    }

    public override string ToString() => throw new NotSupportedException();
    //public override bool Equals(object? _) => throw new NotSupportedException();
    public override int GetHashCode() => throw new NotSupportedException();
}
