using Markdig;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class Markdown
{
    private readonly string _markdownText;

    /// <summary>
    /// 
    /// </summary>
    public Markdown(string markdownText, MarkdownOptions markdownOptions)
    {
        _markdownText = string.Join("\n", new TextLines(markdownText).Where(line => !IsCommentLine(line)));

        bool IsCommentLine(string targetLine)
        {
            var prefix = markdownOptions.SingleLineCommentPrefix;
            return !string.IsNullOrEmpty(prefix) && targetLine.StartsWith(prefix, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public TMetadata? GetMetadata<TMetadata>()
        where TMetadata : class
    {
        return YamlSerializer.DeserializeFirst<TMetadata>(_markdownText);
    }

    /// <summary>
    /// HTML変換
    /// </summary>
    public string ToHtml()
    {
        return Markdig.Markdown.ToHtml(_markdownText, new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .UseDiagrams()
            .UsePipeTables()
            .Build());
    }

    /// <summary>
    /// プレーンテキスト変換
    /// </summary>
    public string ToPlainText()
    {
        return Markdig.Markdown.ToPlainText(_markdownText, new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .UseDiagrams()
            //.UsePipeTables() これを使うとtableタグとして出力されてしまう
            .Build());
    }
}

/// <summary>
/// YAMLシリアライザ
/// </summary>
file static class YamlSerializer
{
    /// <summary>
    /// 文書内の先頭のYAMLをデシリアライズする
    /// </summary>
    public static TResult? DeserializeFirst<TResult>(string text)
        where TResult : class
    {
        try
        {
            using var reader = new StringReader(text);
            var parser = new Parser(reader);
            parser.Consume<StreamStart>();
            if (parser.Accept<DocumentStart>(out _))
            {
                var deserializer = new DeserializerBuilder().Build();
                return deserializer.Deserialize<TResult>(parser);
            }
        }
        catch { }
        return null;
    }
}
