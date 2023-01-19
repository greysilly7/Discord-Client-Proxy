using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class FastIdentifyPatch : ClientPatch
{
    public override async Task<string> ApplyPatch(string content)
    {
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        content = content.Replace("e.isFastConnect=t;t?e._doFastConnectIdentify():e._doResumeOrIdentify()",
            "e.isFastConnect=t; if (t !== undefined) e._doResumeOrIdentify();");
        return content;
    }
}