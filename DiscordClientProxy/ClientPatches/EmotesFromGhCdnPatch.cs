using System.Text.Json;
using System.Text.RegularExpressions;
using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class EmotesFromGhCdnPatch : ClientPatch
{
    private static Dictionary<string,string> _emotes = new Dictionary<string, string>();
    public override async Task<string> ApplyPatch(string content)
    {
        return content;
        if (!content.Contains(".svg")) return content;
        if(!File.Exists("emotes.json"))
        {
            Console.WriteLine("Emoji map not found, not patching emotes!");
            return content;
        } else if (_emotes.Count == 0) {
            _emotes = JsonSerializer.Deserialize<Dictionary<string, string>>(await File.ReadAllTextAsync("emotes.json"));
        }
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        
        foreach (var (key, value) in _emotes)
        {
            content = Regex.Replace(content, $"(.)\\.exports=.\\..+\\\"{key}\\\"", "$1.exports=\"https://raw.githubusercontent.com/twitter/twemoji/master/assets/svg/"+value+"\"");           
        }
        
        
        return content;
    }
}