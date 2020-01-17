using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.EfCore
{
    public class ProjectDbContextFactory:IDesignTimeDbContextFactory<ProjectDbContext>
    {
        public static ProjectDbContext CreateDbContext()
        {
            var optionBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            var connection= new SqlConnectionStringBuilder()
            {
                DataSource = "KING-VAIO",
                InitialCatalog = "AmaProject",
                IntegratedSecurity =true,
            };

            optionBuilder.UseSqlServer(connection.ConnectionString);
            return new ProjectDbContext(optionBuilder.Options);



        }

        public ProjectDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }
    }
}
