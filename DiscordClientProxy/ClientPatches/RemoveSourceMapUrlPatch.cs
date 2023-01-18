using System.Text.RegularExpressions;
using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class RemoveSourceMapUrlPatch : ClientPatch
{
    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("sourceMappingURL")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Removing source map url from client");
        content = Regex.Replace(content, @"\/\/# sourceMappingURL=.*\.map", "");
        content = Regex.Replace(content, @"/\*# sourceMappingURL=.*\.map \*/", "");
        //content = content.Replace("//# sourceMappingURL=", "//# disabledSourceMappingURL=");
        return content;
    }
}