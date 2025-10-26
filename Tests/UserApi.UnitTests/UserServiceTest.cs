using DataAccess.Models;
using Moq;
using UsersApi.Abstractions;
using UsersApi.BLL.DTOs;
using UsersApi.BLL.Services;
using UsersApi.Repositories;

namespace Tests.UserApi.UnitTests
{
    [TestClass]
    public class UserServiceTest
    {
        public TestContext TestContext { get; set; }

        private Mock<IUserRepository> _mockRepository;
        private Mock<IDbListener> _listner;
        private Mock<IAchievementsService> _achievementsService;
        private Mock<INutritionsService> _nutritionsService;
        private Mock<ITrainingsService> _trainingsService;
        private UserService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IUserRepository>();
            _listner = new Mock<IDbListener>();
            _achievementsService = new Mock<IAchievementsService>();
            _trainingsService = new Mock<ITrainingsService>();
            _nutritionsService = new Mock<INutritionsService>();
            _service = new UserService(_mockRepository.Object,
                _listner.Object, _achievementsService.Object,
                _nutritionsService.Object, _trainingsService.Object);
        }

        [TestMethod]
        public async Task GetByIdAsync_ValidId_ReturnsMappedDto()
        {
            var userEntity = new User { Id = 1, Name = "Анна", Surname = "Иванова", Email = "anna@example.com" };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);

            var result = await _service.GetByIdAsync(1, TestContext.CancellationToken);

            Assert.IsNotNull(result);
            Assert.AreEqual("Анна", result.Name);
            Assert.AreEqual("Иванова", result.Surname);
        }

        [TestMethod]
        public async Task DeleteAsync_IdZero_ReturnsFalse()
        {
            var result = await _service.DeleteAsync(0, TestContext.CancellationToken);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetAllAsync_NoUsersInRepo_ReturnsEmptyList()
        {
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new List<User>());

            var result = await _service.GetAllAsync(TestContext.CancellationToken);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((User)null);

            var result = await _service.GetByIdAsync(999, TestContext.CancellationToken);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse_FromRepository()
        {
            _mockRepository.Setup(r => r.DeleteAsync(999, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(false);

            var result = await _service.DeleteAsync(999, TestContext.CancellationToken);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_DeletedUser_ReturnsNull()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((User)null);

            var result = await _service.GetByIdAsync(1, TestContext.CancellationToken);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateAsync_UserNotFound_ReturnsFalse()
        {
            var request = new UserRequest { Id = 999 };
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((User)null);

            var result = await _service.UpdateAsync(request, TestContext.CancellationToken);

            Assert.IsFalse(result);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CreateAsync_RepositoryThrows_ExceptionIsPropagated()
        {
            var request = new UserRequest { Name = "Ошибка" };
            _mockRepository.Setup(r => r.CreatedAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                          .ThrowsAsync(new Exception("DB failed"));

            await _service.CreateAsync(request, TestContext.CancellationToken);
        }
        [TestMethod]
        public async Task CreateAsync_WithExistingIdInDb_ThrowsException()
        {
            var request = new UserRequest { Id = 1, Name = "Новый", Surname = "Пользователь", Email = "new@example.com" };

            var existingUser = new User { Id = 1, Name = "Старый", Surname = "Пользователь", Email = "old@example.com" };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingUser);

            _mockRepository.Setup(r => r.CreatedAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            {
                await _service.CreateAsync(request, TestContext.CancellationToken);
            });

            _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);

            _mockRepository.Verify(r => r.CreatedAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never());
        }
        [TestMethod]
        public async Task DeleteAsync_NegativeId_ReturnsFalse()
        {
            var result = await _service.DeleteAsync(-5, TestContext.CancellationToken);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UpdateAsync_NegativeIdInRequest_ReturnsFalse()
        {
            var request = new UserRequest { Id = -1 };
            _mockRepository.Setup(r => r.GetByIdAsync(-1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((User)null);

            var result = await _service.UpdateAsync(request, TestContext.CancellationToken);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateAsync_NameWithSpaces_CallsRepositoryWithTrimmedName()
        {
            var request = new UserRequest { Name = "  Вася  ", Surname = " Петров ", Email = "v@example.com" };

            User capturedUser = null;
            _mockRepository.Setup(r => r.CreatedAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                          .Callback<User, CancellationToken>((u, ct) => capturedUser = u)
                          .Returns(Task.CompletedTask);

            await _service.CreateAsync(request, TestContext.CancellationToken);

            Assert.AreEqual("Вася", capturedUser.Name);
            Assert.AreEqual("Петров", capturedUser.Surname);
            Assert.AreEqual("v@example.com", capturedUser.Email);
        }

        [TestMethod]
        public async Task UpdateAsync_NameWithSpaces_CallsRepositoryWithTrimmedName()
        {
            var existingUser = new User { Id = 1, Name = "Старое", Surname = "Старая", Email = "old@example.com" };
            var request = new UserRequest { Id = 1, Name = "  Новое ", Surname = " Фамилия ", Email = " new@example.com " };

            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingUser);

            User capturedUser = null;
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                          .Callback<User, CancellationToken>((u, ct) => capturedUser = u)
                          .Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(request, TestContext.CancellationToken);

            Assert.IsTrue(result);
            Assert.AreEqual("Новое", capturedUser.Name);
            Assert.AreEqual("Фамилия", capturedUser.Surname);
            Assert.AreEqual("new@example.com", capturedUser.Email);
        }

        [TestMethod]
        public async Task CreateAsync_ServicesHasData_ServicesReturnValues()
        {
            var userEntity = new User { Id = 1, Name = "Анна", Surname = "Иванова", Email = "anna@example.com" };
            var achievements = GetAchievements();
            var trainings = GetTrainings();
            var nutritions = GetNutritions();

            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);
            _achievementsService.Setup(x => x.GetAllAchievements(1))
                .ReturnsAsync(achievements);
            _trainingsService.Setup(x => x.GetAllTrainings(1))
                .ReturnsAsync(trainings);
            _nutritionsService.Setup(x => x.GetAllNutritions(1))
                .ReturnsAsync(nutritions);

            var result = await _service.GetByIdAsync(1, TestContext.CancellationToken);

            _achievementsService.Verify(r => r.GetAllAchievements(1), Times.Once);
            _nutritionsService.Verify(r => r.GetAllNutritions(1), Times.Once);
            _trainingsService.Verify(r => r.GetAllTrainings(1), Times.Once);

            Assert.HasCount(1, result.Achievements!);
            Assert.HasCount(1, result.Nutritions!);
            Assert.HasCount(1, result.Trainings!);
        }

        [TestMethod]
        public async Task CreateAsync_ServicesHasNull_ServicesHasEmptyCollection()
        {
            var userEntity = new User { Id = 1, Name = "Анна", Surname = "Иванова", Email = "anna@example.com" };

            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);
            _achievementsService.Setup(x => x.GetAllAchievements(1))
                .ReturnsAsync([]);
            _trainingsService.Setup(x => x.GetAllTrainings(1))
                .ReturnsAsync([]);
            _nutritionsService.Setup(x => x.GetAllNutritions(1))
                .ReturnsAsync([]);

            var result = await _service.GetByIdAsync(1, TestContext.CancellationToken);

            _achievementsService.Verify(r => r.GetAllAchievements(1), Times.Once);
            _nutritionsService.Verify(r => r.GetAllNutritions(1), Times.Once);
            _trainingsService.Verify(r => r.GetAllTrainings(1), Times.Once);

            Assert.IsEmpty(result.Achievements!);
            Assert.IsEmpty(result.Nutritions!);
            Assert.IsEmpty(result.Trainings!);
        }

        private static List<NutritionDto> GetNutritions()
        {
            return
            [
                new() {
                    Calories = 1,
                    Description = "asdfas",
                    NutritionId = 1,
                }
            ];
        }

        private static List<TrainingDto> GetTrainings()
        {
            return
            [
                new() {
                    Date = DateTime.Now,
                    Description = "123",
                    DurationInMinutes = 3,
                    Id = 4,
                    IsCompleted = true,
                }
            ];
        }

        private static List<AchievementDto> GetAchievements()
        {
            return
            [
                new() {
                    AchievedDate = DateTime.Now,
                    Id = 2,
                    Reward = 3,
                    Type = DataAccess.Enums.AchievementType.StrengthTraining,
                    Value = 4
                }
            ];
        }
    }
}
