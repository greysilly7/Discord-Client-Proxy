using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class PlainTextGatewayPatch : ClientPatch
{
    public override bool IsEnabledByDefault { get; set; } = false;

    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("isDiscordGatewayPlaintextSet")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        content = content.Replace("e.isDiscordGatewayPlaintextSet=function(){0;return!1};",
            "e.isDiscordGatewayPlaintextSet = function() { return true };");
        return content;
    }
}