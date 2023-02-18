using CMS.Core.Constants;
using CMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookStoreController : BaseApiController
    {
        private readonly IBookStoreQueryService _bookStoreQueryService;

        public BookStoreController(IBookStoreQueryService bookStoreQueryService)
        {
            _bookStoreQueryService = bookStoreQueryService;
        }

        [HttpGet]
        [Route("get-book")]
        public async Task<IActionResult> GetBookFromStoredProcedure()
        {
            var id = 1;
            var book = _bookStoreQueryService.GetBookStoreFromStoredProcedure(id);

            return Ok(book);
        }

        [HttpGet]
        [Route("get-book-with-admin")]
        [Authorize(Roles = RoleConstants.PermisstionType.Administrators)]
        public async Task<IActionResult> OnlyAdmin()
        {
            var id = 2;
            var book = _bookStoreQueryService.GetBookStoreFromStoredProcedure(id);

            return Ok(book);
        }
    }
}
