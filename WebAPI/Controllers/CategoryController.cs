using Application.Contract;
using Application.DTOs;
using Application.Helper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;
        private readonly HandleExceptionHelper _handleExceptionHelper;

        public CategoryController(ICategory categoryRepository, HandleExceptionHelper handleExceptionHelper)
        {
            _category = categoryRepository;
            _handleExceptionHelper = handleExceptionHelper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = await _category.GetAsync();
                return categories?.Any()==true ? Ok(categories) : NotFound("There are no registered categories");
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Service unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return _handleExceptionHelper.HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid ID provided.");

                var category = await _category.GetByIdAsync(id);
                return category!=null ? Ok(category) : NotFound($"Could not find any category with the provided id: {id} ");
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Service unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return _handleExceptionHelper.HandleException(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> Create([FromBody] CategoryDto category)
        {
            try
            {
                if (category == null)
                    return BadRequest("Invalid category data provided.");
                
                Category createdCategory = new() { 
                    Name = category.Name,
                    Description = category.Description,
                };
                var result = await _category.AddAsync(createdCategory);
                return result.Flag ? Ok(createdCategory) : NotFound(result.Message);
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Service unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return _handleExceptionHelper.HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto category)
        {
            try
            {
                if (category == null)
                    return BadRequest("Invalid category data provided.");

                if (id <= 0)
                    return BadRequest("Invalid ID provided.");

                var foundCategory = await _category.GetByIdAsync(id);
                foundCategory.Description = category.Description;
                foundCategory.Name =  category.Name;
                var updateResult = await _category.UpdateAsync(foundCategory);
                return updateResult.Flag ? Ok(foundCategory) : NotFound(updateResult.Message);
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Service unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return _handleExceptionHelper.HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid ID provided.");

                var result = await _category.DeleteAsync(id);
                return result.Flag? Ok("Category deleted") : NotFound(result.Message);
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Service unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                return _handleExceptionHelper.HandleException(ex);
            }
        }
    }
}
