namespace Sitegen.Common;

public interface IProgress : IProgress<string>
{
}

public sealed class DefaultProgress : IProgress
{
    // noop
    public void Report(string message) { }
}

public sealed class ConsoleProgress : IProgress
{
    public void Report(string message) => Console.WriteLine(message);
}

public sealed class Progress : IProgress
{
    private readonly IProgress<string> _progress;

    public Progress(Action<string> callback) => _progress = new Progress<string>(callback);

    public void Report(string message) => _progress.Report(message);
}
