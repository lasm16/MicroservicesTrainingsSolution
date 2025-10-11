using DataAccess.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersApi.BLL.Models;
using UsersApi.BLL.Services;
using UsersApi.Repositories;

namespace Tests.UserApi.UnitTests
{
    [TestClass]
    public class UserServiceTest
    {
        public TestContext TestContext { get; set; }

        private Mock<IUserRepository> _mockRepository;
        private UserService _service;

        [TestInitialize]
        public void Setup()
        {            
            _mockRepository = new Mock<IUserRepository>();
            _service = new UserService(_mockRepository.Object);
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
        public async Task GetAllAsync_UserWithMaxLengthName_ReturnsFullName()
        {
            var longName = new string('A', 140);
            var users = new List<User> { new() { Name = longName, Surname = "Test" } };

            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(users);

            var result = await _service.GetAllAsync(TestContext.CancellationToken);

            Assert.AreEqual(longName, result[0].Name);
        }

        [TestMethod]
        public async Task GetByIdAsync_UserHasNullName_ReturnsDtoWithNullName()
        {
            var userEntity = new User { Id = 1, Name = null, Surname = "Фамилия" };
            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(userEntity);

            var result = await _service.GetByIdAsync(1, TestContext.CancellationToken);

            Assert.IsNull(result.Name);
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
        public async Task CreateAsync_LongName_PassesToRepository()
        {
            var longName = new string('A', 140);
            var request = new UserRequest { Name = longName, Email = "test@example.com" };

            User capturedUser = null;
            _mockRepository.Setup(r => r.CreatedAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                          .Callback<User, CancellationToken>((u, ct) => capturedUser = u)
                          .Returns(Task.CompletedTask);

            await _service.CreateAsync(request, TestContext.CancellationToken);

            Assert.AreEqual(longName, capturedUser.Name);
        }

        [TestMethod]
        public async Task CreateAsync_EmptyName_PassesToRepository()
        {
            var request = new UserRequest { Name = "", Email = "test@example.com" };

            User capturedUser = null;
            _mockRepository.Setup(r => r.CreatedAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                          .Callback<User, CancellationToken>((u, ct) => capturedUser = u)
                          .Returns(Task.CompletedTask);

            await _service.CreateAsync(request, TestContext.CancellationToken);

            Assert.AreEqual("", capturedUser.Name);
        }

        [TestMethod]
        public async Task CreateAsync_NameWithSpaces_PreservesSpaces()
        {
            var request = new UserRequest { Name = "  Вася  ", Email = "v@example.com" };

            User capturedUser = null;
            _mockRepository.Setup(r => r.CreatedAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                          .Callback<User, CancellationToken>((u, ct) => capturedUser = u)
                          .Returns(Task.CompletedTask);

            await _service.CreateAsync(request, TestContext.CancellationToken);

            Assert.AreEqual("  Вася  ", capturedUser.Name);
        }
    }
}
