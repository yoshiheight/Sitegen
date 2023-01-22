using System.Reflection;
using Sitegen.Domain.Model.HtmlPage;

namespace Sitegen.Infrastructure.Local.Storage;

/// <summary>
/// WebFilesディレクトリを管理する。
/// </summary>
public sealed class ResourceFilesStorage
{
    private static readonly string ResourceFilesPath = "HtmlPage/resource_files";

    private readonly LocalDirectory _entryAssemblyDir;

    public ResourceFilesStorage()
    {
        _entryAssemblyDir = new LocalFile(Assembly.GetEntryAssembly()!.Location).GetParent();
    }

    /// <summary>
    /// WebFilesファイルを読み込む。
    /// </summary>
    public IEnumerable<FileResource> EnumerateWebFiles()
    {
        var dir = _entryAssemblyDir.CombineDirectoryPath($"{ResourceFilesPath}/web_files");
        return dir.EnumerateAllFiles()
            .Select(file => new FileResource(file, Path.Join(WebFilesCacheBusting.DirName, dir.GetRelativePath(file))));
    }

    public HtmlTemplates ReadHtmlTemplates()
    {
        var dir = _entryAssemblyDir.CombineDirectoryPath($"{ResourceFilesPath}/html_templates");
        return new(
            pageTemplateText: dir.CombineFilePath("page.html").ReadText(),
            articleContentTemplateText: dir.CombineFilePath("article_content.html").ReadText(),
            listContentTemplateText: dir.CombineFilePath("list_content.html").ReadText(),
            tagsContentTemplateText: dir.CombineFilePath("tags_content.html").ReadText(),
            searchContentTemplateText: dir.CombineFilePath("search_content.html").ReadText());
    }
}
