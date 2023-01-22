using System.Net;
using System.Security.Cryptography;

namespace Sitegen.Domain.Model.Core.Options;

/// <summary>
/// 
/// </summary>
public class CategoryTagOptions
{
    /// <summary>
    /// 
    /// </summary>
    public string ConvertNameToUrl(string name)
    {
        var filteredName = OnConvertNameToUrl(name);

        var encoded = WebUtility.UrlEncode(filteredName
            .Replace(" ", "")
            .Replace(".", "")
            .Replace("'", ""));

        if (encoded.Contains('%'))
        {
            using var sha1 = SHA1.Create();
            return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(encoded))).Replace("-", "").ToLower();
        }
        return encoded.ToLower();
    }

    protected virtual string OnConvertNameToUrl(string name) => name;
}
