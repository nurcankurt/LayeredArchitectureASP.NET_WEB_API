namespace MyProject.DataAccessLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using global::MyProject.DataAccessLayer.Models;

        public class PersonRepository : IPersonRepository
        {
            protected readonly PersonDbContext _context;
            public PersonRepository(PersonDbContext context)
            {
                _context = context;
            }
            public void Add(Person person)
            {
                _context.Add(person);
                _context.SaveChanges();
            }

            public void Delete(Person person)
            {
                _context.Remove(person);
                _context.SaveChanges();
            }

            public Person Get(int id)
            {
               
               // return _context.People.Find(x => x.Id == id);
               throw new NotImplementedException();
            }

            public List<Person> GetAll()
            {
                return _context.People.ToList();

            }

            public void Update(Person person)
            {
                _context.Update(person);
                _context.SaveChanges(); 
            }
        }
    }
         

