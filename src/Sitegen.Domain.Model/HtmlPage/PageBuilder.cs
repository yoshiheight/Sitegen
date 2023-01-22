using Sitegen.Domain.Model.HtmlPage.Content;

namespace Sitegen.Domain.Model.HtmlPage;

[Mutable]
public sealed class PageBuilder
{
    private readonly Categories _categories;
    private readonly IProgress _progress;

    private readonly HtmlTemplates _templates;

    private readonly List<BuildItem> _list = new();

    private sealed record BuildItem(IMainContent PageContent, string ProgressMessage);

    public PageBuilder(HtmlTemplates templates, Categories categories, IProgress progress)
    {
        _templates = templates;
        _categories = categories;
        _progress = progress;
    }

    public PageBuilder Add(IMainContent content, string progressMessage)
    {
        _list.Add(new BuildItem(content, progressMessage));
        return this;
    }

    public PageBuilder AddRange(IEnumerable<IMainContent> contents, string progressMessage)
    {
        _list.AddRange(contents.Select(pageContent => new BuildItem(pageContent, progressMessage)));
        return this;
    }

    public IEnumerable<Page> Build()
    {
        return _list.AsParallel()
            .Select(item =>
            {
                _progress.Report(string.Format(item.ProgressMessage, item.PageContent.PageTitle));
                return Page.Build(_templates, item.PageContent, _categories);
            });
    }
}
