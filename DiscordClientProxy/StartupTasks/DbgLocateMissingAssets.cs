using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using DiscordClientProxy.Interfaces;
using DiscordClientProxy.Utilities;

namespace DiscordClientProxy.StartupTasks;

public class DbgLocateMissingAssets : IStartupTask
{
    public int Order { get; } = 1000;

    public async Task ExecuteAsync()
    {
        return;
        if (!File.Exists("cache_misses")) return;
        File.Delete("missing_matches");
        var lines = (await File.ReadAllTextAsync("cache_misses")).Split('\n');
        Dictionary<string, string> cache = new();
        foreach (var asset in Directory.GetFiles("assets/latest"))
        {
            if(!(asset.EndsWith(".js") || asset.EndsWith(".css"))) continue;
            Console.Write($"Caching {asset}...                \r");
            cache.Add(asset, await File.ReadAllTextAsync(asset));
        }
        foreach (var line in lines)
        {
            if(line.Length < 5) continue;
            bool fullMatchFound = false;
            foreach (var (asset, file) in cache)
            {
                if (fullMatchFound) continue;
                Console.Write($"Looking for {line} in {asset}...                \r");
                var index = file.IndexOf(line);
                if(index == -1) index = file.IndexOf(line.Split('.')[0]);
                else fullMatchFound = true;
                if(index != -1)
                {
                    var match = String.Join("", file.Skip(Math.Max(index - 20, 0)).Take(line.Length + 40).ToList());
                    Console.WriteLine($"{line}: '{(fullMatchFound ? line : line.Split('.')[0])}' in {asset}#{index}: {match}");
                    Console.Write("");
                    await File.AppendAllTextAsync("missing_matches", $"{(fullMatchFound ? line : line.Split('.')[0])}\t{match}\n");
                    if (index != -1) fullMatchFound = true;
                }
            }
        }
        //Debugger.Break();
    }

    
}