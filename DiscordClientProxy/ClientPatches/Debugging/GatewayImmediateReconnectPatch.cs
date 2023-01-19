using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class GatewayImmediateReconnectPatch : ClientPatch
{
    public override bool IsEnabledByDefault { get; set; } = false;

    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("nextReconnectIsImmediate")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        content = content.Replace("nextReconnectIsImmediate=!1", "nextReconnectIsImmediate = true");
        return content;
    }
}