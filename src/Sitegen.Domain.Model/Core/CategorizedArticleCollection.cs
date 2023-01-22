using System.Collections;

namespace Sitegen.Domain.Model.Core;

public sealed class CategorizedArticleCollection : IReadOnlyCollection<Article>
{
    public Category Category { get; }
    public IReadOnlyList<Article> _value;

    public CategorizedArticleCollection(Category category, IEnumerable<Article> articles)
    {
        Category = category;
        _value = articles.ToArray();
    }

    public CategorizedArticleCollection(Category category, IReadOnlyList<Article> articles)
    {
        Category = category;
        _value = articles;
    }

    public int Count => _value.Count;
    IEnumerator<Article> IEnumerable<Article>.GetEnumerator() => _value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _value.GetEnumerator();
}
