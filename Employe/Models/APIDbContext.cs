using Microsoft.EntityFrameworkCore;

namespace EmployeAPI.Models
{
    public class APIDbContext:DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> option):base(option)
        {
            
        }

        public DbSet<Employe> Employes { get; set; }
    }
}
