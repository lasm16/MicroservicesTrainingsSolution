using UsersApi.Repositories;

namespace UsersApi.BLL.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(id, cancellationToken);
            return user.Name;
        }

        public async Task CreateAsync(string name, string email, CancellationToken cancellationToken = default)
        {
            var user = new UserDto
            {
                Name = name,
                Email = email
            };
            await userRepository.AddAsync(user);
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, string name, string email, CancellationToken cancellationToken = default)
        {
            var user = new UserDto
            {
                Id = id,
                Name = name,
                Email = email
            };
            throw new NotImplementedException();
        }
    }
}
