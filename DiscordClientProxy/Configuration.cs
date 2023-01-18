using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordClientProxy;

public class Configuration
{
    public static Configuration Instance { get; private set; } = new();

    public string Version { get; set; } = "latest";
    public string AssetCacheLocation { get; set; } = "assets/$VERSION/";
    public string InstanceName { get; set; } = "Fosscord";

    public ClientOptions Client { get; set; } = new();
    public CacheOptions Cache { get; set; } = new();
    public DebugOptions Debug { get; set; } = new();

    [JsonIgnore]
    public string AssetCacheLocationResolved => AssetCacheLocation.Replace("$VERSION", Version);

    //This must be delayed until after working dir is set!
    public static void Load()
    {
        Instance = (File.Exists("config.json")
            ? JsonSerializer.Deserialize<Configuration>(File.ReadAllText("config.json"))
            : new Configuration()) ?? new Configuration();
        Instance.Save();
    }

    public void Save()
    {
        File.WriteAllText("config.json", JsonSerializer.Serialize(Instance, new JsonSerializerOptions()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        }));
    }
}

public class DebugOptions
{
    public string? ClientEnvProxyUrl { get; set; } = null;
}

public class CacheOptions
{
    public bool Disk { get; set; } = true;
    public bool Memory { get; set; } = true;
    public bool ReuseHtml { get; set; } = true;
    public bool DownloadAssetsRecursive { get; set; } = true;
    public string AssetBaseUri { get; set; } = "https://discord.com/assets/";
    public string AppBaseUri { get; set; } = "https://canary.discord.com/app/";
    public string DevBaseUri { get; set; } = "https://canary.discord.com/developers/";
    
    public StartupCacheOptions StartupCacheOptions { get; set; } = new();
    public int RecursiveDownloadDepth { get; set; } = 10;
}

public class StartupCacheOptions
{
    public bool WipeCodeOnPatchlistChanged { get; set; } = true;
    public bool WipeCodeOnStart { get; set; } = false;
    public bool WipeAllOnStart { get; set; } = false;
    public bool DownloadOnStart { get; set; } = true;
}

public class ClientOptions
{
    public bool AddPrefetchTags { get; set; } = true;
    public ClientDebugOptions DebugOptions { get; set; } = new();
}

public class ClientDebugOptions
{
    public bool DumpWebsocketTrafficToBrowserConsole { get; set; } = false;
    public bool DumpWebsocketTraffic { get; set; } = false;
    public Dictionary<string, bool> Patches { get; set; } = new();
}