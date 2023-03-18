using System.Threading.Tasks;
using CMS.Core.Constants;
using CMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CMS.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class BookStoreController : BaseApiController
{
    private readonly IBookStoreQueryService _bookStoreQueryService;
    private readonly ILogger<BookStoreController> _logger;

    public BookStoreController(IBookStoreQueryService bookStoreQueryService, ILogger<BookStoreController> logger)
    {
        _bookStoreQueryService = bookStoreQueryService;
        _logger = logger;
    }

    [HttpGet]
    [Route("get-book")]
    public async Task<IActionResult> GetBookFromStoredProcedure()
    {
        _logger.LogInformation($"GetBookFromStoredProcedure id = {1}");
        var id = 1;
        var book = _bookStoreQueryService.GetBookStoreFromStoredProcedure(id);

        return Ok(book);
    }

    [HttpGet]
    [Route("get-book-with-admin")]
    [Authorize(Roles = RoleConstants.PermisstionType.Administrators)]
    public async Task<IActionResult> OnlyAdmin()
    {
        _logger.LogInformation($"OnlyAdmin id = {2}");
        var id = 2;
        var book = _bookStoreQueryService.GetBookStoreFromStoredProcedure(id);

        return Ok(book);
    }
}
