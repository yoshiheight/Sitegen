using System.Collections;

namespace Sitegen.Common.Text;

public sealed class TextLines : IEnumerable<string>
{
    private readonly string _text;

    public TextLines(string text)
    {
        _text = text;
    }

    private IEnumerable<string> ReadLines()
    {
        using var reader = new StringReader(_text);
        while (reader.ReadLine() is { } line)
        {
            yield return line;
        }
    }

    IEnumerator<string> IEnumerable<string>.GetEnumerator() => ReadLines().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ReadLines().GetEnumerator();
}
