namespace Sitegen.Domain.Model.SearchDatabase.Options;

/// <summary>
/// 
/// </summary>
public class SearchDatabaseBuildOptions
{
    /// <summary>JSONファイル1つ当たりのサイズ上限</summary>
    public virtual int MaxFileSize => 200 * 1024;

    /// <summary></summary>
    public virtual bool UseIndent => false;
}
