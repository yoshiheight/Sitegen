using System.Diagnostics.CodeAnalysis;

namespace Sitegen.Common;

/// <summary>
/// バリデーション。
/// </summary>
public static class Validation
{
    public static void Validate([DoesNotReturnIf(false)] bool condition, string message)
    {
        if (!condition)
        {
            throw new ValidateException(message);
        }
    }
}

/// <summary>
/// バリデーション例外。
/// </summary>
public sealed class ValidateException : Exception
{
    public ValidateException(string message) : base(message) { }
}
