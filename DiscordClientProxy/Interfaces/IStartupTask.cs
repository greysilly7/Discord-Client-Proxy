namespace DiscordClientProxy.Interfaces;

public interface IStartupTask
{
    public int Order { get; }
    public Task ExecuteAsync();

    string GetPrefix(int maxLength = 32)
    {
        return $"[StartupTask/{Order:00}: {GetType().Name.PadRight(maxLength)}]";
    }
}