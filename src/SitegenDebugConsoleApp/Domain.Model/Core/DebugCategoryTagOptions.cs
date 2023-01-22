namespace SitegenDebugConsoleApp.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class DebugCategoryTagOptions : CategoryTagOptions
{
    protected override string OnConvertNameToUrl(string name)
    {
        return name
            .Replace("C#", "cs")
            .Replace("C++", "cpp")
            .Replace("デザインパターン", "design-pattern")
            .Replace("開発全般", "general-dev")
            .Replace("静的サイトジェネレーター", "static-site-generator")
            .Replace("Web全般", "general-web");
    }
}
