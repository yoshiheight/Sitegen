namespace Sitegen.Infrastructure.Local.Common;

/// <summary>
/// DirectoryInfoの代替用。
/// （DirectoryInfoはミュータブルであり、また使い勝手が悪い為）
/// </summary>
public sealed class LocalDirectory : IFileSystem
{
    private readonly bool _isRoot;

    public string FullName { get; }

    public LocalDirectory(string path)
    {
        FullName = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
        Validate();

        _isRoot = string.IsNullOrEmpty(Path.GetDirectoryName(FullName));

        // Path.TrimEndingDirectorySeparator() を実行すると、
        // ・通常のパスの場合、末尾のセパレータは除去される
        // ・ルートパスの場合、末尾のセパレータは残ったままとなる（Windowsであれば C:\ 等、Linuxであれば /）
        // ルートパスに対して Path.GetFileName() を実行すると空文字列が返ってくる
        // ルートパスに対して Path.GetDirectoryName() を実行すると null が返ってくる
    }

    private void Validate()
    {
        if (string.IsNullOrEmpty(FullName)) throw new IOException();
        // UNCパスやDOSデバイスパスはエラー扱い
        if (FullName.StartsWith(@"\\", StringComparison.Ordinal)) throw new IOException();
    }

    public bool Exists() => Directory.Exists(FullName);

    public void Create() => Directory.CreateDirectory(FullName);

    public bool MoveTo(LocalDirectory destDir)
    {
        if (_isRoot) throw new IOException(FullName);
        if (!Exists()) throw new DirectoryNotFoundException(FullName);

        try
        {
            destDir.GetParent().Create();
            Directory.Move(FullName, destDir.FullName); // Directory.Moveは移動先ディレクトリが既に存在する場合は例外発生

            // ディレクトリ操作直後は存在状態が正しく取得できない可能性があるので、リトライする
            foreach (var _ in Enumerable.Range(0, 10))
            {
                if (!Exists()) return true;
                Thread.Sleep(300);
            }
        }
        catch { }
        return !Exists();
    }

    public string GetName() => _isRoot ? FullName : Path.GetFileName(FullName);

    public LocalDirectory GetParent() => !_isRoot ? new(Path.GetDirectoryName(FullName)!) : throw new IOException();

    public string GetRelativePath(IFileSystem targetPath) => Path.GetRelativePath(FullName, targetPath.FullName);

    private string GetEndSeparatedPath() => Path.EndsInDirectorySeparator(FullName) ? FullName : FullName + Path.DirectorySeparatorChar;
    //public bool IsSubFile(FilePath file) => file.FullName.StartsWith(GetEndSeparatedPath(), StringComparison.Ordinal);
    public bool IsSubPath(IFileSystem targetPath)
    {
        var relativeLen = targetPath.FullName.Length - GetEndSeparatedPath().Length;
        return relativeLen > 0 && GetRelativePath(targetPath).Length == relativeLen;
    }

    public LocalDirectory CombineDirectoryPath(string relative) => CombinePath(relative, path => new LocalDirectory(path));

    public LocalFile CombineFilePath(string relative) => CombinePath(relative, path => new LocalFile(path));

    private TPath CombinePath<TPath>(string relative, Func<string, TPath> pathFactory)
        where TPath : IFileSystem
    {
        var newPath = pathFactory(Path.Combine(FullName, relative));
        // ドット2つを含むパスや区切り文字で始まるパスといった、当該ディレクトリのサブにならない相対パスの結合は許可しない
        if (!IsSubPath(newPath)) throw new IOException();
        return newPath;
    }

    public IEnumerable<LocalDirectory> EnumerateDirectories() =>
        Directory.EnumerateDirectories(FullName)
        .Select(path => new LocalDirectory(path));

    public IEnumerable<LocalFile> EnumerateFiles() =>
        Directory.EnumerateFiles(FullName)
        .Select(path => new LocalFile(path));

    public IEnumerable<LocalFile> EnumerateAllFiles() =>
        Directory.EnumerateFiles(FullName, "*", SearchOption.AllDirectories)
        .Select(path => new LocalFile(path));

    public override string ToString() => FullName;
    public override bool Equals(object? _) => throw new NotSupportedException();
    public override int GetHashCode() => throw new NotSupportedException();
}
