using AchievementsApi.Repositores;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Tests.AchievementsApi.UnitTests
{
    [TestClass]
    public class AchievementsRepositoryTest
    {
        public TestContext TestContext { get; set; }

        private readonly DataAccess.AppContext appContext;
        private readonly AchievementRepository _repository;

        public AchievementsRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            appContext = new DataAccess.AppContext(dbOptions.Options);
            _repository = new AchievementRepository(appContext);
        }

        [TestMethod]
        public async Task AddAsync_AchievementExistsInDb_ShouldSetAchievementIdTo2()
        {
            var achievement = GetAchievement();
            await appContext.Achievements.AddAsync(achievement, TestContext.CancellationToken);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);
            achievement.Id = 2;

            var result = _repository.AddAsync(achievement, TestContext.CancellationToken).Result;
            var achievements = appContext.Achievements.ToList();

            Assert.IsTrue(result);
            Assert.IsNotNull(achievements.LastOrDefault());
            Assert.AreEqual(2, achievements.LastOrDefault()!.Id);
        }

        [TestMethod]
        public async Task DeleteAsync_AchievementIsDeletedFalse_ShouldSetAchievementIsDeletedToTrue()
        {
            var achievement = GetAchievement();
            await appContext.Achievements.AddAsync(achievement, TestContext.CancellationToken);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            var result = _repository.DeleteAsync(achievement, TestContext.CancellationToken).Result;
            var achievements = appContext.Achievements.ToList();

            Assert.IsTrue(result);
            Assert.ContainsSingle(achievements);
            Assert.IsTrue(achievements.FirstOrDefault()!.IsDeleted);
        }

        [TestMethod]
        public async Task UpdateAsync_AchievementExists_ShouldUpdateFields()
        {
            var achievement = GetAchievement();
            await appContext.Achievements.AddAsync(achievement, TestContext.CancellationToken);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);
            achievement.Reward = 30m;
            achievement.Type = DataAccess.Enums.AchievementType.Running;
            achievement.Value = 15;
            achievement.UpdatedAt = DateTime.Parse("Jan 1, 2009");
            var beforeUpdate = DateTime.UtcNow;

            var result = _repository.UpdateAsync(achievement, TestContext.CancellationToken).Result;
            var achievements = appContext.Achievements.ToList();
            var expected = achievements.FirstOrDefault();

            Assert.IsTrue(result);
            Assert.ContainsSingle(achievements);
            Assert.AreEqual(2, expected!.UserId);
            Assert.AreEqual(1, expected!.Id);
            Assert.AreEqual(30m, expected!.Reward);
            Assert.AreEqual(DataAccess.Enums.AchievementType.Running, expected!.Type);
            Assert.AreEqual(15, expected!.Value);
            Assert.IsGreaterThanOrEqualTo(beforeUpdate, expected.UpdatedAt);
        }

        [TestMethod]
        public async Task GetByIdAsync_AchievementExists_ShouldReturnEntity()
        {
            var achievement = GetAchievement();
            await appContext.Achievements.AddAsync(achievement, TestContext.CancellationToken);
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            var expected = _repository.GetByIdAsync(1, TestContext.CancellationToken).Result;

            Assert.IsNotNull(expected);
            Assert.AreEqual(2, expected!.UserId);
            Assert.AreEqual(1, expected!.Id);
            Assert.AreEqual(15.5m, expected!.Reward);
            Assert.AreEqual(DataAccess.Enums.AchievementType.StrengthTraining, expected!.Type);
            Assert.AreEqual(35, expected!.Value);
        }

        [TestMethod]
        public async Task GetAllAsync_AchievementExists_ShouldReturn2Entities()
        {
            await appContext.Achievements.AddRangeAsync(GetAchievement(), GetAchievement());
            await appContext.SaveChangesAsync(TestContext.CancellationToken);

            var expected = _repository.GetAllAsync(2, TestContext.CancellationToken).Result;

            Assert.HasCount(2, expected);
        }

        private static Achievement GetAchievement()
        {
            return new Achievement()
            {
                AchievedDate = DateTime.UtcNow,
                Reward = 15.5m,
                Type = DataAccess.Enums.AchievementType.StrengthTraining,
                Value = 35,
                UserId = 2
            };
        }
    }
}
