using System.Net;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class Tag
{
    private static readonly Regex TagRegex = new("""^([^"\s]| )+$""", RegexOptions.Compiled);

    /// <summary>リンク</summary>
    private static readonly string LinkFormat = "/tags/{0}/";

    public string Name { get; }
    public string HtmlEncodedName { get; }
    public string Permalink { get; }

    internal Tag(string? name, CategoryTagOptions categoryTagOptions)
    {
        var normalized = name?.Trim() ?? string.Empty;

        Validation.Validate(TagRegex.IsMatch(normalized), $"記事のタグ情報が不正です。");

        Name = normalized;
        HtmlEncodedName = WebUtility.HtmlEncode(normalized);
        Permalink = string.Format(LinkFormat, categoryTagOptions.ConvertNameToUrl(normalized));
    }

    public override string ToString() => throw new NotSupportedException();
    //public override bool Equals(object? _) => throw new NotSupportedException();
    public override int GetHashCode() => throw new NotSupportedException();
}
