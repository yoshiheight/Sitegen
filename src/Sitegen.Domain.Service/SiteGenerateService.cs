using Sitegen.Domain.Model.SearchDatabase.Factory;
using Sitegen.Domain.Service.Repository;

namespace Sitegen.Domain.Service;

public sealed class SiteGenerateService
{
    private readonly IRepository _repository;
    private readonly IProgress _progress;
    private readonly SearchDatabaseBuilderFactory _searchDatabaseBuilderFactory;

    public SiteGenerateService(
        IRepository repository,
        IProgress progress,
        SearchDatabaseBuilderFactory searchDatabaseBuilderFactory)
    {
        _repository = repository;
        _progress = progress;
        _searchDatabaseBuilderFactory = searchDatabaseBuilderFactory;
    }

    public void Generate()
    {
        _progress.Report("================ 開始 ================");

        _progress.Report("出力先クリーンアップ中...");
        _repository.CleanupDist();

        _progress.Report("記事の読み込み中...");
        var articles = _repository.ReadArticles();

        var categorizedGroups = new CategorizedGroups(articles);
        var taggedGroups = new TaggedGroups(articles);
        var categories = new Categories(articles);
        var templates = _repository.ReadHtmlTemplates();

        foreach (var page in new PageBuilder(templates, categories, _progress)
            .Add(new TopContent(categorizedGroups.First()), "トップページ生成")
            .Add(new TagsContent(taggedGroups), "タグ一覧ページ生成")
            .Add(new SearchContent(), "検索ページ生成")
            .AddRange(categorizedGroups.Select(group => new CategorizedContent(group)), "カテゴリページ生成: {0}")
            .AddRange(taggedGroups.Select(group => new TaggedContent(group)), "タグページ生成: {0}")
            .AddRange(articles.Select(article => new ArticleContent(article)), "ページ変換: {0}")
            .Build())
        {
            _repository.WriteHtmlPage(page);
        }

        _progress.Report("検索データベース生成中...");
        foreach (var dbChunk in _searchDatabaseBuilderFactory.Create()
            .Build(articles))
        {
            _repository.WriteSearchDatabaseChunk(dbChunk);
        }

        _progress.Report("各種ファイルコピー中...");
        _repository.CopyFiles();

        _progress.Report("================ 完了 ================");
    }
}
