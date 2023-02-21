using System.Threading.Tasks;
using CMS.Core.Domains;
using CMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoController : BaseApiController
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] TodoQueryParam request)
    {
        var result = await _todoService.GetAllAsync(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var result = await _todoService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(TodoRequest request)
    {
        await _todoService.CreateAsync(request);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, TodoRequest request)
    {
        await _todoService.UpdateAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _todoService.DeleteAsync(id);
        return Ok();
    }
}
