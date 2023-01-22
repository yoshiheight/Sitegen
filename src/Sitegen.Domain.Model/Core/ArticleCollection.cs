using System.Collections;

namespace Sitegen.Domain.Model.Core;

public sealed class ArticleCollection : IReadOnlyCollection<Article>
{
    public IReadOnlyList<Article> _value;

    public ArticleCollection(IEnumerable<Article> articles)
    {
        Validation.Validate(articles.Any(), "記事が存在しません。");

        _value = articles
            .OrderByDescending(article => article.Date.Value)
            .ThenBy(article => article.Title.Value)
            .ToArray();

        var duplicateIds = _value.DuplicateKey(article => article.Id.Value).ToArray();
        Validation.Validate(!duplicateIds.Any(), $"記事IDが重複しています。\n{string.Join("\n", duplicateIds)}");
    }

    public int Count => _value.Count;
    IEnumerator<Article> IEnumerable<Article>.GetEnumerator() => _value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _value.GetEnumerator();
}
