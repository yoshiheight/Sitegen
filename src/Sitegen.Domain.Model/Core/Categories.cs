using System.Collections;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class Categories : IEnumerable<Category>
{
    private readonly IReadOnlyList<Category> _value;

    public Categories(ArticleCollection articles)
    {
        _value = articles
            .DistinctBy(article => article.Category.Order)
            .Select(article => article.Category)
            .OrderBy(category => category.Order)
            .ToArray();
    }

    IEnumerator<Category> IEnumerable<Category>.GetEnumerator() => _value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _value.GetEnumerator();
}
