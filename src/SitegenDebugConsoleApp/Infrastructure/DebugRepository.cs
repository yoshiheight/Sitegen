namespace SitegenDebugConsoleApp.Infrastructure;

/// <summary>
/// 
/// </summary>
public sealed class DebugRepository<TRepository> : IRepository
    where TRepository : IRepository
{
    private readonly TRepository _defaultRepository;
    private readonly ArticleFactory _articleFactory;
    private readonly CategoryFactory _categoryFactory;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DebugRepository(TRepository defaultRepository, ArticleFactory articleFactory, CategoryFactory categoryFactory)
    {
        _defaultRepository = defaultRepository;
        _articleFactory = articleFactory;
        _categoryFactory = categoryFactory;
    }

    /// <summary>
    /// 
    /// </summary>
    public void CleanupDist()
    {
        _defaultRepository.CleanupDist();
    }

    /// <summary>
    /// 
    /// </summary>
    public ArticleCollection ReadArticles()
    {
        return new(_defaultRepository.ReadArticles()
            .Concat(Enumerable.Range(0, 100).Select(i => CreateDummyArticle(i))));
    }

    /// <summary>
    /// 
    /// </summary>
    public HtmlTemplates ReadHtmlTemplates()
    {
        return _defaultRepository.ReadHtmlTemplates();
    }

    public void WriteHtmlPage(Page page)
    {
        _defaultRepository.WriteHtmlPage(page);
    }

    public void WriteSearchDatabaseChunk(SearchDatabaseChunk chunk)
    {
        _defaultRepository.WriteSearchDatabaseChunk(chunk);
    }

    /// <summary>
    /// 
    /// </summary>
    public void CopyFiles()
    {
        _defaultRepository.CopyFiles();
    }

    // ダミー記事生成
    private Article CreateDummyArticle(int index) => _articleFactory.Create(
        _categoryFactory.Create("_9_Test's_"),
        $"9999-12-31-これは<br>テスト用の記事です {index:D3}",
        $$"""
        ---
        id: test-{{index:D3}}
        tags: [Test's, Test's, Dummy, Dummy's Data, Dummy, Dummy, visualSTU DIO, これはデバッグタグ]
        ---

        ### テスト見出し

        * テスト項目１
        ＠＠＠これはコメント
        * テスト項目２

        これは`C# 11.0`で開発。フロントエンドは`TypeScript 4.9`を使用。

        > Stay hungry, stay foolish

        <https://visualstudio.microsoft.com/>

        ```cs
        class Program
        {
            void Main()
            {
                Console.WriteLine("Hello, World!");
            }
        }
        ```
        """);
}
