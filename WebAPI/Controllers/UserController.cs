using Application.Contract;
using Application.Helper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser _user;
        private readonly HandleExceptionHelper _handleExceptionHelper;

        public UserController(IUser user, HandleExceptionHelper handleExceptionHelper)
        {
            _handleExceptionHelper = handleExceptionHelper;
            _user = user;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _user.GetAsync();
                return result.Any() ? Ok(result) : NotFound("There are no registered transactions");
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

                var result = await _user.GetByIdAsync(id);
                return result !=null ? Ok(result) : NotFound($"Could not find any transaction with the provided id: {id} ");
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
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Invalid category data provided.");

                User createdUser = new()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Username = user.Username,
                    Password = user.Password
                };
                var result = await _user.AddAsync(createdUser);
                return result.Flag ? Ok(createdUser) : throw new Exception(result.Message);
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
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Invalid category data provided.");

                if (id <= 0)
                    return BadRequest("Invalid ID provided.");

                var foundUser = await _user.GetByIdAsync(id);
                foundUser.Username = user.Username;
                foundUser.Name = user.Name;
                foundUser.Password = user.Password;
                
                var result = await _user.UpdateAsync(foundUser);
                return result.Flag ? Ok(foundUser) : NotFound(result.Message);
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

                var result = await _user.DeleteAsync(id);
                return result.Flag ? Ok(result) : NotFound(result.Message);
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
