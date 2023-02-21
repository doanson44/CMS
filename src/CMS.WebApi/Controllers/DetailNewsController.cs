using System;
using System.Threading.Tasks;
using CMS.Core.Domains;
using CMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApi.Controllers;

[Route("cms/detailnews")]
[ApiController]
[Authorize]
public class DetailNewsController : BaseApiController
{
    private readonly IDetailNewsService _detailNewsService;

    public DetailNewsController(IDetailNewsService detailNewsService)
    {
        _detailNewsService = detailNewsService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] DetailNewsQueryParam request)
    {
        request.CurrentUser = CurrentUser;
        var result = await _detailNewsService.GetAllAsync(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var result = await _detailNewsService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] DetailNewsRequest request)
    {
        await _detailNewsService.CreateAsync(request);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromForm] DetailNewsRequest request)
    {
        await _detailNewsService.UpdateAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _detailNewsService.DeleteAsync(id);
        return Ok();
    }
}
