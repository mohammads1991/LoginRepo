using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.EfCore
{
    class ProjectDbContextFactory:IDesignTimeDbContextFactory<ProjectDbContext>
    {
        public ProjectDbContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            var connection= new SqlConnectionStringBuilder()
            {
                DataSource = "(localdb)",
                InitialCatalog = "AmaProject",
                IntegratedSecurity =true,
            };

            optionBuilder.UseSqlServer(connection.ConnectionString);
            return new ProjectDbContext(optionBuilder.Options);



        }
    }
}
