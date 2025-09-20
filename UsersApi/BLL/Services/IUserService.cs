namespace UsersApi.BLL.Services
{
    public interface IUserService
    {
        Task<string> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<string>> GetAllAsync(CancellationToken cancellationToken = default);
        internal Task CreateAsync(string name, string email, CancellationToken cancellationToken = default);
        internal Task UpdateAsync(int id, string name, string email, CancellationToken cancellationToken = default);
        internal Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
