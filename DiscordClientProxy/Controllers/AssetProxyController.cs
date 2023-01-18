using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClientProxy.Controllers;

[ApiController]
[Route("/assets")]
public class AssetProxyController : ControllerBase
{
    private readonly ILogger<AssetProxyController> _logger;

    public AssetProxyController(ILogger<AssetProxyController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/assets/{*res:required}")]
    public async Task<object> Asset(string res)
    {
        //we dont have map files, so dont even bother
        if (res.EndsWith(".map")) return NotFound();
        if (Regex.IsMatch(res, "version\\..*\\.json")) return NotFound();
        //if (res.EndsWith(".svg")) return Redirect("https://raw.githubusercontent.com/twitter/twemoji/master/assets/svg/1f004.svg");

        byte[] data = await TieredAssetStore.GetAsset(res);

        if (data is null) return NotFound();
        return File(data, HttpUtilities.GetContentTypeByFilename(res));
    }
}