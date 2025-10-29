using AchievementsApi.Repositores;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using NutritionsApi.Repositories;


namespace Tests.NutritionsApi.UnitTests
{
    [TestClass]
    public class NutritionsRepositoryTest
    {
        public TestContext TestContext { get; set; }

        private readonly DataAccess.AppContext _appContext;
        private readonly NutritionRepository _repository;

        public NutritionsRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            _appContext = new DataAccess.AppContext(dbOptions.Options);
            _repository = new NutritionRepository(_appContext);
        }
        [TestInitialize]
        public void Setup()
        {
            _appContext.Database.EnsureDeleted();
            _appContext.Database.EnsureCreated();            
        }
        #region GetByIdAsync

        [TestMethod]
        public async Task GetByIdAsync_ExistingId_ReturnsNutrition()
        {
            var nutrition = new Nutrition { Id = 1, UserId = 2, Description = "Test Meal", Calories = 200, IsDeleted = false };
            _appContext.Nutritions.Add(nutrition);
            await _appContext.SaveChangesAsync(TestContext.CancellationToken);

            var result = await _repository.GetByIdAsync(1, TestContext.CancellationToken);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(2, result.UserId);
            Assert.AreEqual("Test Meal", result.Description);
            Assert.AreEqual(200, result.Calories);
            Assert.IsFalse(result.IsDeleted);
        }

        [TestMethod]
        public async Task GetByIdAsync_IdZero_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(0, TestContext.CancellationToken);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_EmptyDb_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(1, TestContext.CancellationToken);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(999, TestContext.CancellationToken);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_DeletedNutrition_ReturnsNull()
        {
            var nutrition = new Nutrition { Id = 1, UserId = 2, Description = "Deleted", Calories = 100, IsDeleted = true };
            _appContext.Nutritions.Add(nutrition);
            await _appContext.SaveChangesAsync(TestContext.CancellationToken);

            var directFetch = await _appContext.Nutritions.FindAsync(1);
            Assert.IsNotNull(directFetch);
            Assert.IsTrue(directFetch.IsDeleted);

            var result = await _repository.GetByIdAsync(1, TestContext.CancellationToken);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(-5, TestContext.CancellationToken);
            Assert.IsNull(result);
        }

        #endregion

        #region GetAllAsync

        [TestMethod]
        public async Task GetAllAsync_EmptyDb_ReturnsEmptyList()
        {
            var result = await _repository.GetAllAsync(1, TestContext.CancellationToken);
            Assert.IsEmpty(result);
        }

        [TestMethod]
        public async Task GetAllAsync_ExcludesDeletedNutritions()
        {
            _appContext.Nutritions.AddRange(
                new Nutrition { Id = 1, UserId = 1, Description = "Active", Calories = 100, IsDeleted = false },
                new Nutrition { Id = 2, UserId = 1, Description = "Deleted", Calories = 200, IsDeleted = true }
            );
            await _appContext.SaveChangesAsync(TestContext.CancellationToken);

            var result = await _repository.GetAllAsync(1, TestContext.CancellationToken);

            Assert.HasCount(1, result);
            Assert.AreEqual("Active", result[0].Description);
        }

        #endregion

        #region DeleteAsync

        [TestMethod]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            var result = await _repository.DeleteAsync(999, TestContext.CancellationToken);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteAsync_ExistingActiveNutrition_SetsIsDeletedTrue()
        {
            var nutrition = new Nutrition { Id = 1, UserId = 2, Description = "To Delete", Calories = 100, IsDeleted = false };
            _appContext.Nutritions.Add(nutrition);
            await _appContext.SaveChangesAsync(TestContext.CancellationToken);

            var result = await _repository.DeleteAsync(1, TestContext.CancellationToken);

            Assert.IsTrue(result);
            var dbNutrition = await _appContext.Nutritions.FindAsync(1);
            Assert.IsTrue(dbNutrition.IsDeleted);
            Assert.IsTrue(dbNutrition.Updated > DateTime.UtcNow.AddMinutes(-1));
        }

        [TestMethod]
        public async Task DeleteAsync_AlreadyDeletedNutrition_ReturnsFalse()
        {
            var nutrition = new Nutrition { Id = 1, UserId = 2, Description = "Already Deleted", Calories = 100, IsDeleted = true };
            _appContext.Nutritions.Add(nutrition);
            await _appContext.SaveChangesAsync(TestContext.CancellationToken);

            var result = await _repository.DeleteAsync(1, TestContext.CancellationToken);

            Assert.IsFalse(result);
        }

        #endregion

        #region CreateAsync

        [TestMethod]
        public async Task CreateAsync_SetsCreatedToUtcNow()
        {
            var beforeCall = DateTime.UtcNow;
            var nutrition = new Nutrition { UserId = 1, Description = "New Meal", Calories = 150, IsDeleted = false };
            var afterCall = DateTime.UtcNow;

            Assert.IsTrue(nutrition.Created >= beforeCall && nutrition.Created <= afterCall.AddSeconds(1));

            await _repository.CreateAsync(nutrition, TestContext.CancellationToken);
            Assert.IsTrue(nutrition.Created <= DateTime.UtcNow.AddSeconds(1));
        }

        #endregion

        #region UpdateAsync

        [TestMethod]
        public async Task UpdateAsync_SetsUpdatedToUtcNow()
        {
            var nutrition = new Nutrition { Id = 1, UserId = 1, Description = "Old", Calories = 100, IsDeleted = false };
            _appContext.Nutritions.Add(nutrition);
            await _appContext.SaveChangesAsync(TestContext.CancellationToken);

            var originalCreated = nutrition.Created;
            var originalUpdated = nutrition.Updated;
            nutrition.Description = "Updated";
            nutrition.Calories = 300;

            await _repository.UpdateAsync(nutrition, TestContext.CancellationToken);

            Assert.AreEqual(originalCreated, nutrition.Created);
            Assert.AreEqual(originalUpdated, nutrition.Updated);
            Assert.AreEqual("Updated", nutrition.Description);
            Assert.AreEqual(300, nutrition.Calories);
        }

        #endregion
    }
}
