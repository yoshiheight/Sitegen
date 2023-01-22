namespace Sitegen.Infrastructure.Local.Storage;

/// <summary>
/// 入力元ディレクトリを管理する。
/// </summary>
public sealed class SrcStorage
{
    /// <summary>入力元ディレクトリパス。</summary>
    private readonly LocalDirectory _srcDir;

    private readonly ArticleFactory _articleFactory;
    private readonly CategoryFactory _categoryFactory;

    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public SrcStorage(
        LocalRepository.ProjectPath projectPath,
        ArticleFactory articleFactory,
        CategoryFactory categoryFactory)
    {
        _srcDir = projectPath.Dir;
        _articleFactory = articleFactory;
        _categoryFactory = categoryFactory;

        Validation.Validate(_srcDir.Exists(), $"入力元の {_srcDir} ディレクトリが存在しません。");
    }

    /// <summary>
    /// 記事マークダウンファイルを読み込む。
    /// </summary>
    public IEnumerable<Article> EnumerateArticles()
    {
        return _srcDir.CombineDirectoryPath("_article")
            .EnumerateDirectories()
            .Select(dir => (dir, Category: _categoryFactory.Create(dir.GetName())))
            .SelectMany(item => item.dir.EnumerateFiles().Select(file => (file, item.Category)))
            .Select(item => _articleFactory.Create(
                item.Category,
                item.file.GetNameWithoutExt(),
                item.file.ReadText()));
    }

    /// <summary>
    /// コピーのみのファイルを読み込む。
    /// </summary>
    public IEnumerable<FileResource> EnumerateCopyOnlyFiles()
    {
        var dir = _srcDir.CombineDirectoryPath("_copyonly");
        return dir.EnumerateAllFiles()
            .Select(file => new FileResource(file, dir.GetRelativePath(file)));
    }
}
