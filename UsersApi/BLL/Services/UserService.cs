using UsersApi.Repositories;

namespace UsersApi.BLL.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private static (string? firstName, string? lastName) ParseName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return (null, null);
            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length switch
            {
                0 => (null, null),
                1 => (parts[0], null),
                _ => (parts[0], string.Join(" ", parts[1..]))
            };
        }

        public async Task<string> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(id, cancellationToken);
            return user == null ? string.Empty : $"{user.Name} {user.Surname}".Trim();
        }

        public async Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = await userRepository.GetAllAsync(cancellationToken);
            return users
                .Select(u => $"{u.Name} {u.Surname}".Trim())
                .ToList();
        }

        public async Task CreateAsync(string name, string email, CancellationToken cancellationToken = default)
        {
            var (firstName, lastName) = ParseName(name);

            var userDto = new UserDto
            {
                Name = firstName,
                Surname = lastName,
                Email = email
            };

            await userRepository.AddAsync(userDto, cancellationToken);
        }

        public async Task UpdateAsync(int id, string name, string email, CancellationToken cancellationToken = default)
        {
            var (firstName, lastName) = ParseName(name);

            var userDto = new UserDto
            {
                Id = id,
                Name = firstName,
                Surname = lastName,
                Email = email
            };

            await userRepository.UpdateAsync(userDto, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await userRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
