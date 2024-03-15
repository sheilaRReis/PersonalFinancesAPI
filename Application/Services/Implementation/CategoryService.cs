using Application.DTOs;
using Application.Contract;
using Domain.Entities;
using System.Net.Http.Json;

namespace Application.Services.Implementation
{
    public class CategoryService : ICategory
    {
        private readonly HttpClient _httpClient;
        private const string CategoryEndpoint = "api/category";

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse> AddAsync(Category category)
        {
            var data = await _httpClient.PostAsJsonAsync(CategoryEndpoint, category);
            var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
            return response;
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{CategoryEndpoint}/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }

        public async Task<List<Category>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Category>>(CategoryEndpoint);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Category>($"{CategoryEndpoint}/{id}");
        }

        public async Task<ServiceResponse> UpdateAsync(Category category)
        {
            var response = await _httpClient.PutAsJsonAsync($"{CategoryEndpoint}/{category.Id}", category);
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }
    }
}
