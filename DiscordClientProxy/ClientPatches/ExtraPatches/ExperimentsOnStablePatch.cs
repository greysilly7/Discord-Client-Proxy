using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class ExperimentsOnStablePatch : ClientPatch
{
    public override bool IsEnabledByDefault { get; set; } = true;

    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("=\"staging\"===window.GLOBAL_ENV.RELEASE_CHANNEL")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        var lines = content.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.Contains("userExperimentOverrides"))
            {
                lines[i] = line.Replace("=\"staging\"===window.GLOBAL_ENV.RELEASE_CHANNEL", "=true");
            }
        }
        return content;
    }
}