using System.Globalization;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class ArticleDate
{
    public string Value { get; }

    internal ArticleDate(string date)
    {
        var value = date;
        Validation.Validate(
            DateTime.TryParseExact(value, "yyyy-MM-dd", null, DateTimeStyles.None, out var _),
            $"記事の日付情報が不正です。");

        Value = value;
    }

    public override string ToString() => throw new NotSupportedException();
    public override bool Equals(object? _) => throw new NotSupportedException();
    public override int GetHashCode() => throw new NotSupportedException();
}
