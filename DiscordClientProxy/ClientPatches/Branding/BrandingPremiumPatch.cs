using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches.Branding;

public class BrandingPremiumPatch : ClientPatch
{
    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("Nitro")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        content = content.Replace("Discord Nitro", $"{Configuration.Instance.InstanceName} Premium"));
        content = content.Replace("\"Nitro\"", "\"Premium\"");
        content = content.Replace("Nitro ", "Premium ");
        content = content.Replace(" Nitro", " Premium");
        content = content.Replace("[Nitro]", "[Premium]");
        content = content.Replace("*Nitro*", "*Premium*");
        content = content.Replace("\"Nitro . ", "Premium. ");
        return content;
    }
}
