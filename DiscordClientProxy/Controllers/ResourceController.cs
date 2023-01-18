using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClientProxy.Controllers;

[ApiController]
[Route("/resources")]
public class ResourceController : ControllerBase
{
    private readonly ILogger<ResourceController> _logger;

    public ResourceController(ILogger<ResourceController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/resources/welcome-screen")]
    public async Task<object> WelcomeScreenContent()
    {
        return File(Encoding.UTF8.GetBytes(await System.IO.File.ReadAllTextAsync(RuntimeEnvironment.BinDir + "/Resources/Overridable/WelcomeScreen/index.html")), "text/html");
    }
}