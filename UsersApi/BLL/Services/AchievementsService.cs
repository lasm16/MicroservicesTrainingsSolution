using UsersApi.Abstractions;
using UsersApi.BLL.DTOs;
using UsersApi.Properties;

namespace UsersApi.BLL.Services
{
    public class AchievementsService(IHttpClientFactory httpClientFactory) : IAchievementsService
    {
        private const string Endpoint = "api/Achievements/get-all";

        public async Task<List<AchievementDto>> GetAllAchievements(int userId)
        {
            try
            {
                using var client = httpClientFactory.CreateClient(HttpClientConfig.AchievementsClient);
                var response = await client.GetAsync($"{Endpoint}/{userId}");
                response.EnsureSuccessStatusCode();
                var list = await response.Content.ReadFromJsonAsync<List<AchievementDto>>();
                return list ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обращении к AchievementsService: {ex.InnerException}");
                return [];
            }
        }
    }
}
