using Application.Contract;
using Application.DTOs;
using Application.Helper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Transactions;
using Transaction = Domain.Entities.Transaction;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _transaction;
        private readonly HandleExceptionHelper _handleExceptionHelper;

        public TransactionController(ITransaction transactionRepository, HandleExceptionHelper handleExceptionHelper)
        {
            _transaction = transactionRepository;
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
                var transactions = await _transaction.GetAsync();
                return transactions.Any() ? Ok(transactions) : NotFound("There are no registered transactions");
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

                var transaction = await _transaction.GetByIdAsync(id);
                return transaction!=null ? Ok(transaction) : NotFound($"No transactions were found with the provided id: {id}");
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
        public async Task<IActionResult> Create([FromBody] TransactionDto transaction)
        {
            try
            {
                if (transaction == null)
                    return BadRequest("Invalid category data provided.");

                Transaction createdTransaction = new()
                {
                    Description = transaction.Description,
                    IdCategory = transaction.IdCategory,
                    Date = transaction.Date,
                    IdUser = transaction.IdUser,
                    Value = transaction.Value,
                    Type = transaction.Type
                };
                var result = await _transaction.AddAsync(createdTransaction);
                return result.Flag ? Ok(createdTransaction) : NotFound(result.Message);
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
        public async Task<IActionResult> Update(int id, [FromBody] TransactionDto transaction)
        {
             try
            {
                if (transaction == null)
                    return BadRequest("Invalid category data provided.");

                if (id <= 0)
                    return BadRequest("Invalid ID provided.");
                var loadedTransaction = await LoadDataFromDto(id, transaction);
                var result = await _transaction.UpdateAsync(loadedTransaction);
                return result.Flag ? Ok(loadedTransaction) : NotFound(result.Message);
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID provided.");

            try
            {
                var result = await _transaction.DeleteAsync(id);
                return result.Flag ? Ok("Transaction created") : NotFound(result.Message);
            }
            catch (Exception ex)
            {
                return _handleExceptionHelper.HandleException(ex);
            }
        }
        private async Task<Transaction> LoadDataFromDto(int id, TransactionDto transaction)
        {
            var foundTransaction = await _transaction.GetByIdAsync(id);
            foundTransaction.Description = transaction.Description;
            foundTransaction.IdCategory = transaction.IdCategory;
            foundTransaction.Date = transaction.Date;
            foundTransaction.IdUser = transaction.IdUser;
            foundTransaction.Value = transaction.Value;
            foundTransaction.Type = transaction.Type;
            return foundTransaction;
        }
    }

}
