using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;

namespace StargateTests.Infastructure
{
    [TestClass]
    public class DataTestBase
    {

        protected StargateContext _dbContext;

        [TestInitialize]
        public void TestInitialize()
        {
            // Initialize SQLite in-memory database
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<StargateContext>()
                .UseSqlite(connection)
                    .Options;

                // Create schema and seed data
                _dbContext = new StargateContext(options);
                _dbContext.Database.EnsureCreated();
            }
            catch (Exception)
            {
                connection.Close();
                throw;
            }
        }

        protected async Task<(IList<Person> People, IList<AstronautDetail> AstronautDetails, IList<AstronautDuty> AstronautDuties)> InsertTwoPeopleAndDuties()
        {
            List<Person> people = new()
            {
                new Person { Id = 1, Name = "John Doe" },
                new Person { Id = 2, Name = "Jane Doe" }
            };
        
            List<AstronautDetail> astronautDetails = new()
            {
                new AstronautDetail { Id = 1, PersonId = 1, CurrentRank = "Captain", CurrentDutyTitle = "Pilot", CareerStartDate = DateTime.Parse("2000-01-01") },
                new AstronautDetail { Id = 2, PersonId = 2, CurrentRank = "Major", CurrentDutyTitle = "Engineer", CareerStartDate = DateTime.Parse("2005-01-01") }
            };

            List<AstronautDuty> astronautDuties = new()
            {
                new AstronautDuty { Id = 1, PersonId = 1, Rank = "Captain", DutyTitle = "Pilot", DutyStartDate = DateTime.Parse("2000-01-01") },
                new AstronautDuty { Id = 2, PersonId = 2, Rank = "Major",DutyTitle = "Engineer", DutyStartDate = DateTime.Parse("2005-01-01") }
            };

            // Insert test data into the in-memory database directly
            await _dbContext.People.AddRangeAsync(people);
            await _dbContext.AstronautDetails.AddRangeAsync(astronautDetails);
            await _dbContext.AstronautDuties.AddRangeAsync(astronautDuties);

            await _dbContext.SaveChangesAsync();
            return (people, astronautDetails, astronautDuties);
        }

        protected async Task<(IList<Person> People, IList<AstronautDetail> AstronautDetails, IList<AstronautDuty> AstronautDuties)> InsertTwoPeopleWithTwoDuties()
        {
            List<Person> people = new()
            {
                new Person { Id = 1, Name = "John Doe" },
                new Person { Id = 2, Name = "Jane Doe" }
            };

            List<AstronautDetail> astronautDetails = new()
            {
                new AstronautDetail { Id = 1, PersonId = 1, CurrentRank = "Captain", CurrentDutyTitle = "Pilot", CareerStartDate = DateTime.Parse("2008-01-01") },
                new AstronautDetail { Id = 2, PersonId = 2, CurrentRank = "Major", CurrentDutyTitle = "Engineer", CareerStartDate = DateTime.Parse("2012-01-01") }
            };

            List<AstronautDuty> astronautDuties = new()
            {
                new AstronautDuty { Id = 1, PersonId = 1, Rank = "Initiate", DutyTitle = "Pilot", DutyStartDate = DateTime.Parse("2000-01-01"), DutyEndDate = DateTime.Parse("2008-01-01").AddDays(-1) },
                new AstronautDuty { Id = 2, PersonId = 2, Rank = "Beginner",DutyTitle = "Engineer", DutyStartDate = DateTime.Parse("2005-01-01"), DutyEndDate = DateTime.Parse("2012-01-01").AddDays(-1) },
                new AstronautDuty { Id = 3, PersonId = 1, Rank = "Captain", DutyTitle = "Pilot", DutyStartDate = DateTime.Parse("2008-01-01") },
                new AstronautDuty { Id = 4, PersonId = 2, Rank = "Major",DutyTitle = "Engineer", DutyStartDate = DateTime.Parse("2012-01-01") }
            };

            // Insert test data into the in-memory database directly
            await _dbContext.People.AddRangeAsync(people);
            await _dbContext.AstronautDetails.AddRangeAsync(astronautDetails);
            await _dbContext.AstronautDuties.AddRangeAsync(astronautDuties);

            await _dbContext.SaveChangesAsync();
            return (people, astronautDetails, astronautDuties);
        }

        protected async Task<(Person Person, AstronautDetail AstronautDetail, AstronautDuty AstronautDuty)> InsertExistingPersonAndDuty()
        {
            // Insert existing person and astronaut detail data into the in-memory database directly
            var person = new Person { Name = "John Doe" };
            var astronautDetail = new AstronautDetail
            {
                Person = person,
                CurrentDutyTitle = "Pilot",
                CurrentRank = "Captain",
                CareerStartDate = DateTime.Parse("2000-01-01"),
                CareerEndDate = null
            };
            var astronautDuty = new AstronautDuty
            {
                Person = person,
                DutyTitle = "Pilot",
                Rank = "Captain",
                DutyStartDate = DateTime.Parse("2000-01-01"),
            };

            _dbContext.People.Add(person);
            _dbContext.AstronautDetails.Add(astronautDetail);
            _dbContext.AstronautDuties.Add(astronautDuty);

            await _dbContext.SaveChangesAsync();
            return (person, astronautDetail, astronautDuty);
        }

        protected async Task<(AstronautDetail AstronautDetail, AstronautDuty AstronautDuty)> InsertRetired(Person person, DateTime retiredDate)
        {
            // Insert existing person and astronaut detail data into the in-memory database directly
            var astronautDetail = new AstronautDetail
            {
                Person = person,
                CurrentDutyTitle = "Pilot",
                CurrentRank = "RETIRED",
                CareerStartDate = retiredDate.AddDays(1),
                CareerEndDate = retiredDate
            };
            var astronautDuty = new AstronautDuty
            {
                Person = person,
                DutyTitle = "Pilot",
                Rank = "RETIRED",
                DutyStartDate = retiredDate.AddDays(1),
            };

            _dbContext.AstronautDetails.Add(astronautDetail);
            _dbContext.AstronautDuties.Add(astronautDuty);

            await _dbContext.SaveChangesAsync();
            return (astronautDetail, astronautDuty);
        }




        protected async Task<Person> InsertExistingPerson()
        {
            // Insert existing person and astronaut detail data into the in-memory database directly
            var person = new Person { Name = "John Doe" };
            _dbContext.People.Add(person);
            await _dbContext.SaveChangesAsync();
            return person;
        }
    }
}