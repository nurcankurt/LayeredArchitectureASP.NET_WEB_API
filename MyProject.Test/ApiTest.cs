using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using MyProject.BusinessLogicLayer;
using MyProject.DataAccessLayer.Models;
using MyProject.WebApi;
using System.Net;
using System.Net.Http.Json;

namespace MyProject.Test
{
    public class ApiTest
    {

        [Fact]
        public async Task TestRootEndpoint()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            var response = await client.GetStringAsync("/");

            Assert.Equal("Hello World!", response);

        }

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
                        Email = "nkurt430"
                    },
                    new Person
                    {
                        Id = 2,
                        FirstName = "Test",
                        LastName = "Test",
                        Email = "Test"
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
            var result = await response.Content.ReadAsStringAsync();

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
                        // Replace the registered IPersonService with the mocked instance
                        services.RemoveAll<IPersonService>();
                        services.AddScoped<IPersonService>(_ => personServiceMock.Object);
                    });
                });
            using var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/people");
            var result = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }



        [Fact]
        public async Task MapPostTest()
        {
            await using var application = new WebApplicationFactory<Program>();
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

    }
}