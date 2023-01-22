using System.Net;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class ArticleTitle
{
    public string Value { get; }
    public string HtmlEncodedName { get; }

    internal ArticleTitle(string title, Tags tags, ArticleTitleOptions articleTitleOptions)
    {
        var value = title.Trim();
        Validation.Validate(value.Length > 0, $"記事のタイトル情報が不正です。");

        Value = articleTitleOptions.ConvertTitle(value, tags);
        HtmlEncodedName = WebUtility.HtmlEncode(Value);
    }

    public override string ToString() => throw new NotSupportedException();
    public override bool Equals(object? _) => throw new NotSupportedException();
    public override int GetHashCode() => throw new NotSupportedException();
}
