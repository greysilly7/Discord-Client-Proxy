namespace DiscordClientProxy;

public class ResourceDirHandler
{
    public static void CreateResourceDirInfo()
    {
        Directory.CreateDirectory("Resources");
        File.WriteAllLines("Resources/README.txt", new []
        {
            "This directory is used to override the default resources that are served.",
            "To override a resource, simply place a file with the same name in this directory.",
            $"For a list of resources, see {RuntimeEnvironment.BinDir}/Resources/Overridable/",
            "Files in this directory will be served instead of the default resources. Missing files will be served as normal."
        });
    }
}