﻿using MyProject.DataAccessLayer.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.BusinessLogicLayer
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPeople();

        Task<Person> GetPersonById(int id);
        void CreatePerson(Person person);

        void DeletePerson(Person entity);

        
        
 
    }
}
