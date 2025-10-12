using AchievementsApi.Abstractions;
using AchievementsApi.BLL.DTO;
using AchievementsApi.BLL.DTO.Requests;
using AchievementsApi.BLL.Services;
using DataAccess.Models;
using Moq;

namespace Tests.AchievementsApi.UnitTests;

[TestClass]
public class AchievementsServiceTest
{
    public TestContext TestContext { get; set; }
    public readonly Mock<IAchievementRepository> _repository;
    public readonly AchievementService _service;

    public AchievementsServiceTest()
    {
        _repository = new Mock<IAchievementRepository>();
        _service = new AchievementService(_repository.Object);
    }

    #region Tests for CreateAsync method
    [TestMethod]
    public void CreateAsync_AnyAchievementCreateRequest_ShouldReturnTrue()
    {
        _repository.Setup(x => x.AddAsync(It.IsAny<Achievement>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var request = new AchievementCreateRequest
        {
            AchievedDate = DateTime.UtcNow,
            Type = DataAccess.Enums.AchievementType.StrengthTraining,
            UserId = 1,
            Value = 15
        };

        var result = _service.CreateAsync(request, TestContext.CancellationToken).Result;

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CreateAsync_AchievementCreateRequestWithIncorrectAchievementType_ShouldReturnTrue()
    {
        _repository.Setup(x => x.AddAsync(It.IsAny<Achievement>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        var request = new AchievementCreateRequest
        {
            AchievedDate = DateTime.UtcNow,
            Type = (DataAccess.Enums.AchievementType)2,
            UserId = 1,
            Value = 15
        };

        var result = _service.CreateAsync(request, TestContext.CancellationToken).Result;

        Assert.IsTrue(result);
    }
    #endregion

    #region Tests for UpdateAsync method
    [TestMethod]
    public void UpdateAsync_AnyAchievementUpdateRequest_ShouldReturnTrue()
    {
        var request = new AchievementUpdateRequest
        {
            Id = 1,
            AchievedDate = DateTime.UtcNow,
            Type = DataAccess.Enums.AchievementType.StrengthTraining,
            UserId = 1,
            Value = 15,
            IsDeleted = true
        };

        _repository.Setup(x => x.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Achievement());
        _repository.Setup(x => x.UpdateAsync(It.IsAny<Achievement>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(true));

        var result = _service.UpdateAsync(request, TestContext.CancellationToken).Result;

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void UpdateAsync_WhenNotFound_ShouldReturnFalse()
    {
        var request = new AchievementUpdateRequest
        {
            Id = 1,
            AchievedDate = DateTime.UtcNow,
            Type = DataAccess.Enums.AchievementType.StrengthTraining,
            UserId = 1,
            Value = 15,
            IsDeleted = true
        };

        _repository.Setup(x => x.UpdateAsync(It.IsAny<Achievement>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(false));

        var result = _service.UpdateAsync(request, TestContext.CancellationToken).Result;

        Assert.IsFalse(result);
    }
    #endregion

    #region Tests for DeleteAsync method
    [TestMethod]
    public void DeleteAsync_AnyAchievementUpdateRequest_ShouldReturnTrue()
    {
        var request = new AchievementUpdateRequest
        {
            Id = 1,
            AchievedDate = DateTime.UtcNow,
            Type = DataAccess.Enums.AchievementType.StrengthTraining,
            UserId = 1,
            Value = 15,
            IsDeleted = true
        };

        _repository.Setup(x => x.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Achievement());
        _repository.Setup(x => x.DeleteAsync(It.IsAny<Achievement>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(true));

        var result = _service.DeleteAsync(request.Id, TestContext.CancellationToken).Result;

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DeleteAsync_WhenNotFound_ShouldReturnFalse()
    {
        var request = new AchievementUpdateRequest
        {
            Id = 1,
            AchievedDate = DateTime.UtcNow,
            Type = DataAccess.Enums.AchievementType.StrengthTraining,
            UserId = 1,
            Value = 15,
            IsDeleted = true
        };

        _repository.Setup(x => x.DeleteAsync(It.IsAny<Achievement>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(false));

        var result = _service.DeleteAsync(request.Id, TestContext.CancellationToken).Result;

        Assert.IsFalse(result);
    }
    #endregion

    #region Tests for GetByIdAsync method
    [TestMethod]
    public void GetByIdAsync_AnyAchievementUpdateRequest_ShouldReturnTrue()
    {
        var expected = GetAchievementDto();
        var achievement = GetAchievement();
        _repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(achievement);

        var actual = _service.GetByIdAsync(1, TestContext.CancellationToken).Result;

        Assert.AreEqual(expected.Id, actual!.Id);
        Assert.AreEqual(expected.Type, actual.Type);
        Assert.AreEqual(expected.UserId, actual.UserId);
        Assert.AreEqual(expected.Value, actual.Value);
        Assert.AreEqual(expected.IsDeleted, actual.IsDeleted);
    }

    [TestMethod]
    public void GetByIdAsync_WhenNotFound_ShouldReturnFalse()
    {
        _repository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult((Achievement?)null));

        var result = _service.GetByIdAsync(1, TestContext.CancellationToken).Result;

        Assert.IsNull(result);
    }
    #endregion

    #region Tests for GetAllByUserIdAsync method
    [TestMethod]
    public void GetAllByUserIdAsync_HasAchievementList_ShouldReturnSameCount()
    {
        var expected = GetAchievementDto();
        var achievementList = new List<Achievement>()
        {
            GetAchievement(),
            GetAchievement()
        };

        _repository.Setup(x => x.GetAllAsync(1, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(achievementList);

        var result = _service.GetAllByUserIdAsync(1, TestContext.CancellationToken).Result;

        Assert.HasCount(2, result);
    }

    [TestMethod]
    public void GetAllByUserIdAsync_WhenNotFound_ShouldReturnZeroCount()
    {
        _repository.Setup(x => x.GetAllAsync(1, It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(new List<Achievement>()));

        var result = _service.GetAllByUserIdAsync(1, TestContext.CancellationToken).Result;

        Assert.HasCount(0, result);
    }
    #endregion

    private static Achievement GetAchievement()
    {
        return new Achievement()
        {
            Id = 1,
            AchievedDate = DateTime.UtcNow,
            Reward = 15.5m,
            Type = DataAccess.Enums.AchievementType.StrengthTraining,
            Value = 35,
            UserId = 2
        };
    }

    private static AchievementDto GetAchievementDto()
    {
        return new AchievementDto()
        {
            Id = 1,
            AchievedDate = DateTime.UtcNow,
            Reward = 15.5m,
            Type = DataAccess.Enums.AchievementType.StrengthTraining,
            Value = 35,
            UserId = 2
        };
    }
}
