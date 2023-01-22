using System.Collections;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class CategorizedGroups : IReadOnlyCollection<CategorizedArticleCollection>
{
    private readonly IReadOnlyList<CategorizedArticleCollection> _value;

    public CategorizedGroups(ArticleCollection articles)
    {
        _value = articles
            .GroupBy(article => article.Category.Order)
            .Select(group => new CategorizedArticleCollection(group.First().Category, group))
            .OrderBy(articles => articles.Category.Order)
            .ToArray();
    }

    public int Count => _value.Count;
    IEnumerator<CategorizedArticleCollection> IEnumerable<CategorizedArticleCollection>.GetEnumerator() => _value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _value.GetEnumerator();
}
