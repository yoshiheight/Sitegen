namespace SitegenDebugConsoleApp.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class DebugArticleTitleOptions : ArticleTitleOptions
{
    protected override string OnConvertTitle(string title, Tags tags)
    {
        return tags
            .Aggregate(new StringBuilder(), (sb, tag) => sb.Append(tag.Name switch
            {
                "TypeScript" => "[TS]",
                "C#" or "C++" or "Rust" => $"[{tag.Name}]",
                _ => "",
            }))
            .Append(' ')
            .Append(title)
            .ToString()
            .TrimStart();
    }
}
