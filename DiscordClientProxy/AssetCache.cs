using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using DiscordClientProxy.Utilities;

namespace DiscordClientProxy;

public class AssetCache
{
    public static async Task FindEmojiMatches()
    {
        if (!Directory.Exists("twemoji"))
        {
            Console.WriteLine("twemoji not found, skipping emoji matching");
            return;
        }

        using (SHA256 sha256Hash = SHA256.Create())
        {
            Console.WriteLine("Getting twemoji hashes...");
            var twemojiHashes = Directory.GetFiles("twemoji/assets/svg")
                .Select(x => (x, BytesToString(SHA256.Create().ComputeHash(File.OpenRead(x)))))
                .ToDictionary(x => x.Item2, x => x.Item1);
            Console.WriteLine("Getting asset hashes...");
            var assetHashes = Directory.GetFiles(Configuration.Instance.AssetCacheLocationResolved)
                .Where(x => x.EndsWith(".svg"))
                .Select(x => (x, BytesToString(SHA256.Create().ComputeHash(File.OpenRead(x)))))
                .ToDictionary(x => x.Item2, x => x.Item1);
            //match filenames based on hashes
            Console.WriteLine("Matching...");
            var matches = assetHashes.Where(x => twemojiHashes.ContainsKey(x.Key)).ToDictionary(x => x.Value, x => twemojiHashes[x.Key]);
            Console.WriteLine();
        }
    }

    private static string BytesToString(byte[] bytes)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }

        return builder.ToString();
    }
}