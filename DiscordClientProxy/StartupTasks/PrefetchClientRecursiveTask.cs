using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DiscordClientProxy.Interfaces;

namespace DiscordClientProxy.StartupTasks;

public class PrefetchClientRecursiveTask : IStartupTask
{
    public int Order { get; } = 11;

    public async Task ExecuteAsync()
    {
        //await TestClientBuilder.PrefetchClient();
        if (!Configuration.Instance.Cache.StartupCacheOptions.DownloadOnStart) return;
        if (!Configuration.Instance.Cache.DownloadAssetsRecursive) return;
        if (!Configuration.Instance.Version.Contains("latest")) return;
        for (int i = 0; i < Configuration.Instance.Cache.RecursiveDownloadDepth; i++)
        {
            Console.WriteLine($"[Startup/PrefetchClientResursiveTask] Downloading assets recursively, depth {i + 1} of {Configuration.Instance.Cache.RecursiveDownloadDepth}");
            Console.WriteLine("[Startup/PrefetchClientResursiveTask] ==> Enumerating assets...");
            var files = TieredAssetStore.EnumerateAssets().Where(x => x.EndsWith(".js") || x.EndsWith(".css"));
            Console.WriteLine("[Startup/PrefetchClientResursiveTask] ==> Reading assets...");
            var assettasks = files
                .Select(x => Task.Factory.StartNew(() => TieredAssetStore.GetAsset(x))).ToList();
            await Task.WhenAll(assettasks);
            var fileContentsAsBytes = assettasks.Select(x => x.Result.Result);
            Console.WriteLine("[Startup/PrefetchClientResursiveTask] ==> Reading assets as strings...");
            var fileContents = fileContentsAsBytes.Select(x => Encoding.UTF8.GetString(x));
            Console.WriteLine("[Startup/PrefetchClientResursiveTask] ==> Parsing assets...");
            var assets = new List<string>();
            fileContents.Select(FindMoreAssets).ToList().ForEach(x => assets.AddRange(x));
            assets = assets.Distinct().ToList();
            Console.WriteLine($"[Startup/PrefetchClientResursiveTask] ==> Identified {assets.Count} assets...");
            assets.RemoveAll(x => File.Exists(Configuration.Instance.AssetCacheLocationResolved + x));
            
            if (assets.Count == 0)
            {
                Console.WriteLine("[Startup/PrefetchClientResursiveTask] ==> No new assets found, stopping...");
                break;
            }
            else
            {
                Console.WriteLine($"[Startup/PrefetchClientResursiveTask] ==> Getting {assets.Count} new assets...");
                var assetDownloadTasks = assets
                    .Select(TieredAssetStore.GetAsset).ToList();
                await Task.WhenAll(assetDownloadTasks);
            }
        }
        //await FetchAssets(Configuration.Instance.Cache.AppBaseUri);
        //await FetchAssets(Configuration.Instance.Cache.DevBaseUri);

        TieredAssetStore.RecordNewDownloads = true;
    }

    //no async here :c
    public static List<string> FindMoreAssets(string content)
    {
        var assets = new List<string>();
        assets.AddRange(Regex.Matches(content, @"\.exports=.\..\+\""(.*?\..{0,5})\""").Select(x => x.Groups[1].Value));
        assets.AddRange(Regex.Matches(content, @"url\(/assets/(.*?)\)").Select(x => x.Groups[1].Value));
        assets.AddRange(Regex.Matches(content, @"\+""([a-zA-Z0-9]+?\..{1,5})""").Select(x => x.Groups[1].Value)); //.Where(x => !assets.Contains(x))
        assets.AddRange(Regex.Matches(content, @"\+""([a-zA-Z0-9]+?\.worker\.js)""").Select(x => x.Groups[1].Value));
        assets.AddRange(Regex.Matches(content, @"\+""([a-zA-Z0-9]+?\.worker\.js)""").Select(x => x.Groups[1].Value));
        var questionableMatches = new List<string>();

        if (questionableMatches.Any())
        {
            Console.WriteLine("Found questionable matches!!!!");
            Console.WriteLine(string.Join("\n", questionableMatches));
            Debugger.Break();
            //Thread.Sleep(10000);
        }
        assets.AddRange(questionableMatches);

        if (File.Exists("emotes.json"))
        {
            var _emotes = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("emotes.json"));
            assets.RemoveAll(x => _emotes.ContainsKey(x));
        }

        return assets;
    }
}