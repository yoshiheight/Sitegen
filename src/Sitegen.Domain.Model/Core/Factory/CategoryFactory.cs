namespace Sitegen.Domain.Model.Core.Factory;

/// <summary>
/// 
/// </summary>
public sealed class CategoryFactory
{
    private readonly CategoryTagOptions _categoryTagOptions;

    /// <summary>
    /// 
    /// </summary>
    public CategoryFactory(CategoryTagOptions categoryTagOptions)
    {
        _categoryTagOptions = categoryTagOptions;
    }

    /// <summary>
    /// 
    /// </summary>
    public Category Create(string rawCategory)
    {
        var parser = new RawCategoryNameParser(rawCategory);

        var order = parser.Order;
        var name = parser.Name.Trim();
        Validation.Validate(name.Length > 0, $"カテゴリ名を指定してください。カテゴリ: {rawCategory}");
        var allowRobot = parser.AllowRobot;
        var convertedName = _categoryTagOptions.ConvertNameToUrl(name);

        return new(order, name, allowRobot, convertedName);
    }
}

/// <summary>
/// カテゴリ名の解析（但し各フィールドの詳細な検証はここでは行わない）
/// </summary>
file sealed class RawCategoryNameParser
{
    private static readonly Regex ParseRegex = new(@"^_(\d)_(.+?)(_*)$", RegexOptions.Compiled);

    private readonly Match _match;

    public int Order => Convert.ToInt32(_match.Groups[1].Value);
    public string Name => _match.Groups[2].Value;
    public bool AllowRobot => _match.Groups[3].Value.Length == 0;

    public RawCategoryNameParser(string rawCategory)
    {
        _match = ParseRegex.Match(rawCategory);

        Validation.Validate(_match.Success, $"カテゴリ指定が不正です。カテゴリ: {rawCategory}");
    }
}
