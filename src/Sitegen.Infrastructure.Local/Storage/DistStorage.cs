using System.Diagnostics;

namespace Sitegen.Infrastructure.Local.Storage;

/// <summary>
/// 出力先ディレクトリを管理する
/// </summary>
public sealed class DistStorage
{
    private readonly LocalDirectory _outputDir;

    public DistStorage(LocalRepository.ProjectPath projectPath)
    {
        var destDir = projectPath.Dir.CombineDirectoryPath("_dist");

        // 出力先は既存ディレクトリであること（ユーザーが出力先指定を間違えた場合の対策）
        Validation.Validate(destDir.Exists(), $"出力先の {destDir} ディレクトリが存在しません。");

        // 出力先に更に1階層ディレクトリ用意する（ユーザーが出力先指定を間違えた場合の対策）
        _outputDir = destDir.CombineDirectoryPath("latest");
    }

    /// <summary>
    /// 
    /// </summary>
    public void MoveExistsDir()
    {
        if (!_outputDir.Exists()) return;

        var moveDest = _outputDir.GetParent().CombineDirectoryPath($"_old/{DateTime.Now:yyyyMMddHHmmss}");
        Validation.Validate(
            _outputDir.MoveTo(moveDest),
            $"{_outputDir} ディレクトリのリネームに失敗しました。\n手動でリネームもしくは削除してください。");
    }

    /// <summary>
    /// 
    /// </summary>
    public void WriteTextFile(string rootRelativeResourceUrl, string text)
    {
        var destFile = GetDestFilePath(rootRelativeResourceUrl.TrimStart('/'));
        destFile.WriteText(text);
    }

    /// <summary>
    /// ファイルをコピーする
    /// </summary>
    public void CopyFile(FileResource src)
    {
        var destFile = GetDestFilePath(src.RelativePath);
        src.File.CopyTo(destFile);
    }

    /// <summary>
    /// 
    /// </summary>
    private LocalFile GetDestFilePath(string destRelativePath)
    {
        var destFile = _outputDir.CombineFilePath(destRelativePath);
        Debug.WriteLine(destFile);
        return destFile;
    }
}
