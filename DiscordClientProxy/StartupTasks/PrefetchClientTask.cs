using System.Text.RegularExpressions;
using System.Text.Unicode;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using DiscordClientProxy.Interfaces;
using DiscordClientProxy.Utilities;

namespace DiscordClientProxy.StartupTasks;

public class PrefetchClientTask : IStartupTask
{
    public int Order { get; } = 10;

    public async Task ExecuteAsync()
    {
        //await TestClientBuilder.PrefetchClient();
        if (!Configuration.Instance.Cache.StartupCacheOptions.DownloadOnStart) return;
        if (!Configuration.Instance.Version.Contains("latest")) return;
        await FetchAssets(Configuration.Instance.Cache.AppBaseUri);
        //await FetchAssets(Configuration.Instance.Cache.DevBaseUri);
    }

    private static async Task FetchAssets(string url)
    {
        Console.WriteLine($"[Startup/PrefetchClientTask] Downloading app HTML from {url}");
        var html = (await GetHtmlFormatted(url)).Split('\n');
        Console.WriteLine("[Startup/PrefetchClientTask] Fetching missing assets...");
        var assets = new List<string>();
        MemoryStore.ClientScripts.Clear();
        MemoryStore.ClientPreloadScripts.Clear();
        MemoryStore.ClientStylesheets.Clear();
        //css
        var cssHtml = html.Where(x => x.Contains("<link rel=\"stylesheet\"")).ToList();
        foreach (var script in cssHtml)
        {
            var match = Regex.Match(script, "href=\"(.*?)\"");
            if (match.Success)
            {
                assets.Add(match.Groups[1].Value);
                MemoryStore.ClientStylesheets.Add(match.Groups[1].Value);
            }
        }

        //scripts
        var mainScriptHtml = html.Where(x => x.Contains("<script src=")).ToList();
        foreach (var script in mainScriptHtml)
        {
            var match = Regex.Match(script, "src=\"(.*?)\"");
            if (match.Success)
            {
                assets.Add(match.Groups[1].Value);
                MemoryStore.ClientScripts.Add(match.Groups[1].Value);
            }
        }

        //preload
        var prefetchScriptHtml = html.Where(x => x.Contains("link rel=\"prefetch\" as=\"script\"")).ToList();
        foreach (var script in prefetchScriptHtml)
        {
            var match = Regex.Match(script, "href=\"(.*?)\"");
            if (match.Success)
            {
                assets.Add(match.Groups[1].Value);
                MemoryStore.ClientPreloadScripts.Add(match.Groups[1].Value);
            }
        }

        var assettasks = assets
            .Select(TieredAssetStore.GetAsset).ToList();
        await Task.WhenAll(assettasks);
        Console.WriteLine("Downloading done!!!!!");
    }

    private static async Task<string> GetHtmlFormatted(string url)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var document = new HtmlParser().ParseDocument(html);
        var sw = new StringWriter();
        document.ToHtml(sw, new PrettyMarkupFormatter());
        return sw.ToString();
    }
}