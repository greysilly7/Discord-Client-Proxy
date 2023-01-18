using DiscordClientProxy.Interfaces;
using DiscordClientProxy.Utilities;

namespace DiscordClientProxy.StartupTasks;

public class LoadConfigurationTask : IStartupTask
{
    public int Order { get; } = 5;
    public Task ExecuteAsync()
    {
        Configuration.Load();
        ClientPatcher.EnsureConfigPopulated();
        return Task.CompletedTask;
    }
}