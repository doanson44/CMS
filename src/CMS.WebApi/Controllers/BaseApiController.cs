using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApi.Controllers;

// No longer used - shown for reference only if using full controllers instead of Endpoints for APIs
[Route("api/[controller]/[action]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    public string? CurrentUser { get => User?.Identity?.Name; }
}
