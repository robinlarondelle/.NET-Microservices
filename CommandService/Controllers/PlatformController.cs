using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/command-service/[controller]")]
[ApiController]
public class PlatformController : ControllerBase
{
    public PlatformController()
    {
        
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("request received");
        return Ok("request received");
    }
}