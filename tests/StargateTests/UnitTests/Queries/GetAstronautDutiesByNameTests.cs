using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Queries;
using StargateTests.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StargateTests.UnitTests.Queries
{
    [TestClass]
    public class GetAstronautDutiesByNameTests :DataTestBase
    {

        protected GetAstronautDutiesByNameHandler _handler;
        [TestInitialize]
        public void Setup()
        {
            _handler = new GetAstronautDutiesByNameHandler(_dbContext);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnEmptyList_WhenNoPeople()
        {
            // Arrange
            var request = new GetAstronautDutiesByName{ Name = "John Doe" };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.AreEqual(result.AstronautDuties.Count, 0 );
        }

        [TestMethod]
        public async Task Handle_ShouldReturnSpecificPerson()
        {
            (var people, var details, var duties) = await InsertTwoPeopleWithTwoDuties();
            var person = people.Last();
            var astronautDuties = person.AstronautDuties;


            var request = new GetAstronautDutiesByName { Name = person.Name };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Person.Name, person.Name);
            Assert.AreEqual(result.AstronautDuties.Count, astronautDuties.Count());
            Assert.IsTrue(result.AstronautDuties.All(ad => ad.PersonId == person.Id));
            foreach (var duty in astronautDuties) 
            {
                Assert.IsTrue(result.AstronautDuties.Exists( z => z.DutyTitle == duty.DutyTitle
                && z.Rank == duty.Rank
                && z.DutyStartDate == duty.DutyStartDate
                && z.DutyEndDate == z.DutyEndDate));
            }
        }

    }
}
