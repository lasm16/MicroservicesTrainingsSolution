using TrainingsApi.BLL;
using TrainingsApi.DAL.Models;

namespace TrainingsApi.DAL.Repositories
{
    public class TrainingRepository(AppContext context) : ITrainingRepository
    {
        public Task<List<Training>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Training> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(TrainingDto training, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }        

        public Task UpdateAsync(TrainingDto training, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
