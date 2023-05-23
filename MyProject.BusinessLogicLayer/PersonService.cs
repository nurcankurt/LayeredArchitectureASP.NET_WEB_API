using MyProject.DataAccessLayer.Models;
using MyProject.DataAccessLayer;


namespace MyProject.BusinessLogicLayer
{
    public class PersonService : IPersonService
    {
        private IPersonRepository _personRepository;
        public PersonService(IPersonRepository personRepository) 
        {
            _personRepository = personRepository;  

        }

        public void CreatePerson(Person person)
        {
            _personRepository.Add(person);
        }

        public void DeletePerson(Person entity)
        {
            _personRepository.Delete(entity);
        }

        public List<Person> GetAllPeople()
        {
            return _personRepository.GetAll();
        }

        public Person GetPerson(int id)
        {
            return _personRepository.Get(id);
        }
    }
}
