using System.Collections;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class Tags : IEnumerable<Tag>
{
    private readonly IReadOnlyList<Tag> _value;

    internal Tags(Article.Metadata metadata, CategoryTagOptions categoryTagOptions)
    {
        var normalized = metadata.Tags?.ToArray() ?? Array.Empty<string>();
        _value = normalized.Select(tagName => new Tag(tagName, categoryTagOptions)).ToArray();

        Validation.Validate(_value.Count > 0, $"記事のタグ情報が存在しません。");
    }

    IEnumerator<Tag> IEnumerable<Tag>.GetEnumerator() => _value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _value.GetEnumerator();

    public override string ToString() => throw new NotSupportedException();
    public override bool Equals(object? _) => throw new NotSupportedException();
    public override int GetHashCode() => throw new NotSupportedException();
}
