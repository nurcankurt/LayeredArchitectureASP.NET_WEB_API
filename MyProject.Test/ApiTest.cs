using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MyProject.BusinessLogicLayer;
using MyProject.DataAccessLayer.Models;
using System.Net;
using System.Net.Http.Json;

namespace MyProject.Test
{
    public class ApiTest
    {
        [Fact]
        public async Task MapGet_ReturnsPersonList()
        {
            //Arrange
            var personServiceMock = new Mock<IPersonService>();
            var expectedPeople = new List<Person> {
            new Person
                    {
                        Id = 1,
                        FirstName = "Nurcan",
                        LastName = "Kurt",
                        Email = "nurcan.kurt@test.com"
                    },
                    new Person
                    {
                        Id = 2,
                        FirstName = "TestName",
                        LastName = "TestLastName",
                        Email = "exampe@test.com"
                    }
            };
            personServiceMock.Setup(service => service.GetAllPeople()).ReturnsAsync(expectedPeople);

            using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        // Replace the registered IPersonService with the mocked instance
                        services.RemoveAll<IPersonService>();
                        services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                    });
                });
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/people");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task MapGet_WithExistingId_ReturnsPerson()
        {
            //Arrange
            var personServiceMock = new Mock<IPersonService>();
            var person = new Person
            {
                FirstName = "Nurcan",
                LastName = "Kurt",
                Email = "nurcan.kurt@test.com"
            };

            personServiceMock.Setup(service => service.GetPersonById(1)).ReturnsAsync(person);

            using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll<IPersonService>();
                        services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                    });
                });
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/people/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
           
        }

        [Fact]
        public async Task MapGet_WithNotExistingId_ReturnsNotFound()
        {
            //Arrange
            var personServiceMock = new Mock<IPersonService>();

            personServiceMock.Setup(service => service.GetPersonById(1)).ReturnsAsync((Person)null);

            using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll<IPersonService>();
                        services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                    });
                });
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/people/1");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task MapPost_WithNullValue_ReturnsBadRequest()
        {
            var personServiceMock = new Mock<IPersonService>();
            using var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IPersonService>();
                    services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                });
            });
            var client = application.CreateClient();

            // Act
            var result = await client.PostAsJsonAsync("/api/people", (Person)null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task MapPost_ReturnsCreated()
        {
            var personServiceMock = new Mock<IPersonService>();
            using var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IPersonService>();
                    services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                });
            });
            var client = application.CreateClient();

            var person = new Person
            {
                FirstName = "Nurcan",
                LastName = "Kurt",
                Email = "nurcan.kurt@test.com"
            };

            // Act
            var result = await client.PostAsJsonAsync("/api/people", person);

            // Assert
            result.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }
        [Fact]
        public async Task UpdatePerson_WithExistingId_ReturnsOk()
        {
            //Arrange
            var personServiceMock = new Mock<IPersonService>();
            var person = new Person
            {
                FirstName = "Nurcan",
                LastName = "Kurt",
                Email = "nurcan.kurt@test.com"
            };

            var updatePerson = new Person
            {
                FirstName = "TestName",
                LastName = "TestLastName",
                Email = "example@test.com"
            };

            personServiceMock.Setup(service => service.GetPersonById(1)).ReturnsAsync(person);
            personServiceMock.Setup(service => service.UpdatePerson(1,updatePerson)).Returns(Task.CompletedTask);


            using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll<IPersonService>();
                        services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                    });
                });
            using var client = application.CreateClient();

            // Act
            var result = await client.PutAsJsonAsync("api/people/1", updatePerson);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public async Task UpdatePerson_WithNotExistingId_ReturnsNotFound()
        {
            //Arrange
            var personServiceMock = new Mock<IPersonService>();

            var updatePerson = new Person
            {
                FirstName = "TestName",
                LastName = "TestLastName",
                Email = "example@test.com"
            };

            personServiceMock.Setup(service => service.GetPersonById(1)).ReturnsAsync((Person)null);
           

            using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll<IPersonService>();
                        services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                    });
                });
            using var client = application.CreateClient();

            // Act
            var result = await client.PutAsJsonAsync("api/people/1", updatePerson);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public async Task DeletePerson_WithExistingId_ReturnsOk()
        {
            //Arrange
            var personServiceMock = new Mock<IPersonService>();
            var person = new Person
            {
                Id = 1,
                FirstName = "Nurcan",
                LastName = "Kurt",
                Email = "nurcan.kurt@test.com"
            };

            personServiceMock.Setup(service => service.GetPersonById(1)).ReturnsAsync(person);
            personServiceMock.Setup(service => service.DeletePerson(1)).Returns(Task.CompletedTask);


            using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll<IPersonService>();
                        services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                    });
                });
            using var client = application.CreateClient();

            // Act
            var result = await client.DeleteAsync("/api/people/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public async Task DeletePerson_WithNotExistingId_ReturnsNotFound()
        {
            //Arrange
            var personServiceMock = new Mock<IPersonService>();

            personServiceMock.Setup(service => service.GetPersonById(1)).ReturnsAsync((Person)null);

            using var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.RemoveAll<IPersonService>();
                        services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                    });
                });
            using var client = application.CreateClient();

            // Act
            var result = await client.DeleteAsync("/api/people/1");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        
        }



    }
}