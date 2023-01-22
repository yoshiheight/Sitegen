using Microsoft.Extensions.DependencyInjection;
using Sitegen.Common;
using Sitegen.Domain.Model.Core.Factory;
using Sitegen.Domain.Model.Core.Options;
using Sitegen.Domain.Model.SearchDatabase.Factory;
using Sitegen.Domain.Model.SearchDatabase.Options;
using Sitegen.Domain.Service;
using Sitegen.Domain.Service.Repository;
using Sitegen.Infrastructure.Local;
using Sitegen.Infrastructure.Local.Storage;

namespace Sitegen;

/// <summary>
/// 
/// </summary>
public sealed class Bootstrapper
{
    /// <summary></summary>
    private readonly ServiceCollection _sc = new();

    /// <summary>
    /// 
    /// </summary>
    public Bootstrapper(string dirPath)
    {
        // ドメインモデル
        _sc.AddSingleton<ArticleFactory>();
        _sc.AddSingleton<CategoryFactory>();
        _sc.AddSingleton<ArticleTitleOptions>();
        _sc.AddSingleton<CategoryTagOptions>();
        _sc.AddSingleton<MarkdownOptions>();
        _sc.AddSingleton<SearchDatabaseBuilderFactory>();
        _sc.AddSingleton<SearchDatabaseBuildOptions>();

        // ドメインサービス
        _sc.AddSingleton<SiteGenerateService>();

        // インフラ
        _sc.AddSingleton(_ => new LocalRepository.ProjectPath(dirPath));
        _sc.AddSingleton<IRepository, LocalRepository>();
        _sc.AddSingleton<SrcStorage>();
        _sc.AddSingleton<ResourceFilesStorage>();
        _sc.AddSingleton<DistStorage>();

        // その他
        _sc.AddSingleton<IProgress, DefaultProgress>();
    }

    /// <summary>
    /// 
    /// </summary>
    public Bootstrapper ConfigureServices(Action<IServiceCollection> setupper)
    {
        setupper(_sc);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task RunAsync()
    {
        await Task.Factory.StartNew(() =>
        {
            var serviceProvider = _sc.BuildServiceProvider();
            serviceProvider.GetRequiredService<SiteGenerateService>()
                .Generate();
        });
    }
}
