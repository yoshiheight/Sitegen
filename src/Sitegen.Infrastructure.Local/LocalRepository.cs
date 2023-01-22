using Sitegen.Domain.Model.Core;
using Sitegen.Domain.Model.HtmlPage;
using Sitegen.Domain.Model.SearchDatabase;
using Sitegen.Domain.Service.Repository;
using Sitegen.Infrastructure.Local.Storage;

namespace Sitegen.Infrastructure.Local;

/// <summary>
/// 
/// </summary>
public sealed class LocalRepository : IRepository
{
    private readonly SrcStorage _srcStorage;
    private readonly DistStorage _distStorage;
    private readonly ResourceFilesStorage _resourceFilesStorage;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public LocalRepository(
        SrcStorage srcStorage,
        DistStorage distStorage,
        ResourceFilesStorage resourceFilesStorage)
    {
        _srcStorage = srcStorage;
        _distStorage = distStorage;
        _resourceFilesStorage = resourceFilesStorage;
    }

    /// <summary>
    /// 
    /// </summary>
    public void CleanupDist()
    {
        _distStorage.MoveExistsDir();
    }

    /// <summary>
    /// 
    /// </summary>
    public ArticleCollection ReadArticles()
    {
        return new(_srcStorage.EnumerateArticles());
    }

    /// <summary>
    /// 
    /// </summary>
    public HtmlTemplates ReadHtmlTemplates()
    {
        return _resourceFilesStorage.ReadHtmlTemplates();
    }

    public void WriteHtmlPage(Page page)
    {
        _distStorage.WriteTextFile(page.Url, page.Html);
    }

    public void WriteSearchDatabaseChunk(SearchDatabaseChunk chunk)
    {
        _distStorage.WriteTextFile(chunk.Url, chunk.Json);
    }

    /// <summary>
    /// 
    /// </summary>
    public void CopyFiles()
    {
        foreach (var srcFile in _srcStorage.EnumerateCopyOnlyFiles()
            .Concat(_resourceFilesStorage.EnumerateWebFiles()))
        {
            _distStorage.CopyFile(srcFile);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ProjectPath
    {
        internal LocalDirectory Dir { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProjectPath(string path)
        {
            Dir = new(path);
        }
    }
}
