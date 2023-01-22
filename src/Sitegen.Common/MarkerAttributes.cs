namespace Sitegen.Common;

public class MarkerAttribute : Attribute { }

/// <summary>
/// ミュータブルなクラスに指定する属性。
/// この属性指定がないクラスはイミュータブルである。
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class MutableAttribute : MarkerAttribute { }
