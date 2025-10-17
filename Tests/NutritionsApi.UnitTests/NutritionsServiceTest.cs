using AutoMapper;
using DataAccess.Models;
using Moq;
using NutritionsApi.Abstractions;
using NutritionsApi.BLL.DTO.RequestDto;
using NutritionsApi.BLL.DTO.ResponseDto;
using NutritionsApi.BLL.Services;
using NutritionsApi.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.NutritionsApi.UnitTests
{
    [TestClass]
    public class NutritionsServiceTest
    {
        public TestContext TestContext { get; set; }

        private Mock<INutritionRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IDtoFactory> _mockDtoFactory;
        private NutritionService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<INutritionRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockDtoFactory = new Mock<IDtoFactory>();
            _service = new NutritionService(_mockRepository.Object, _mockMapper.Object, _mockDtoFactory.Object);
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingId_ReturnsMappedDto()
        {
            var nutrition = new Nutrition { Id = 1, UserId = 2, Description = "Meal", Calories = 100, IsDeleted = false };
            var expectedDto = new NutritionDetailsResponseDto { NutritionId = 1, UserId = 2, Description = "Meal", Calories = 100, IsDeleted = false };

            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(nutrition);
            _mockDtoFactory.Setup(f => f.GenerateDetailsResponse(1, 2, "Meal", 100, false))
                          .Returns(expectedDto);

            var result = await _service.GetByIdAsync(1, TestContext.CancellationToken);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto, result);
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Nutrition)null);

            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
                await _service.GetByIdAsync(999, TestContext.CancellationToken));
        }

        [TestMethod]
        public async Task GetByIdAsync_DeletedNutrition_ReturnsDtoWithIsDeletedTrue()
        {
            var deletedNutrition = new Nutrition { Id = 1, UserId = 2, Description = "Deleted", Calories = 100, IsDeleted = true };
            var expectedDto = new NutritionDetailsResponseDto { NutritionId = 1, UserId = 2, Description = "Deleted", Calories = 100, IsDeleted = true };

            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(deletedNutrition);
            _mockDtoFactory.Setup(f => f.GenerateDetailsResponse(1, 2, "Deleted", 100, true))
                          .Returns(expectedDto);

            var result = await _service.GetByIdAsync(1, TestContext.CancellationToken);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto, result);
        }


        [TestMethod]
        public async Task GetAllAsync_UserHasNutritions_ReturnsMappedDtoList()
        {
            var userId = 5;
            var nutritions = new List<Nutrition>
            {
                new() { Id = 1, UserId = userId, Description = "Meal 1", Calories = 100, IsDeleted = false },
                new() { Id = 2, UserId = userId, Description = "Meal 2", Calories = 150, IsDeleted = false }
            };
            var expectedDto1 = new NutritionDetailsResponseDto { NutritionId = 1, UserId = userId, Description = "Meal 1", Calories = 100, IsDeleted = false };
            var expectedDto2 = new NutritionDetailsResponseDto { NutritionId = 2, UserId = userId, Description = "Meal 2", Calories = 150, IsDeleted = false };

            _mockRepository.Setup(r => r.GetAllAsync(userId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(nutritions);
            _mockDtoFactory.Setup(f => f.GenerateDetailsResponse(1, userId, "Meal 1", 100, false))
                          .Returns(expectedDto1);
            _mockDtoFactory.Setup(f => f.GenerateDetailsResponse(2, userId, "Meal 2", 150, false))
                          .Returns(expectedDto2);

            var result = await _service.GetAllAsync(userId, TestContext.CancellationToken);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedDto1, result[0]);
            Assert.AreEqual(expectedDto2, result[1]);
        }

        [TestMethod]
        public async Task GetAllAsync_NoNutritionsInRepo_ReturnsEmptyList()
        {
            _mockRepository.Setup(r => r.GetAllAsync(999, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new List<Nutrition>());

            var result = await _service.GetAllAsync(999, TestContext.CancellationToken);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task CreateAsync_ValidRequest_CallsRepositoryAndReturnsDto()
        {
            var requestDto = new CreateNutritionRequestDto { UserId = 1, Description = "New Meal", Calories = 200 };
            var expectedDto = new NutritionDetailsResponseDto {
                NutritionId = 999,
                UserId = 1,
                Description = "New Meal",
                Calories = 200,
                IsDeleted = false };

            _mockMapper.Setup(m => m.Map<CreateNutritionRequestDto, Nutrition>(requestDto, It.IsAny<Nutrition>()))
                       .Callback<CreateNutritionRequestDto, Nutrition>((dto, dest) =>
                       {
                           dest.UserId = dto.UserId;
                           dest.Description = dto.Description;
                           dest.Calories = dto.Calories;
                       })
                       .Returns<CreateNutritionRequestDto, Nutrition>((dto, dest) => dest); 

            Nutrition capturedModelForRepo = null;
            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Nutrition>(), It.IsAny<CancellationToken>()))
                          .Callback<Nutrition, CancellationToken>((model, ct) =>
                          {
                              capturedModelForRepo = model;
                              model.Id = 999;
                          })
                          .ReturnsAsync(true);

            _mockDtoFactory.Setup(f => f.GenerateDetailsResponse(999, 1, "New Meal", 200, false))
                          .Returns(expectedDto);

            var result = await _service.CreateAsync(requestDto, TestContext.CancellationToken);

            Assert.AreEqual(expectedDto, result);
            _mockMapper.Verify(m => m.Map<CreateNutritionRequestDto, Nutrition>(requestDto, It.IsAny<Nutrition>()), Times.Once);
            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Nutrition>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockDtoFactory.Verify(f => f.GenerateDetailsResponse(999, 1, "New Meal", 200, false), Times.Once);

            Assert.IsNotNull(capturedModelForRepo);
            Assert.AreEqual(999, capturedModelForRepo.Id);
        }

        [TestMethod]
        public async Task CreateAsync_RepositoryReturnsFalse_ThrowsInvalidOperationException()
        {
            var requestDto = new CreateNutritionRequestDto { UserId = 1, Description = "New", Calories = 200 };
            var model = new Nutrition();

            _mockMapper.Setup(m => m.Map<CreateNutritionRequestDto, Nutrition>(requestDto, model))
                       .Returns(model);
            _mockRepository.Setup(r => r.CreateAsync(model, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(false);

            await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
                await _service.CreateAsync(requestDto, TestContext.CancellationToken));
        }

        [TestMethod]
        public async Task UpdateAsync_ExistingNutrition_CallsMapAndRepository()
        {
            var existingNutrition = new Nutrition {
                Id = 1,
                UserId = 1,
                Description = "Old",
                Calories = 100,
                IsDeleted = false,
                Created = DateTime.UtcNow.AddMinutes(-10),
                Updated = DateTime.UtcNow.AddMinutes(-5) };

            var requestDto = new UpdateNutritionRequestDto { NutritionId = 1, Description = "Updated", Calories = 200 };

            _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingNutrition);

            _mockMapper.Setup(m => m.Map<UpdateNutritionRequestDto, Nutrition>(requestDto, existingNutrition))
                       .Callback<UpdateNutritionRequestDto, Nutrition>((dto, dest) =>
                       {
                           dest.Description = dto.Description;
                           dest.Calories = dto.Calories;
                       })
                       .Returns(existingNutrition);

            _mockRepository.Setup(r => r.UpdateAsync(existingNutrition, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);

            var result = await _service.UpdateAsync(requestDto, TestContext.CancellationToken);

            Assert.IsTrue(result);
            _mockMapper.Verify(m => m.Map<UpdateNutritionRequestDto, Nutrition>(requestDto, existingNutrition), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(existingNutrition, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_NonExistingNutrition_ThrowsNotFoundException()
        {
            var requestDto = new UpdateNutritionRequestDto { NutritionId = 999, Description = "Updated", Calories = 200 };
            _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Nutrition)null);

            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
                await _service.UpdateAsync(requestDto, TestContext.CancellationToken));

            _mockMapper.Verify(m => m.Map<UpdateNutritionRequestDto, Nutrition>(requestDto, It.IsAny<Nutrition>()), Times.Never());
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Nutrition>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [TestMethod]
        public async Task UpdateAsync_NegativeIdInRequest_ThrowsNotFoundException()
        {
            var requestDto = new UpdateNutritionRequestDto { NutritionId = -1, Description = "Updated", Calories = 200 };
            _mockRepository.Setup(r => r.GetByIdAsync(-1, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Nutrition)null);

            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
                await _service.UpdateAsync(requestDto, TestContext.CancellationToken));

            _mockRepository.Verify(r => r.GetByIdAsync(-1, It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<UpdateNutritionRequestDto, Nutrition>(requestDto, It.IsAny<Nutrition>()), Times.Never());
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Nutrition>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [TestMethod]
        public async Task DeleteAsync_ExistingNutrition_CallsRepositoryAndReturnsTrue()
        {
            var nutritionId = 1;
            var existingNutrition = new Nutrition { Id = nutritionId, UserId = 1, Description = "To Delete", Calories = 100, IsDeleted = false };

            _mockRepository.Setup(r => r.GetByIdAsync(nutritionId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingNutrition);
            _mockRepository.Setup(r => r.DeleteAsync(nutritionId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);

            var result = await _service.DeleteAsync(nutritionId, TestContext.CancellationToken);

            Assert.IsTrue(result);
            _mockRepository.Verify(r => r.DeleteAsync(nutritionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_NonExistingId_ThrowsNotFoundException()
        {
            var nutritionId = 999;
            _mockRepository.Setup(r => r.GetByIdAsync(nutritionId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Nutrition)null);

            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
                await _service.DeleteAsync(nutritionId, TestContext.CancellationToken));

            _mockRepository.Verify(r => r.DeleteAsync(nutritionId, It.IsAny<CancellationToken>()), Times.Never());
        }

        [TestMethod]
        public async Task DeleteAsync_NegativeId_ThrowsNotFoundException()
        {
            var nutritionId = -5;
            _mockRepository.Setup(r => r.GetByIdAsync(nutritionId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Nutrition)null);

            await Assert.ThrowsExactlyAsync<NotFoundException>(async () =>
                await _service.DeleteAsync(nutritionId, TestContext.CancellationToken));

            _mockRepository.Verify(r => r.DeleteAsync(nutritionId, It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}

