using Microsoft.AspNetCore.Http;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateTests.Infastructure;

namespace StargateTests.UnitTests.Commands
{
    [TestClass]

    public class CreateAstronautDutyTests : DataTestBase
    {
        protected CreateAstronautDutyHandler _handler;
        private CreateAstronautDutyPreProcessor _preProcessor;

        [TestInitialize]
        public void Setup()
        {
            _handler = new CreateAstronautDutyHandler(_dbContext);
            _preProcessor = new CreateAstronautDutyPreProcessor(_dbContext);
        }

        [TestMethod]
        public async Task PreProcessor_ShouldThrowException_WhenPersonNotFound()
        {
            (var person, var detail, var duty) = await InsertExistingPersonAndDuty();

            var request = new CreateAstronautDuty { Name = "Nonexistent Name", Rank = string.Empty, DutyTitle = string.Empty };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BadHttpRequestException>(() => _preProcessor.Process(request, CancellationToken.None));
        }


        [TestMethod]
        public async Task PreProcessor_ShouldThrowException_WhenDutyStartDateConflicts()
        {
            // Arrange: Insert conflicting duty data directly into the database
            (var person, var detail, var duty) = await InsertExistingPersonAndDuty();

            var request = new CreateAstronautDuty { Name = person.Name, DutyStartDate = duty.DutyStartDate, Rank = string.Empty, DutyTitle = string.Empty };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BadHttpRequestException>(() => _preProcessor.Process(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handler_ShouldCreateNewAstronautDetailAndDuty()
        {
            var person = await InsertExistingPerson();
            var request = new CreateAstronautDuty { Name = person.Name, DutyTitle = "Pilot", Rank = "Captain", DutyStartDate = DateTime.Now };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Id);

            // Verify data in the database
            var astronautDetail = _dbContext.AstronautDetails.FirstOrDefault(z => z.Person.Name == person.Name);
            Assert.IsNotNull(astronautDetail);
            Assert.AreEqual("Pilot", astronautDetail.CurrentDutyTitle);
            Assert.AreEqual("Captain", astronautDetail.CurrentRank);
            Assert.AreEqual(request.DutyStartDate.Date, astronautDetail.CareerStartDate);

            var astronautDuty = _dbContext.AstronautDuties.FirstOrDefault(z => z.Person.Name == person.Name && z.DutyEndDate == null);
            Assert.IsNotNull(astronautDuty);
            Assert.AreEqual("Pilot", astronautDuty.DutyTitle);
            Assert.AreEqual("Captain", astronautDuty.Rank);
            Assert.AreEqual(request.DutyStartDate.Date, astronautDuty.DutyStartDate);

        }

        [TestMethod]
        public async Task Handler_ShouldUpdateExistingAstronautDetailAndDuty()
        {
            // Arrange: Insert existing person and astronaut detail data directly into the database
            (var person, var detail, var duty) = await InsertExistingPersonAndDuty();

            var request = new CreateAstronautDuty { Name = person.Name, DutyTitle = "Engineer", Rank = "Major", DutyStartDate = DateTime.Now };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Id);

            // Verify data in the database
            var astronautDetail = _dbContext.AstronautDetails.FirstOrDefault(z => z.Person.Name == person.Name);
            Assert.IsNotNull(astronautDetail);
            Assert.AreEqual("Engineer", astronautDetail.CurrentDutyTitle);
            Assert.AreEqual("Major", astronautDetail.CurrentRank);
            Assert.AreNotEqual(request.DutyStartDate.Date, astronautDetail.CareerStartDate);

            var astronautDuty = _dbContext.AstronautDuties.FirstOrDefault(z => z.Person.Name == person.Name && z.DutyEndDate == null);
            Assert.IsNotNull(astronautDuty);
            Assert.AreEqual("Engineer", astronautDuty.DutyTitle);
            Assert.AreEqual("Major", astronautDuty.Rank);
            Assert.AreEqual(request.DutyStartDate.Date, astronautDuty.DutyStartDate);
        }

        [TestMethod]
        public async Task Handler_ShouldEndPriorDutyAstronautDetail()
        {
            // Arrange: Insert existing person and astronaut detail data directly into the database
            (var person, var detail, var duty) = await InsertExistingPersonAndDuty();

            var request = new CreateAstronautDuty { Name = person.Name, DutyTitle = "Engineer", Rank = "Major", DutyStartDate = DateTime.Now };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Id);

            // Verify data in the database
            var previousDuty = _dbContext.AstronautDuties.FirstOrDefault(z => z.Id == duty.Id);
            Assert.IsNotNull(previousDuty);
            Assert.AreEqual(request.DutyStartDate.Date.AddDays(-1), previousDuty.DutyEndDate);
        }




        [TestMethod]
        public async Task Handler_ShouldRetire()
        {
            // Arrange: Insert existing person and astronaut detail data directly into the database
            (var person, var detail, var duty) = await InsertExistingPersonAndDuty();

            var request = new CreateAstronautDuty { Name = person.Name, DutyTitle = "RETIRED", Rank = duty.Rank, DutyStartDate = DateTime.Now };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Id);

            // Verify data in the database
            var astronautDetail = _dbContext.AstronautDetails.FirstOrDefault(z => z.Person.Name == person.Name);
            Assert.IsNotNull(astronautDetail);
            Assert.AreEqual("RETIRED", astronautDetail.CurrentDutyTitle);
            Assert.AreEqual(request.DutyStartDate.Date.AddDays(-1), astronautDetail.CareerEndDate);
        }


        [TestMethod]
        public async Task Handler_ShouldComeOutOfRetirement()
        {
            // Arrange: Insert existing person and astronaut detail data directly into the database
            (var person, var detail, var duty) = await InsertExistingPersonAndDuty();
            (var retiredDetail, var retiredDuty) = await InsertRetired(person, DateTime.Parse("2010-01-01"));

            var request = new CreateAstronautDuty { Name = person.Name, DutyTitle = "Not Retired", Rank = "Legend", DutyStartDate = DateTime.Now };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Id);

            // Verify data in the database
            var astronautDetail = _dbContext.AstronautDetails.FirstOrDefault(z => z.Person.Name == person.Name);
            Assert.IsNotNull(astronautDetail);
            Assert.AreEqual("Not Retired", astronautDetail.CurrentDutyTitle);
            Assert.IsNull(astronautDetail.CareerEndDate);
        }

    }
}
