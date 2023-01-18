using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class ForceIsStaffPatch : ClientPatch
{
    public override bool IsEnabledByDefault { get; set; } = false;

    public override async Task<string> ApplyPatch(string content)
    {
        if (!(content.Contains("isStaff=function") || content.Contains("isStaffPersonal=function"))) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        content = content.Replace("isStaff=function(){return!1}", "isStaff=function(){return true}");
        content = content.Replace("isStaffPersonal=function(){return!1}", "isStaffPersonal=function(){return true}");
        content = content.Replace("isStaffPersonal:{writable:!1,configurable:!1,value:function(){", "isStaffPersonal:{writable:!1,configurable:!1,value:function(){return true;");
        return content;
    }
}