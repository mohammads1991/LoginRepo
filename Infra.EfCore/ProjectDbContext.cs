using System;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.EfCore
{
    public class ProjectDbContext:DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ProjectDbContext(DbContextOptions options)
            :base(options)
        {
            
        }
    }
}
