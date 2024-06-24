namespace StargateTests.UnitTests.Queries
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using StargateAPI.Business.Dtos;
    using StargateAPI.Business.Queries;
    using StargateTests.Infastructure;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [TestClass]
    public class GetPeopleTests : DataTestBase
    {
        protected GetPeopleHandler _handler;
        [TestInitialize]
        public void Setup()
        {
            _handler = new GetPeopleHandler(_dbContext);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnPeople_WhenQueryIsSuccessful()
        {
            // Arrange
            (var people, var details, var duties) = await InsertTwoPeopleAndDuties();

            var request = new GetPeople();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(people.Count, result.People.Count);
            foreach (var person in people)
            {
                Assert.IsNotNull(result.People.Find(p => p.Name == person.Name));
            }
        }

        [TestMethod]
        public async Task Handle_ShouldReturnEmptyList_WhenNoPeopleFound()
        {
            // Arrange
            var people = new List<PersonAstronaut>();

            var request = new GetPeople();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.People.Count);
        }
    }
}