namespace Sitegen.Domain.Model.SearchDatabase;

/// <summary>
/// 
/// </summary>
public sealed record TagEntity(
    string Name,
    string Url)
{
    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public TagEntity(Tag tag) : this(
        tag.Name,
        tag.Permalink)
    {
    }
}
