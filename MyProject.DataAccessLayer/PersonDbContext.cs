using Microsoft.EntityFrameworkCore;
using MyProject.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DataAccessLayer
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions options) :
            base(options)
        {

        }
        public DbSet<Person> People { get; set; }
    }
}
