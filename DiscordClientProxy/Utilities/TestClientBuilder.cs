using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Html;
using AngleSharp.Html.Parser;

namespace DiscordClientProxy.Utilities;

public class TestClientBuilder
{
    public static async Task<byte[]> BuildClientPage()
    {
        if (Configuration.Instance.Cache.ReuseHtml && MemoryStore.ClientPageHtml != null)
            return MemoryStore.ClientPageHtml;
        Console.WriteLine("[TestClientBuilder] Building client page...");
        //var html = "";
        var html = await File.ReadAllTextAsync(RuntimeEnvironment.BinDir + "/Resources/Pages/index.html");

        html = AddScripts(html);
        html = await AddDebugUtils(html);
        //add welcome screen
        html +=
            (await File.ReadAllTextAsync(RuntimeEnvironment.BinDir + "/Resources/Private/WelcomeScreenWrapper.html"))
            .Replace("<!-- content -->",
                await File.ReadAllTextAsync(RuntimeEnvironment.BinDir +
                                            "/Resources/Overridable/WelcomeScreen/index.html"));
        MemoryStore.ClientPageHtml = Encoding.UTF8.GetBytes(html);
        await File.WriteAllTextAsync("last_index.html", html);
        return MemoryStore.ClientPageHtml;
    }


    public static async Task<byte[]> BuildDevPage()
    {
        if (Configuration.Instance.Cache.ReuseHtml && MemoryStore.DevPageHtml != null)
            return MemoryStore.DevPageHtml;
        Console.WriteLine("[TestClientBuilder] Building dev page...");
        //var html = "";
        var html = await File.ReadAllTextAsync(RuntimeEnvironment.BinDir + "/Resources/Pages/developers.html");

        html = AddScripts(html);
        html = await AddDebugUtils(html);
        
        MemoryStore.DevPageHtml = Encoding.UTF8.GetBytes(html);
        await File.WriteAllTextAsync("last_dev.html", html);
        
        return MemoryStore.DevPageHtml;
    }
    
    private static string AddScripts(string html)
    {
        if (Configuration.Instance.Client.AddPrefetchTags)
            html = html.Replace("<!--prefetch_script-->",
                string.Join("\n",
                    MemoryStore.ClientPreloadScripts.Select(
                        x => $"<link rel=\"prefetch\" as=\"script\" href=\"{x}\" />")));
        html = html.Replace("<!--client_script-->",
            string.Join("\n", MemoryStore.ClientScripts.Select(x => $"<script src=\"{x}\"></script>")));
        html = html.Replace("<!--client_css-->",
            string.Join("\n", MemoryStore.ClientStylesheets.Select(x => $"<link rel=\"stylesheet\" href=\"{x}\" />")));
        return html;
    }

    private static async Task<string> AddDebugUtils(string html)
    {
        //inject debug utilities
        var debugOptions = Configuration.Instance.Client.DebugOptions;
        if (debugOptions.DumpWebsocketTrafficToBrowserConsole)
            html = html.Replace("<!-- preload plugin marker -->",
                await File.ReadAllTextAsync(RuntimeEnvironment.BinDir +
                                            "/Resources/Private/Injections/WebSocketDataLog.html") +
                "\n<!-- preload plugin marker -->");
        if (debugOptions.DumpWebsocketTraffic)
            html = html.Replace("<!-- preload plugin marker -->",
                await File.ReadAllTextAsync(RuntimeEnvironment.BinDir +
                                            "/Resources/Private/Injections/WebSocketDumper.html") +
                "\n<!-- preload plugin marker -->");
        return html;
    }
}