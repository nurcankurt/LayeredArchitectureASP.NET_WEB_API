using Microsoft.EntityFrameworkCore;
using MyProject.DataAccessLayer.Models;

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
