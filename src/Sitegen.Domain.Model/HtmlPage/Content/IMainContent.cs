namespace Sitegen.Domain.Model.HtmlPage.Content;

/// <summary>
/// HTMLページのメインコンテンツ部分を表す。
/// </summary>
public interface IMainContent
{
    /// <summary></summary>
    bool AllowIndex { get; }

    bool AllowFollow { get; }

    /// <summary>パーマリンク</summary>
    string Permalink { get; }

    string Build(HtmlTemplates templates);

    string PageTitle { get; }

    IEnumerable<string> CssFileNames { get; }
}
