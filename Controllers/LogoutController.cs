
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

// [Route("api/[controller]")]
[Route("api/logout")]
[ApiController]
public class LogoutController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        await HttpContext.SignOutAsync("Identity.Application");
        return Ok();
    }
}
