using Application.Contract;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUser user) : Controller
    {
        private readonly IUser _user = user;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _user.GetAsync();
            return result.Any() ? Ok(result) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _user.GetByIdAsync(id);
            return result is not null ? Ok(result) : NotFound();

        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            var result = await _user.AddAsync(user);
            return result.Flag ? Ok(User) : throw new Exception(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            var result = await _user.UpdateAsync(user);
            return result.Flag ? Ok(result) : throw new Exception(result.Message);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _user.DeleteAsync(id);
            return result.Flag ? Ok(result) : throw new Exception(result.Message);
        }
    }
}
