using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using SitegenDebugConsoleApp.Domain.Model.Core;
using SitegenDebugConsoleApp.Domain.Model.SearchDatabase;
using SitegenDebugConsoleApp.Infrastructure;

namespace SitegenDebugConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var options = (Parser.Default.ParseArguments<Options>(args) as Parsed<Options>)?.Value;
            Validation.Validate(options is not null, "コマンド引数が不正です。");

            await new Bootstrapper(options.ProjectDir)
                .ConfigureServices(sc => sc
                    // ドメインモデル
                    .AddSingleton<MarkdownOptions, DebugMarkdownOptions>()
                    .AddSingleton<CategoryTagOptions, DebugCategoryTagOptions>()
                    .AddSingleton<ArticleTitleOptions, DebugArticleTitleOptions>()
                    .AddSingleton<SearchDatabaseBuildOptions, DebugSearchDatabaseBuildOptions>()
                    // インフラ
                    .AddSingleton<LocalRepository>()
                    .AddSingleton<IRepository, DebugRepository<LocalRepository>>()
                    // その他
                    .AddSingleton<IProgress, ConsoleProgress>())
                .RunAsync();
        }
        catch (ValidateException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }
    }
}

/// <summary>
/// コマンドライン引数。
/// </summary>
file sealed class Options
{
    // 例) --project=C:\my_project_dir

    [Option("project", Required = true, HelpText = "=プロジェクトディレクトリ")]
    public string ProjectDir { get; set; } = null!;
}
