using MyProject.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DataAccessLayer
{
    public interface IPersonRepository
    {
        List<Person> GetAll();
        Person Get(int id);
        void Add(Person person);
        void Update(Person person);
        void Delete(Person person);
    }

}
