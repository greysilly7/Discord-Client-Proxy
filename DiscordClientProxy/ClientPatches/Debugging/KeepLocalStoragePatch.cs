using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class KeepLocalStoragePatch : ClientPatch
{
    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("delete window.localStorage")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        content = content.Replace(
            "delete window.localStorage",
            "console.log('Prevented deletion of localStorage')"
        );
        return content;
    }
}