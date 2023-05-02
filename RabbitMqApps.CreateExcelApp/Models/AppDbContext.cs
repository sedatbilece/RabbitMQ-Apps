﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RabbitMqApps.CreateExcelApp.Models
{
    public class AppDbContext :IdentityDbContext
    {



        public AppDbContext(DbContextOptions options) : base(options)
        {
                
        }
        public DbSet<UserFile> UserFiles { get; set; }
    }
}
