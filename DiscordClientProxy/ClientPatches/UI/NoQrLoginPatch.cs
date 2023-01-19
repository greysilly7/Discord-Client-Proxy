using System.Text.RegularExpressions;
using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class NoQrLoginPatch : ClientPatch
{
    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("delete window.localStorage")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        Console.WriteLine("This patch is broken, please fix!");
        content = Regex.Replace(content, @"\w\?\(\d,\w\.jsx\)\(\w*\,{authTokenCallback:this\.handleAuthToken}\):null", "null");
        //original for reference...
        //content = content.replaceAll(/\w\?\(\d,\w\.jsx\)\(\w*\,{authTokenCallback:this\.handleAuthToken}\):null/g, "null");
        return content;
    }
}