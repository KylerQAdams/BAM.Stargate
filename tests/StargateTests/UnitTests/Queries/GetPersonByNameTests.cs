namespace StargateTests.UnitTests.Queries
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using StargateAPI.Business.Data;
    using StargateAPI.Business.Queries;
    using StargateTests.Infastructure;
    using System.Threading;
    using System.Threading.Tasks;


    [TestClass]
    public class GetPersonByNameTests : DataTestBase
    {


        protected GetPersonByNameHandler _handler;
        [TestInitialize]
        public void Setup()
        {
            _handler = new GetPersonByNameHandler(_dbContext);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnPerson_WhenPersonExists()
        {
            // Arrange
            (var people, var details, var duties) = await InsertTwoPeopleAndDuties();
            var person = people.Last();
            var activeDetail = details.FirstOrDefault(d => d.Person == person);

            var request = new GetPersonByName { Name = person.Name };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Person);
            Assert.AreEqual(person.Name, result.Person.Name);
            Assert.AreEqual(activeDetail.CurrentRank, result.Person.CurrentRank);
            Assert.AreEqual(activeDetail.CurrentDutyTitle, result.Person.CurrentDutyTitle);
            Assert.IsNull(result.Person.CareerEndDate);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnNull_WhenPersonNotFound()
        {
            var request = new GetPersonByName { Name = "Nonexistent Name" };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Person);
        }

    }
}