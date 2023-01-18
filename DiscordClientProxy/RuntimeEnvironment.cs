namespace DiscordClientProxy;

public static class RuntimeEnvironment
{
    public static string BaseDir =>
        (System.Environment.GetEnvironmentVariable("BASE_DIR") ?? System.Environment.CurrentDirectory)
        .Replace("~",
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));

    public static string BinDir =>
        new FileInfo(typeof(Program).Assembly.Location).DirectoryName;
}