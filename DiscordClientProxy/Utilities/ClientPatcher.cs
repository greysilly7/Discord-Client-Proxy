using System.Reflection;
using System.Text.RegularExpressions;
using DiscordClientProxy.Classes;
using DiscordClientProxy.ClientPatches;
using DiscordClientProxy.ClientPatches.Branding;
using DiscordClientProxy.ClientPatches.CustomisationPatches;
using DiscordClientProxy.Interfaces;

namespace DiscordClientProxy.Utilities;

public class ClientPatcher
{
    public static ClientPatch[] ClientPatches = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => typeof(ClientPatch).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
        .Select(t => (ClientPatch) Activator.CreateInstance(t)).ToArray();

    public static void EnsureConfigPopulated()
    {
        Console.WriteLine("[ClientPatcher] Populating config with patches...");
        foreach (var patch in ClientPatches)
        {
            if (Configuration.Instance.Client.DebugOptions.Patches.ContainsKey(patch.GetType().Name)) continue;
            Console.WriteLine($"[ClientPatcher] Adding patch {patch.GetType().Name} to config with default value {patch.IsEnabledByDefault}");
            Configuration.Instance.Client.DebugOptions.Patches.TryAdd(patch.GetType().Name, patch.IsEnabledByDefault);
            Configuration.Instance.Save();
        }
    }

    public static async Task PatchFile(string path)
    {
        Console.WriteLine($"[ClientPatcher] Applying patches to {path}...");
        var content = await File.ReadAllTextAsync(path);
        content = await Patch(content);
        await File.WriteAllTextAsync(path, content);
    }

    public static async Task<string> Patch(string content, string filename = "unknown-file")
    {
        var oldSize = content.Length;
        foreach (var patch in ClientPatches)
        {
            //make sure its definitely in there!
            if (!Configuration.Instance.Client.DebugOptions.Patches.ContainsKey(patch.GetType().Name))
            {
                Configuration.Instance.Client.DebugOptions.Patches.TryAdd(patch.GetType().Name, patch.IsEnabledByDefault);
                Configuration.Instance.Save();
            }

            if (Configuration.Instance.Client.DebugOptions.Patches.TryGetValue(patch.GetType().Name, out var enabled) && enabled)
                content = await patch.ApplyPatch(content);
        }

        Console.WriteLine($"[ClientPatcher] {filename}: {oldSize} -> {content.Length} ({(content.Length < oldSize ? "+" : "")}{oldSize - content.Length} bytes)");

        /*if (Configuration.Instance.Cache.DownloadAssetsRecursive)
        {
var assets = await FindMoreAssets(content);
assets = assets.Where(x => !File.Exists($"{Configuration.Instance.AssetCacheLocationResolved}/{x}")).ToList();
if (assets.Count > 0)
{
    Console.WriteLine($"[ClientPatcher] Found {assets.Count} assets to fetch");
    var throttler = new SemaphoreSlim(256); //(System.Environment.ProcessorCount * 8);
    var assettasks = assets.Where(x => !x.EndsWith("js") && !x.EndsWith("css"))
        .Select(x => Task.Factory.StartNew(() => AssetCache.StreamToDiskAsync($"{Configuration.Instance.AssetCacheLocationResolved}/{x}", "https://discord.com/assets/" + x))).ToList();
    var tasks = assets.Where(x => x.EndsWith("js") || x.EndsWith("css")).Select(x => Task.Factory.StartNew(async () =>
    {
        {
            await throttler.WaitAsync();
            await AssetCache.GetFromNetwork(x.Replace("/assets/", ""));
            throttler.Release();
        }
    })).ToList();
    //await Task.WhenAll(tasks);
    await Task.WhenAll(assettasks);
}
        }*/

        return content;
    }
}