namespace Sitegen.Domain.Model.SearchDatabase;

/// <summary>
/// 
/// </summary>
public sealed record CategoryEntity(
    int Order,
    string Name,
    string Url)
{
    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public CategoryEntity(Category category) : this(
        category.Order,
        category.Name,
        category.Permalink)
    {
    }
}
