namespace SitegenDebugConsoleApp.Domain.Model.SearchDatabase;

/// <summary>
/// 
/// </summary>
public sealed class DebugSearchDatabaseBuildOptions : SearchDatabaseBuildOptions
{
    /// <summary>JSONファイル1つ当たりのサイズ上限</summary>
    public override int MaxFileSize => 20 * 1024;

    /// <summary></summary>
    public override bool UseIndent => true;
}
