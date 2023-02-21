using System.Threading.Tasks;
using CMS.Core.Domains;
using CMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApi.Controllers;

[Route("cms/categorynews")]
[ApiController]
[Authorize]
public class CategoryNewsController : BaseApiController
{
    private readonly ICategoryNewsService _categoryNewsService;

    public CategoryNewsController(ICategoryNewsService categoryNewsService)
    {
        _categoryNewsService = categoryNewsService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] CategoryNewsQueryParam request)
    {
        var x = CurrentUser;
        var result = await _categoryNewsService.GetAllAsync(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var result = await _categoryNewsService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CategoryNewsRequest request)
    {
        await _categoryNewsService.CreateAsync(request);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, CategoryNewsRequest request)
    {
        await _categoryNewsService.UpdateAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _categoryNewsService.DeleteAsync(id);
        return Ok();
    }
}
