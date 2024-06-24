using Microsoft.AspNetCore.Http;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateTests.Infastructure;

namespace StargateTests.UnitTests.Commands
{

    [TestClass]
    public class CreatePersonTests : DataTestBase
    {
        private CreatePersonPreProcessor _preProcessor;
        private CreatePersonHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _handler = new CreatePersonHandler(_dbContext);
            _preProcessor = new CreatePersonPreProcessor(_dbContext);
        }



        [TestMethod]
        public async Task PreProcessor_ShouldThrowException_WhenPersonAlreadyExists()
        {
            // Arrange: Insert test data for an existing person into the in-memory database directly
            var person = await InsertExistingPerson();

            var request = new CreatePerson { Name = person.Name };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BadHttpRequestException>(() => _preProcessor.Process(request, CancellationToken.None));
        }

        [TestMethod]
        [DataRow("John Doe")]
        [DataRow("Jane Doe")]
        [DataRow("Lord Taquito Taco The Consumed")]
        [DataRow("Jaq")]
        public async Task Handler_ShouldCreateNewPerson_WhenPersonDoesNotExist(string name)
        {

            var request = new CreatePerson { Name = name };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Id);

            // Verify data in the database
            var person = _dbContext.People.FirstOrDefault(p => p.Name == request.Name);
            Assert.IsNotNull(person);
        }

    }
}
