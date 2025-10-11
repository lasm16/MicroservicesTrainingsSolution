using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using UsersApi.Repositories;
using Moq;
namespace Tests.UserApi.UnitTests
{
    [TestClass]
    public class UserRepositoryTest
    {
        public TestContext TestContext { get; set; }

        private readonly DataAccess.AppContext appContext;
        private UserRepository repository;

        public UserRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            appContext = new DataAccess.AppContext(dbOptions);
        }

        [TestInitialize]
        public void Setup()
        {
            appContext.Database.EnsureDeleted();
            appContext.Database.EnsureCreated();

            repository = new UserRepository(appContext);
        }

        [TestMethod]
        public async Task GetByIdAsync_ValidId_ReturnsUser()
        {
            var user = new User { Id = 1, Name = "Анна", Surname = "Иванова", Email = "anna@example.com", IsDeleted = false };
            appContext.Users.Add(user);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            var result = await repository.GetByIdAsync(1, TestContext.CancellationToken);

            Assert.IsNotNull(result);
            Assert.AreEqual("Анна", result.Name);
        }

        [TestMethod]
        public async Task CreatedAsync_NameExactly140Chars_AllowsIt()
        {
            var longName = new string('A', 140);
            var user = new User { Name = longName, Email = "test@example.com" };

            await repository.CreatedAsync(user, TestContext.CancellationToken);

            var saved = await appContext.Users.FindAsync(user.Id);
            Assert.AreEqual(longName, saved.Name);
        }       

        [TestMethod]
        public async Task CreatedAsync_NameIsNull_AllowsIt()
        {
            var user = new User { Name = null, Email = "test@example.com" };

            await repository.CreatedAsync(user, TestContext.CancellationToken);

            var saved = await appContext.Users.FindAsync(user.Id);
            Assert.IsNull(saved.Name);
        }

        [TestMethod]
        public async Task GetByIdAsync_IdZero_ReturnsNull()
        {
            var result = await repository.GetByIdAsync(0, TestContext.CancellationToken);
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public async Task GetAllAsync_EmptyDb_ReturnsEmptyList()
        {
            var result = await repository.GetAllAsync(TestContext.CancellationToken);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_EmptyDb_ReturnsNull()
        {
            var result = await repository.GetByIdAsync(1, TestContext.CancellationToken);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            var result = await repository.DeleteAsync(999, TestContext.CancellationToken);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var result = await repository.GetByIdAsync(999, TestContext.CancellationToken);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ExistingUser_SetsIsDeletedTrue()
        {
            var user = new User { Id = 1, Name = "Будет удалён", IsDeleted = false };
            appContext.Users.Add(user);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            await repository.DeleteAsync(1, TestContext.CancellationToken);

            var dbUser = await appContext.Users.FindAsync(1);
            Assert.IsTrue(dbUser.IsDeleted);
        }

        [TestMethod]
        public async Task GetByIdAsync_DeletedUser_ReturnsNull()
        {
            var user = new User { Id = 1, Name = "Удалён", IsDeleted = true };
            appContext.Users.Add(user);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            var result = await repository.GetByIdAsync(1, TestContext.CancellationToken);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllAsync_ExcludesDeletedUsers()
        {
            appContext.Users.AddRange(
                new User { Name = "Активный", IsDeleted = false },
                new User { Name = "Удалённый", IsDeleted = true }
            );
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            var result = await repository.GetAllAsync(TestContext.CancellationToken);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Активный", result[0].Name);
        }
        
        [TestMethod]
        public async Task GetAllAsync_ConditionIsDeletedFalse_AppliesFilter()
        {
            var user = new User { Name = "Тест", IsDeleted = true };
            appContext.Users.Add(user);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            var result = await repository.GetAllAsync(TestContext.CancellationToken);

            Assert.IsFalse(result.Any(u => u.Id == user.Id));
        }
        [TestMethod]
        public async Task CreatedAsync_SetsCreatedToUtcNow()
        {
            var now = DateTime.UtcNow;
            var user = new User { Name = "Дата" };
            await repository.CreatedAsync(user, TestContext.CancellationToken);

            Assert.IsTrue((user.Created - now).TotalSeconds < 10);
        }

        [TestMethod]
        public async Task UpdateAsync_SetsUpdatedToUtcNow()
        {
            var user = new User { Id = 1, Name = "Старое", IsDeleted = false };
            appContext.Users.Add(user);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            var beforeUpdate = DateTime.UtcNow;
            user.Name = "Новое";
            await repository.UpdateAsync(user, TestContext.CancellationToken);

            Assert.IsTrue((user.Updated - beforeUpdate).TotalSeconds >= 0);
        }

        [TestMethod]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            var result = await repository.GetByIdAsync(-5, TestContext.CancellationToken);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreatedAsync_EmptyName_AllowsIt()
        {
            var user = new User { Name = "", Email = "test@example.com" };
            await repository.CreatedAsync(user, TestContext.CancellationToken);

            var saved = await appContext.Users.FindAsync(user.Id);
            Assert.AreEqual("", saved.Name);
        }

        [TestMethod]
        public async Task CreatedAsync_NameWithSpaces_PreservesSpaces()
        {
            var user = new User { Name = "  Вася  ", Email = "v@example.com" };
            await repository.CreatedAsync(user, TestContext.CancellationToken);

            var saved = await appContext.Users.FindAsync(user.Id);
            Assert.AreEqual("  Вася  ", saved.Name);
        }

        [TestMethod]
        public async Task DeleteAsync_Twice_ReturnsFalseOnSecondCall()
        {
            var user = new User { Id = 1, Name = "Для удаления", IsDeleted = false };
            appContext.Users.Add(user);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            var first = await repository.DeleteAsync(1, TestContext.CancellationToken);
            var second = await repository.DeleteAsync(1, TestContext.CancellationToken);

            Assert.IsTrue(first);
            Assert.IsFalse(second);
        }
    }
}
