namespace Sitegen.Common.Linq;

/// <summary>
/// LINQ拡張。
/// </summary>
public static class LinqExtension
{
    /// <summary>
    /// シーケンス要素のサイズ合計が指定最大値を超える毎にグループ化する。
    /// </summary>
    public static IEnumerable<IReadOnlyList<TSource>> ChunkBySize<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, int> sizeSelector,
        int maxSize)
    {
        var list = new List<TSource>();
        var totalSize = 0;
        foreach (var elem in source)
        {
            list.Add(elem);
            totalSize += sizeSelector(elem);

            if (totalSize > maxSize)
            {
                yield return list;

                list = new();
                totalSize = 0;
            }
        }

        if (totalSize > 0)
        {
            yield return list;
        }
    }

    /// <summary>
    /// シーケンスの各要素にインデックス値を付加する。
    /// </summary>
    public static IEnumerable<Indexed<TSource>> Indexed<TSource>(this IEnumerable<TSource> source)
    {
        return source.Select((value, index) => new Indexed<TSource>(index, value));
    }

    /// <summary>
    /// シーケンス内で重複するキーを抽出する。
    /// </summary>
    public static IEnumerable<TKey> DuplicateKey<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        return source
            .GroupBy(keySelector)
            .Where(group => group.Skip(1).Any())
            .Select(group => group.Key);
    }
}

public readonly record struct Indexed<TValue>(int Index, TValue Value);
