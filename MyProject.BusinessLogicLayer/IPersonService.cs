using MyProject.DataAccessLayer.Models;

namespace MyProject.BusinessLogicLayer
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPeople();
        Task<Person?> GetPersonById(int id);
        Task UpdatePerson(int id, Person person);
        Task CreatePerson(Person person);
        Task DeletePerson(int id);

        
        
 
    }
}
