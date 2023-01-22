using System.Text;

namespace Sitegen.Infrastructure.Local.Common;

/// <summary>
/// FileInfoの代替用。
/// （FileInfoはミュータブルであり、また使い勝手が悪い為）
/// </summary>
public sealed class LocalFile : IFileSystem
{
    public string FullName { get; }

    public LocalFile(string path)
    {
        FullName = Path.GetFullPath(path);
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrEmpty(FullName)) throw new IOException();
        if (Path.EndsInDirectorySeparator(FullName)) throw new IOException();
        if (string.IsNullOrEmpty(Path.GetDirectoryName(FullName))) throw new IOException();
        // UNCパスやDOSデバイスパスはエラー扱い
        if (FullName.StartsWith(@"\\", StringComparison.Ordinal)) throw new IOException();
    }

    public bool Exists() => File.Exists(FullName);

    public LocalDirectory GetParent() => new(Path.GetDirectoryName(FullName)!);

    public string GetNameWithoutExt() => Path.GetFileNameWithoutExtension(FullName);

    public string ReadText() => File.ReadAllText(FullName, Encoding.UTF8);

    public void WriteText(string text)
    {
        GetParent().Create();

        // 現在のプラットフォームの改行コードを使用して出力する
        using var writer = new StreamWriter(FullName, false, new UTF8Encoding(false, true));
        foreach (var line in new TextLines(text))
        {
            writer.WriteLine(line);
        }
    }

    public void CopyTo(LocalFile destFile)
    {
        destFile.GetParent().Create();
        File.Copy(FullName, destFile.FullName, false);
        File.SetLastWriteTime(destFile.FullName, File.GetLastWriteTime(FullName));
    }

    public override string ToString() => FullName;
    public override bool Equals(object? _) => throw new NotSupportedException();
    public override int GetHashCode() => throw new NotSupportedException();
}
