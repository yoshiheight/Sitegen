namespace Sitegen.Domain.Model.HtmlPage;

public sealed class HtmlTemplates
{
    public HtmlTemplate PageTemplate { get; }
    public HtmlTemplate ArticleContentTemplate { get; }
    public HtmlTemplate ListContentTemplate { get; }
    public HtmlTemplate TagsContentTemplate { get; }
    public HtmlTemplate SearchContentTemplate { get; }

    public HtmlTemplates(
        string pageTemplateText,
        string articleContentTemplateText,
        string listContentTemplateText,
        string tagsContentTemplateText,
        string searchContentTemplateText)
    {
        PageTemplate = new(pageTemplateText);
        ArticleContentTemplate = new(articleContentTemplateText);
        ListContentTemplate = new(listContentTemplateText);
        TagsContentTemplate = new(tagsContentTemplateText);
        SearchContentTemplate = new(searchContentTemplateText);
    }
}

public sealed class HtmlTemplate
{
    private readonly Scriban.Template _template;

    public HtmlTemplate(string templateText)
    {
        _template = Scriban.Template.Parse(templateText);
    }

    public string Render(object model)
    {
        return _template.Render(model, member => member.Name).Trim();
    }
}
