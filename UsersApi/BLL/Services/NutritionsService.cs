using UsersApi.Abstractions;
using UsersApi.BLL.DTOs;
using UsersApi.Properties;

namespace UsersApi.BLL.Services
{
    public class NutritionsService(IHttpClientFactory httpClientFactory) : INutritionsService
    {
        private const string Endpoint = "api/v1/Nutritions/user";

        public async Task<List<NutritionDto>> GetAllNutritions(int userId)
        {
            try
            {
                using var client = httpClientFactory.CreateClient(HttpClientConfig.NutritionsClient);
                using var response = await client.GetAsync($"{Endpoint}/{userId}");
                response.EnsureSuccessStatusCode();
                var list = await response.Content.ReadFromJsonAsync<List<NutritionDto>>();
                return list ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обращении к NutritionsService: {ex.InnerException}");
                return [];
            }
        }
    }
}
