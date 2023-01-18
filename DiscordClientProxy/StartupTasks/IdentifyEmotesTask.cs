using System.Text.Json;
using DiscordClientProxy.Interfaces;
using DiscordClientProxy.Utilities;

namespace DiscordClientProxy.StartupTasks;

public class IdentifyEmotesTask : IStartupTask
{
    public int Order { get; } = 50;

    public async Task ExecuteAsync()
    {
        return;
        Console.WriteLine("Identifying original emote urls...");
        //get all svgs in asset dir
        var assets = Directory.GetFiles(Configuration.Instance.AssetCacheLocationResolved).Where(x=>x.EndsWith(".svg")).ToList();
        var emotes = Directory.GetFiles("twemoji/assets/svg").ToList();
        Console.WriteLine($"Found {assets.Count} assets downloaded, and {emotes.Count} emotes in twemoji.");
        //read all files into memory
        Console.WriteLine("Reading files into memory...");
        var assetDict = new Dictionary<string, string>();
        foreach (var asset in assets)
        {
            assetDict.Add(Path.GetFileName(asset), await File.ReadAllTextAsync(asset));
            Console.Write($"Read {assetDict.Count} assets...\r");
        }
        Console.WriteLine();
        var emoteDict = new Dictionary<string, string>();
        foreach (var emote in emotes)
        {
            emoteDict.Add(Path.GetFileName(emote), await File.ReadAllTextAsync(emote));
            Console.Write($"Read {emoteDict.Count} emotes...\r");
        }
        Console.WriteLine();
        Console.WriteLine("Comparing files...");
        //find identical content, in a dictionary
        var identical = new Dictionary<string, string>();
        foreach (var (key, value) in assetDict)
        {
            if (emoteDict.ContainsValue(value))
            {
                identical.Add(key, emoteDict.First(x => x.Value == value).Key);
            }
        }
        Console.WriteLine($"Found {identical.Count} matches!");
        await using var fs = File.OpenWrite("emotes.json");
        await JsonSerializer.SerializeAsync(fs, identical, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        await fs.FlushAsync();
        fs.Close();
        Console.WriteLine("asdf");
        
    }
}