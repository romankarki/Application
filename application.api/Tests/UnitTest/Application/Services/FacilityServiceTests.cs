using Application.Implementation;
using Application.Interfaces.Infrastructure;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;
using System.Text;

namespace Tests.UnitTest.Application.Services
{
    public class FacilityServiceTests
    {
        private readonly FacilityService _service;
        private readonly Mock<IRepository<Facility>> _repoMock;
        private readonly Mock<IFacilityRepository> _facilityRepoMock;

        public FacilityServiceTests()
        {
            _facilityRepoMock = new Mock<IFacilityRepository>();
            _repoMock = new Mock<IRepository<Facility>>();
            _service = new FacilityService(_facilityRepoMock.Object, _repoMock.Object);
        }

        [Fact]
        public async Task BulkUploadFacilityAsync_ShouldProcessFileCorrectly()
        {
            var officerId = 1;
            var fileContent = "Address,Code,Name,Email,Number,PersonName,Delete\n123abcstreeet,Some-123,ABC,abc@test.com,1234567890,person123,no";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            stream.Position = 0;
            var formFile = new Mock<IFormFile>();
            formFile.Setup(_ => _.OpenReadStream()).Returns(stream);
            formFile.Setup(_ => _.FileName).Returns("test.csv");
            formFile.Setup(_ => _.Length).Returns(stream.Length);

            _repoMock.Setup(repo => repo.FindFirstAsync(It.IsAny<Expression<Func<Facility, bool>>>()))
           .ReturnsAsync((Facility)null);
            _repoMock.Setup(repo => repo.AddAsync(It.IsAny<Facility>())).ReturnsAsync(new Facility());

            var result = await _service.BulkUploadFacilityAsync(formFile.Object, officerId);

            Assert.Equal(1, result.TotalInserted);
            Assert.Equal(0, result.TotalUpdated);
            Assert.Equal(0, result.TotalDeleted);
            Assert.Empty(result.RejectedRecords);
        }

        [Fact]
        public async Task BulkUploadFacilityAsync_ShouldThrowHeaderValidationException_ForWrongFileFormat()
        {
            var officerId = 1;
            var fileContent = "Address,Code,Name,Email,Delete\n123abcstreeet,Some-123,ABC,1234567890,person123,no";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            stream.Position = 0;
            var formFile = new Mock<IFormFile>();
            formFile.Setup(_ => _.OpenReadStream()).Returns(stream);
            formFile.Setup(_ => _.FileName).Returns("test.csv");
            formFile.Setup(_ => _.Length).Returns(stream.Length);

            _repoMock.Setup(repo => repo.FindFirstAsync(It.IsAny<Expression<Func<Facility, bool>>>()))
           .ReturnsAsync((Facility)null);
            _repoMock.Setup(repo => repo.AddAsync(It.IsAny<Facility>())).ReturnsAsync(new Facility());

            await Assert.ThrowsAsync<CsvHelper.HeaderValidationException>(async () => await _service.BulkUploadFacilityAsync(formFile.Object, officerId));
        }


        [Fact]
        public async Task BulkUploadFacilityAsync_ShouldThrowException_WhenRepoThrowsException()
        {
            var officerId = 1;
            var fileContent = "Address,Code,Name,Email,Number,PersonName,Delete\n123abcstreeet,Some-123,ABC,abc@test.com,1234567890,person123,no";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            stream.Position = 0;
            var formFile = new Mock<IFormFile>();
            formFile.Setup(_ => _.OpenReadStream()).Returns(stream);
            formFile.Setup(_ => _.FileName).Returns("test.csv");
            formFile.Setup(_ => _.Length).Returns(stream.Length);

            _repoMock.Setup(repo => repo.FindFirstAsync(It.IsAny<Expression<Func<Facility, bool>>>()))
           .ThrowsAsync(new Exception("Repo Exception"));
            _repoMock.Setup(repo => repo.AddAsync(It.IsAny<Facility>())).ReturnsAsync(new Facility());

            await Assert.ThrowsAsync<Exception>(async () => await _service.BulkUploadFacilityAsync(formFile.Object, officerId));
        }

        [Fact]
        public async Task GetAllFacilitiesCountAsync_ReturnsCorrectCount()
        {
            var facilities = new List<Facility> { new Facility(), new Facility(), new Facility() };

            _facilityRepoMock
                .Setup(repo => repo.GetFacilityByPageNumber(0))
                .ReturnsAsync(facilities);


            var count = await _service.GetALlFacilitiesCountAsync();

            Assert.Equal(3, count);
        }

        [Fact]
        public async Task GetAllFacilitiesCountAsync_ThrowsException_WhenRepoThrowsException()
        {
            _facilityRepoMock
                .Setup(repo => repo.GetFacilityByPageNumber(0))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _service.GetALlFacilitiesCountAsync());
        }

        [Fact]
        public async Task GetAllFacilitiesAsync_ReturnsCorrectModels()
        {
            var facilities = new List<Facility>
            {
                new Facility
                {
                    Id = 1,
                    FacilityName = "Facility A",
                    FacilityContactEmail = "contactA@example.com",
                    FacilityContactPerson = "Person A",
                    FacilityContactNumber = "1234567890",
                    FacilityCode = "FAC001",
                    Address = "Address A"
                },
                new Facility
                {
                    Id = 2,
                    FacilityName = "Facility B",
                    FacilityContactEmail = "contactB@example.com",
                    FacilityContactPerson = "Person B",
                    FacilityContactNumber = "0987654321",
                    FacilityCode = "FAC002",
                    Address = "Address B"
                }
            };

            _facilityRepoMock
                 .Setup(repo => repo.GetFacilityByPageNumber(It.IsAny<int>()))
                 .ReturnsAsync(facilities);


            var result = await _service.GetALlFacilitiesAsync(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            var firstFacility = result.First();
            Assert.Equal(1, firstFacility.Id);
            Assert.Equal("Facility A", firstFacility.FacilityName);
            Assert.Equal("contactA@example.com", firstFacility.ContactEmail);
            Assert.Equal("Person A", firstFacility.ContactPerson);
            Assert.Equal("1234567890", firstFacility.ContactNumber);
            Assert.Equal("FAC001", firstFacility.Code);
            Assert.Equal("Address A", firstFacility.Address);
        }

        [Fact]
        public async Task GetAllFacilitiesAsync_ThrowsException_WheRepoThrowsException()
        {
            _facilityRepoMock
                 .Setup(repo => repo.GetFacilityByPageNumber(It.IsAny<int>()))
                 .Throws(new Exception());


            await Assert.ThrowsAsync<Exception>(async () => await _service.GetALlFacilitiesAsync(1));
        }




    }
}
