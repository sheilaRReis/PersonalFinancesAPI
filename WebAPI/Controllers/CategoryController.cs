using Application.Contract;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;

        public CategoryController(ICategory categoryRepository)
        {
            _category = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _category.GetAsync();
            return categories.Any() ? Ok(categories) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _category.GetByIdAsync(id);
            return category!=null ? Ok(category) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            var result = await _category.AddAsync(category);
            return result.Flag ? Ok(User) : throw new Exception(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            category.Id = id;
            var result = await _category.UpdateAsync(category);
            return result.Flag ? Ok(User) : throw new Exception(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _category.DeleteAsync(id);
            return result.Flag ? Ok(result) : throw new Exception(result.Message);
        }
    }
}
