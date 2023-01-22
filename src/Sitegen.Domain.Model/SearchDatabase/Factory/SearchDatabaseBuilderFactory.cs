using Sitegen.Domain.Model.SearchDatabase.Options;

namespace Sitegen.Domain.Model.SearchDatabase.Factory;

/// <summary>
/// 
/// </summary>
public sealed class SearchDatabaseBuilderFactory
{
    private readonly SearchDatabaseBuildOptions _searchDatabaseBuildOptions;

    /// <summary>
    /// 
    /// </summary>
    public SearchDatabaseBuilderFactory(SearchDatabaseBuildOptions searchDatabaseBuildOptions)
    {
        _searchDatabaseBuildOptions = searchDatabaseBuildOptions;
    }

    /// <summary>
    /// 
    /// </summary>
    public SearchDatabaseBuilder Create()
    {
        return new SearchDatabaseBuilder(_searchDatabaseBuildOptions);
    }
}
