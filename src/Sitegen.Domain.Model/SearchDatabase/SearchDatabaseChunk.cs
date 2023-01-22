namespace Sitegen.Domain.Model.SearchDatabase;

/// <summary>
/// 検索データベース構成要素。
/// </summary>
public sealed class SearchDatabaseChunk
{
    /// <summary>検索インデックスのリンク書式</summary>
    private static readonly string DbLinkFormat = "/search-database/keyword-search-db{0}.json";

    public static readonly string DbLinkFormatWithCacheBusting = $"{DbLinkFormat}?v={Guid.NewGuid():N}";

    public string Url { get; }
    public string Json { get; }

    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public SearchDatabaseChunk(int index, IReadOnlyList<string> entityJsons)
    {
        Url = string.Format(DbLinkFormat, index);
        Json = $"[{string.Join(",", entityJsons)}]";
    }
}
