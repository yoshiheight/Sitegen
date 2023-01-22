using System.Collections;

namespace Sitegen.Domain.Model.Core;

public sealed class TaggedArticleCollection : IReadOnlyCollection<Article>
{
    public Tag Tag { get; }
    public IReadOnlyList<Article> _value;

    public TaggedArticleCollection(Tag tag, IEnumerable<Article> articles)
    {
        Tag = tag;
        _value = articles.ToArray();
    }

    public TaggedArticleCollection(Tag tag, IReadOnlyList<Article> articles)
    {
        Tag = tag;
        _value = articles;
    }

    public int Count => _value.Count;
    IEnumerator<Article> IEnumerable<Article>.GetEnumerator() => _value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _value.GetEnumerator();
}
