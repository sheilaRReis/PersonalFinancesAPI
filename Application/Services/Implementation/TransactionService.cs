using Application.DTOs;
using Application.Contract;
using Domain.Entities;
using System.Net.Http.Json;

namespace Application.Services.Implementation
{
    public class TransactionService : ITransaction
    {
        private readonly HttpClient _httpClient;
        private const string TransactionEndpoint = "api/transaction";

        public TransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse> AddAsync(Transaction transaction)
        {
            var response = await _httpClient.PostAsJsonAsync(TransactionEndpoint, transaction);
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{TransactionEndpoint}/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }

        public async Task<List<Transaction>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Transaction>>(TransactionEndpoint);
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Transaction>($"{TransactionEndpoint}/{id}");
        }

        public async Task<ServiceResponse> UpdateAsync(Transaction transaction)
        {
            var response = await _httpClient.PutAsJsonAsync($"{TransactionEndpoint}/{transaction.Id}", transaction);
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }
    }
}
