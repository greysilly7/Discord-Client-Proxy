using DiscordClientProxy.Interfaces;
using DiscordClientProxy.Utilities;

namespace DiscordClientProxy.StartupTasks;

public class CleanAssetsTask : IStartupTask
{
    public int Order { get; } = 10;

    public async Task ExecuteAsync()
    {
        return;
        if (Configuration.Instance.Cache.StartupCacheOptions.WipeAllOnStart && Directory.Exists(Configuration.Instance.AssetCacheLocationResolved))
        {
            Console.WriteLine("Wiping cache...");
            Directory.Delete(Configuration.Instance.AssetCacheLocationResolved, true);
            Directory.CreateDirectory(Configuration.Instance.AssetCacheLocationResolved);
        }
        else if (Configuration.Instance.Cache.StartupCacheOptions.WipeCodeOnStart && Directory.Exists(Configuration.Instance.AssetCacheLocationResolved))
        {
            Console.WriteLine("Wiping cache...");
            WipeAssetsRecursive(Configuration.Instance.AssetCacheLocationResolved);
        }

        else if (Configuration.Instance.Cache.StartupCacheOptions.WipeCodeOnPatchlistChanged && Directory.Exists(Configuration.Instance.AssetCacheLocationResolved))
        {
            Console.WriteLine("Patch list changed... Wiping cache...");
            //WipeAssetsRecursive(Configuration.Instance.AssetCacheLocationResolved);
        }
    }

    private static void WipeAssetsRecursive(string dir, string[] ext = null)
    {
        ext ??= new[] {".js", ".css"};
        foreach (var directory in Directory.GetDirectories(dir))
        {
            WipeAssetsRecursive(directory);
        }

        foreach (var file in Directory.GetFiles(dir).Where(x => ext.Any(x.EndsWith)))
        {
            File.Delete(file);
            Console.WriteLine($"Deleted {file}");
        }
    }
}