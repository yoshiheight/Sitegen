using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Sitegen.Domain.Model.SearchDatabase.Options;

namespace Sitegen.Domain.Model.SearchDatabase;

/// <summary>
/// 検索データベースを構築用。
/// </summary>
public sealed class SearchDatabaseBuilder
{
    private readonly SearchDatabaseBuildOptions _searchDatabaseBuildOptions;

    /// <summary>
    /// コンストラクタ。
    /// </summary>
    internal SearchDatabaseBuilder(SearchDatabaseBuildOptions searchDatabaseBuildOptions)
    {
        _searchDatabaseBuildOptions = searchDatabaseBuildOptions;
    }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<SearchDatabaseChunk> Build(ArticleCollection articles)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = _searchDatabaseBuildOptions.UseIndent,
        };

        foreach (var (index, entityJsons) in articles
            .OrderByDescending(article => article.Date.Value)
            .ThenBy(article => article.Title.Value)
            .AsParallel()
            .AsOrdered()
            .Select(article => JsonSerializer.Serialize(new ArticleEntity(article), jsonOptions))
            .Append(JsonSerializer.Serialize(ArticleEntity.Terminate, jsonOptions))
            .ChunkBySize(json => Encoding.UTF8.GetByteCount(json), _searchDatabaseBuildOptions.MaxFileSize)
            .Indexed())
        {
            yield return new(index, entityJsons);
        }
    }
}
