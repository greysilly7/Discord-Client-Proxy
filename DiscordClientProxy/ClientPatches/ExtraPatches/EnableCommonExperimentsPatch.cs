using DiscordClientProxy.Classes;

namespace DiscordClientProxy.ClientPatches;

public class EnableCommonExperimentsPatch : ClientPatch
{
    public override bool IsEnabledByDefault { get; set; } = true;

    public override async Task<string> ApplyPatch(string content)
    {
        if (!content.Contains("defaultConfig")) return content;
        Console.WriteLine($"[ClientPatch:{GetType().Name}] Applying patch...");
        content = content.Replace("{canViewProfilePanel:!1,canSeeThemeColors:!1}", "{canViewProfilePanel:true,canSeeThemeColors:true}");
        content = content.Replace("{enabledAMOLEDThemeOption:!1}", "{enabledAMOLEDThemeOption:true}");
        content = content.Replace("{showPronouns:!1}", "{showPronouns:true}");
        content = content.Replace("{hasClientThemes:!1}", "{hasClientThemes:true}");
        content = content.Replace("{inProfileViewsExperiment:!1,inImprovedUpsellExperiment:!1,inThemesExperiment:!1}", "{inProfileViewsExperiment:true,inImprovedUpsellExperiment:true,inThemesExperiment:true}");
        content = content.Replace("{canViewThemes:!1,canEditThemes:!1,canTryItOut:!1,canViewTryItOut:!1}", "{canViewThemes:true,canEditThemes:true,canTryItOut:true,canViewTryItOut:true}");
        content = content.Replace("{manualApprovalEnabled:!1}", "{manualApprovalEnabled:true}");
        content = content.Replace("{showNewUnreadsBar:!1}", "{showNewUnreadsBar:true}");
        content = content.Replace("isPopupEnabled:!1,isTaglessAccountPanelEnabled:!1,isJoinedTagButtonEnabled:!1}", "isPopupEnabled:true,isTaglessAccountPanelEnabled:false,isJoinedTagButtonEnabled:true}");
        content = content.Replace("{allowRoleStyles:!1}", "{allowRoleStyles:true}");
        content = content.Replace("{canUseAvatarDecorations:!1}", "{canUseAvatarDecorations:true}");
        //funny random unknown stuff
        content = content.Replace("{allowChurroSwitching:!1,allowChurroUpsells:!1,allowChurroStaging:!1}", "{allowChurroSwitching:true,allowChurroUpsells:true,allowChurroStaging:true}");
        content = content.Replace("{allowForcedColors:!1}", "{allowForcedColors:true}");
        
        //forum channels?
        //label:\"Threads only channel\",kind:\"guild\",defaultConfig:{enabled:!1}
        
        //maybes:
        //label:\"Voice in Threads\",kind:\"guild\",defaultConfig:{enabled:!1}
        //label:\"AA guild test for the new experiment config UI\",defaultConfig:{isEnabled:!1},
        //label:\"Show the button to join the admin guild in community overview guild settings page\",defaultConfig:{enabled:!1}
        //label:\"Timestamp Tooltip\",kind:\"user\",defaultConfig:{enabled:!1}
        //label:\"Auth Sessions User Settings\",defaultConfig:{showSettings:!1}
        //label:\"WebAuthn\",kind:\"user\",defaultConfig:{enabled:!1}
        return content;
    }
}