namespace DiscordClientProxy.Interfaces;

public interface IStartupTask
{
    public int Order { get; }
    public Task ExecuteAsync();
}