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

        public AchievementsRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            appContext = new DataAccess.AppContext(dbOptions.Options);
        }

        [TestMethod]
        public void AddAsync_CreateAchievement_ShouldAddToRepository()
        {
            var repository = new AchievementRepository(appContext);
            var achievement = new Achievement()
            {
                AchievedDate = DateTime.UtcNow,
                Reward = 15.5m,
                Type = DataAccess.Enums.AchievementType.StrengthTraining,
                Value = 35,
                UserId = 2
            };

            var result = repository.AddAsync(achievement, TestContext.CancellationToken).Result;
            var achievements = appContext.Achievements.ToList();

            Assert.IsTrue(result);
            Assert.ContainsSingle(achievements);
        }
    }
}
