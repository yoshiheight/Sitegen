using System.Net;

namespace Sitegen.Domain.Model.Core;

/// <summary>
/// 
/// </summary>
public sealed class Category
{
    /// <summary>リンク</summary>
    private static readonly string LinkFormat = "/categories/{0}/";

    public int Order { get; }
    public string Name { get; }
    public bool AllowRobot { get; }
    public string HtmlEncodedName { get; }
    public string Permalink { get; }

    internal Category(int order, string name, bool allowRobot, string convertedName)
    {
        Order = order;
        Name = name;
        AllowRobot = allowRobot;
        HtmlEncodedName = WebUtility.HtmlEncode(name);
        Permalink = string.Format(LinkFormat, convertedName);
    }

    public override string ToString() => throw new NotSupportedException();
    //public override bool Equals(object? _) => throw new NotSupportedException();
    public override int GetHashCode() => throw new NotSupportedException();
}
