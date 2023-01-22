namespace Sitegen.Domain.Model.Core.Options;

/// <summary>
/// 
/// </summary>
public class ArticleTitleOptions
{
    /// <summary>
    /// 
    /// </summary>
    public string ConvertTitle(string title, Tags tags) => OnConvertTitle(title, tags);

    protected virtual string OnConvertTitle(string title, Tags tags) => title;
}
