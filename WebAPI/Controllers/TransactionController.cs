using Application.Contract;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _transaction;

        public TransactionController(ITransaction transactionRepository)
        {
            _transaction = transactionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var transactions = await _transaction.GetAsync();
            return transactions.Any() ? Ok(transactions) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _transaction.GetByIdAsync(id);
            return transaction!=null ? Ok(transaction) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Transaction transaction)
        {
            var result = await _transaction.AddAsync(transaction);
            return result.Flag ? Ok(User) : throw new Exception(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Transaction transaction)
        {
            transaction.Id = id;
            var result = await _transaction.UpdateAsync(transaction);
            return result.Flag ? Ok(User) : throw new Exception(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _transaction.DeleteAsync(id);
            return result.Flag ? Ok(User) : throw new Exception(result.Message);
        }
    }
}
