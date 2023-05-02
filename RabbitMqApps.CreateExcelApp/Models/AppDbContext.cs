using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RabbitMqApps.CreateExcelApp.Models
{
    public class AppDbContext :IdentityDbContext
    {



        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
                
        }


        public DbSet<UserFile> UserFiles { get; set; }
    }
}
