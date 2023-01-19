using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class AlternateSentryPatch : ClientPatch
{
    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("sentry.io")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        content = content.Replace("https://fa97a90475514c03a42f80cd36d147c4@sentry.io/140984",
            "https://6bad92b0175d41a18a037a73d0cff282@sentry.thearcanebrony.net/12");
        return content;
    }
}