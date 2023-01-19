using DiscordClientProxy.Interfaces;

namespace DiscordClientProxy.StartupTasks;

public class CreateWorkDirStructureTask : IStartupTask
{
    public int Order { get; } = 7;
    public Task ExecuteAsync()
    {
        ResourceDirHandler.CreateResourceDirInfo();
        Directory.CreateDirectory("assets/" + Configuration.Instance.Version);
        
        return Task.CompletedTask;
    }
}