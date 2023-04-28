using Microsoft.EntityFrameworkCore;

namespace RabbitMqApps.WatermarkApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

            
        }



        public DbSet<Product> Products { get; set; }

    }
}
