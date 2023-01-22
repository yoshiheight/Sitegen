namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class ArticleId
{
    private static readonly Regex IdRegex = new(@"^[0-9a-z\-]+$", RegexOptions.Compiled);

    public string Value { get; }

    internal ArticleId(Article.Metadata metadata)
    {
        var normalized = metadata.Id ?? string.Empty;

        Validation.Validate(IdRegex.IsMatch(normalized), $"記事のID情報が不正です。");

        Value = normalized;
    }

    public override string ToString() => throw new NotSupportedException();
    public override bool Equals(object? _) => throw new NotSupportedException();
    public override int GetHashCode() => throw new NotSupportedException();
}
