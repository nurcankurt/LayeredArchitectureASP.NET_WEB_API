using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
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
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

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

            // Act
            var response = await client.GetAsync("/api/people");
            var result = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task MapPostTest()
        {
            await using var application = new WebApplicationFactory<Program>();


            var client = application.CreateClient();

            var result = await client.PostAsJsonAsync("/api/people", new Person
            {
                FirstName = "Nurcan",
                LastName = "Kurt",
                Email = "nurcan.kurt@test.com"
            });

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }

    }
}