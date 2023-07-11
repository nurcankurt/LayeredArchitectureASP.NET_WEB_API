using Moq;
using MyProject.BusinessLogicLayer;
using MyProject.DataAccessLayer.Models;

namespace MyProject.Test
{
    public class ApiTest
    {/*
        [Fact]
        public async Task MapGet_ReturnsPersonList()
        {
            //Arrange
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(service => service.GetAllPeople()).Returns(GetTestPersonList());
   

            // Act 
           
            //Assert

        }*/

        private List<Person> GetTestPersonList() {
            var persons = new List<Person>
            {
            
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
            return persons;
        }
    }
}