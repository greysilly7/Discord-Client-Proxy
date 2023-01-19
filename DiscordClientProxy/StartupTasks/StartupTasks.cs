using System.Reflection;
using DiscordClientProxy.Interfaces;

namespace DiscordClientProxy.StartupTasks;

public class StartupTasks
{
    public static async Task Run()
    {
        Console.WriteLine("Searching for startup tasks...");
        var startupTasks = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IStartupTask).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IStartupTask)Activator.CreateInstance(t))
            .OrderBy(t=>t.Order);
        
        Console.WriteLine($"Found {startupTasks.Count()} startup tasks. Running...");
        var maxLength = startupTasks.Max(x => x.GetType().Name.Length);
        foreach (var startupTask in startupTasks)
        {
            Console.WriteLine(startupTask.GetPrefix(maxLength)+" Running...");
            await startupTask.ExecuteAsync();
        }
        Console.WriteLine("Startup tasks finished!");
    }
}