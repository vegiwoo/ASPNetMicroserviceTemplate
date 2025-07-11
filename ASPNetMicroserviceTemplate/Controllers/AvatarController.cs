using ASPNetMicroserviceTemplate.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetMicroserviceTemplate.Controllers;

[ApiController, Route("api/avatar")]
public class AvatarController(RobohashClient robohashClient) : ControllerBase
{
    private readonly RobohashClient _robohashClient = robohashClient;

    // GET /api/avatar/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAvatar(string id)
    {
        var bytes = await _robohashClient.GetAvatarAsync(id);

        // ASP.NET Core вернёт правильный Content-Type и длину
        return File(bytes, "image/png");
    }
}