using AchievementsApi.BLL.Abstractions;
using AchievementsApi.BLL.DTO.Requests;
using AchievementsApi.BLL.Services;
using DataAccess.Models;
using Moq;

namespace Tests.AchievementsApi.UnitTests;

[TestClass]
public class AchievementsServiceTest
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public void CreateAsync_AnyAchievementCreateRequest_ShouldReturnTrue()
    {
        var repository = new Mock<IAchievementRepository>();
        repository.Setup(x => x.AddAsync(It.IsAny<Achievement>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        var service = new AchievementService(repository.Object);
        var request = new AchievementCreateRequest
        {
            AchievedDate = DateTime.UtcNow,
            Type = DataAccess.Enums.AchievementType.StrengthTraining,
            UserId = 1,
            Value = 15
        };

        var result = service.CreateAsync(request, TestContext.CancellationToken).Result;

        Assert.IsTrue(result, message: $"ќжидалс€ результат True, но пришел: {result}");
    }
}
