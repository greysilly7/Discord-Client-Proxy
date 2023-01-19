using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class GlobalEnvWarningPatch : ClientPatch
{
    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("Global environment")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        content = content.Replace("Global environment variables not set!", "Global environment variables not set!\\n[DiscordClientProxy] Make sure your reverse proxy is configured correctly!");
        return content;
    }
}