using UsersApi.BLL.DTOs;

namespace UsersApi.Abstractions
{
    public interface ITrainingsService
    {
        Task<List<TrainingDto>> GetAllTrainings(int userId);
    }
}
