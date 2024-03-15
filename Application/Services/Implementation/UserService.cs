using Application.Contract;
using Application.DTOs;
using Domain.Entities;
using System.Net.Http.Json;

namespace Application.Services.Implementation
{
    public class UserService : IUser
    {
        private readonly HttpClient _httpClient;
        private const string UserEndpoint = "api/user";
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse> AddAsync(User user)
        {
            var data = await _httpClient.PostAsJsonAsync(UserEndpoint, user);
            var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
            return response;
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{UserEndpoint}/{id}");
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }

        public async Task<List<User>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<User>>(UserEndpoint);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<User>($"{UserEndpoint}/{id}");
        }

        public async Task<ServiceResponse> UpdateAsync(User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"{UserEndpoint}/{user.Id}", user);
            return await response.Content.ReadFromJsonAsync<ServiceResponse>();
        }
    }
}
