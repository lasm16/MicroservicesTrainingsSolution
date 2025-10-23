using UsersApi.Abstractions;
using UsersApi.BLL.DTOs;
using UsersApi.Properties;

namespace UsersApi.BLL.Services
{
    public class TrainingsService(IHttpClientFactory httpClientFactory) : ITrainingsService
    {
        private const string Endpoint = "api/Trainings";

        public async Task<List<TrainingDto>> GetAllTrainings(int userId)
        {
            try
            {
                using var client = httpClientFactory.CreateClient(HttpClientConfig.TrainingsClient);
                var response = await client.GetAsync($"{Endpoint}?id={userId}");
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
