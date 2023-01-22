using System.Collections;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class TaggedGroups : IReadOnlyCollection<TaggedArticleCollection>
{
    private readonly IReadOnlyList<TaggedArticleCollection> _value;

    public TaggedGroups(ArticleCollection articles)
    {
        _value = articles
            .SelectMany(article => article.Tags.Select(tag => (tag, article)))
            .OrderBy(item => item.article.Category.Order)
            .GroupBy(item => item.tag.Name)
            .Select(group => new TaggedArticleCollection(group.First().tag, group.Select(item => item.article)))
            .OrderBy(articles => articles.Tag.Name)
            .ToArray();
    }

    public int Count => _value.Count;
    IEnumerator<TaggedArticleCollection> IEnumerable<TaggedArticleCollection>.GetEnumerator() => _value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _value.GetEnumerator();
}
