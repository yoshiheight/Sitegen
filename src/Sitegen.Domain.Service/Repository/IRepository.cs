using Sitegen.Domain.Model.Core;
using Sitegen.Domain.Model.HtmlPage;
using Sitegen.Domain.Model.SearchDatabase;

namespace Sitegen.Domain.Service.Repository;

/// <summary>
/// 
/// </summary>
public interface IRepository
{
    void CleanupDist();

    ArticleCollection ReadArticles();

    HtmlTemplates ReadHtmlTemplates();

    void WriteHtmlPage(Page page);

    void WriteSearchDatabaseChunk(SearchDatabaseChunk chunk);

    void CopyFiles();
}
