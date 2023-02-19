using CMS.Core.Domains;
using CMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.WebApi.Controllers
{
    [Route("ntt/categorynews")]
    [ApiController]
    [Authorize]
    public class CategoryNewsController : Controller
    {
        private readonly ICategoryNewsService categoryNewsService;

        public CategoryNewsController(ICategoryNewsService categoryNewsService)
        {
            this.categoryNewsService = categoryNewsService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync([FromQuery] CategoryNewsQueryParam request)
        {
            var result = await categoryNewsService.GetAllAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var result = await categoryNewsService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CategoryNewsRequest request)
        {
            await categoryNewsService.CreateAsync(request);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, CategoryNewsRequest request)
        {
            await categoryNewsService.UpdateAsync(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await categoryNewsService.DeleteAsync(id);
            return Ok();
        }
    }
}
