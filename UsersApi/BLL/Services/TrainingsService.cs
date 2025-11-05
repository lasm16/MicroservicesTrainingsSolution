using UsersApi.Abstractions;
using UsersApi.BLL.DTOs;
using UsersApi.Properties;

namespace UsersApi.BLL.Services
{
    public class TrainingsService(IHttpClientFactory httpClientFactory) : ITrainingsService
    {
        private const string Endpoint = "api/v1/Trainings/user";

        public async Task<List<TrainingDto>> GetAllTrainings(int userId)
        {
            try
            {
                var client = httpClientFactory.CreateClient(HttpClientConfig.TrainingsClient);
                using var response = await client.GetAsync($"{Endpoint}/{userId}");
                response.EnsureSuccessStatusCode();
                var list = await response.Content.ReadFromJsonAsync<List<TrainingDto>>();
                return list ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обращении к TrainingsService: {ex.InnerException}");
                return [];
            }
            
        }
    }
}
