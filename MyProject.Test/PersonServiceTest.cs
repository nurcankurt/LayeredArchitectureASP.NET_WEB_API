using MyProject.BusinessLogicLayer;
using MyProject.DataAccessLayer.Models;
using MyProject.DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace MyProject.Tests
{
    public class PersonServiceTests
    {
        private PersonDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<PersonDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var dbContext = new PersonDbContext(dbContextOptions);
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        [Fact]
        public async Task GetAllPeople_ReturnsAllPeople()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { FirstName = "Nurcan", LastName = "Kurt", Email = "nurcan.kurt@test.com" },
                new Person { FirstName = "TestName", LastName = "TestLastName", Email = "example@test.com" }
            };

            using (var dbContext = CreateDbContext())
            {
                dbContext.People.AddRange(people);
                dbContext.SaveChanges();

                var personService = new PersonService(dbContext);

                // Act
                var result = await personService.GetAllPeople();

                // Assert
                Assert.Equal(2, result.Count());
                Assert.Contains(result, p => p.FirstName == "Nurcan" && p.LastName == "Kurt" && p.Email == "nurcan.kurt@test.com");
                Assert.Contains(result, p => p.FirstName == "TestName" && p.LastName == "TestLastName" && p.Email == "example@test.com");
            }
        }

        [Fact]
        public async Task GetPersonById_WithExistingId_ReturnsPerson()
        {
            // Arrange
            var person = new Person { FirstName = "Nurcan", LastName = "Kurt", Email = "nurcan.kurt@test.com" };

            using (var dbContext = CreateDbContext())
            {
                dbContext.People.Add(person);
                dbContext.SaveChanges();

                var personService = new PersonService(dbContext);

                // Act
                var result = await personService.GetPersonById(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Nurcan", result.FirstName);
                Assert.Equal("Kurt", result.LastName);
                Assert.Equal("nurcan.kurt@test.com", result.Email);
            }
        }
        [Fact]
        public async Task GetPersonById_WithNotExistingId_ReturnsNull()
        {
            // Arrange

            using (var dbContext = CreateDbContext())
            {

                var personService = new PersonService(dbContext);

                // Act
                var result = await personService.GetPersonById(1);

                // Assert
                Assert.Null(result);

            }
        }

        [Fact]
        public async Task CreatePersonTest()
        {
            // Arrange
            var newPerson = new Person { FirstName = "Nurcan", LastName = "Kurt", Email = "nurcan.kurt@test.com" };

            using (var dbContext = CreateDbContext())
            {
                var personService = new PersonService(dbContext);

                //Act
                await personService.CreatePerson(newPerson);

                //Assert
                var createdPerson = dbContext.People.FirstOrDefault(p => p.FirstName == newPerson.FirstName && p.LastName == newPerson.LastName && p.Email == newPerson.Email);
                Assert.Equal("Nurcan",createdPerson.FirstName);
                Assert.Equal("Kurt",createdPerson.LastName);
                Assert.Equal("nurcan.kurt@test.com",createdPerson.Email);

            }
        }
        [Fact]
        public async Task UpdatePerson_WithExistingId()
        {
            // Arrange
            var person = new Person { FirstName = "TestName", LastName = "TestLastName", Email = "example@test.com" };

            using (var dbContext = CreateDbContext())
            {
                dbContext.People.Add(person);
                dbContext.SaveChanges();

                var personService = new PersonService(dbContext);
                var newPerson = new Person { FirstName = "Nurcan", LastName = "Kurt", Email = "nurcan.kurt@test.com" };

                //Act
                await personService.UpdatePerson(1, newPerson);

                // Assert
                var result = await personService.GetPersonById(1);
                Assert.Equal("Nurcan", result.FirstName);
                Assert.Equal("Kurt", result.LastName);
                Assert.Equal("nurcan.kurt@test.com", result.Email);
            }

        }

        [Fact]
        public async Task DeletePerson_WithExistingId()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { FirstName = "Nurcan", LastName = "Kurt", Email = "nurcan.kurt@test.com" },

            };

            using (var dbContext = CreateDbContext())
            {
                dbContext.People.AddRange(people);
                dbContext.SaveChanges();

                var personService = new PersonService(dbContext);

                // Act
                await personService.DeletePerson(1);

                // Assert
                var result = await personService.GetAllPeople();
                Assert.Empty(result);

            }
        }
    }
}
