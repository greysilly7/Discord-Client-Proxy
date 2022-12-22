using Newtonsoft.Json;

namespace DiscordClientProxy;

public class Configuration
{
    //This must be delayed until after working dir is set!
    public static void Load()
    {
        Instance = (File.Exists(Environment.BaseDir + "/config.json")
            ? JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Environment.BaseDir + "/config.json"))
            : new Configuration()) ?? new Configuration();
        File.WriteAllText(Environment.BaseDir + "/config.json", JsonConvert.SerializeObject(Instance, Formatting.Indented));
    }

    public static Configuration Instance { get; private set; } = new();

    public string Version { get; set; } = "latest";
    public string AssetCacheLocation { get; set; } = "assets_cache/$VERSION/";
    
    public ClientOptions Client { get; set; } = new();
    public CacheOptions Cache { get; set; } = new();
    
    [JsonIgnore] public string AssetCacheLocationResolved => AssetCacheLocation.Replace("$VERSION", Version);
}

public class CacheOptions
{
    public bool Disk { get; set; } = true;
    public bool Memory { get; set; } = true;
}

public class ClientOptions
{
    public ClientDebugOptions DebugOptions { get; set; } = new();
}

public class ClientDebugOptions
{
    public bool DumpWebsocketTrafficToBrowserConsole { get; set; } = false;
    public bool DumpWebsocketTraffic { get; set; } = false;
    public TestClientPatchOptions Patches { get; set; } = new();
}

public class TestClientPatchOptions
{
    public bool GatewayPlaintext { get; set; } = true;
    public bool NoXssWarning { get; set; } = true;
    public bool GatewayImmediateReconnect { get; set; } = true;
}