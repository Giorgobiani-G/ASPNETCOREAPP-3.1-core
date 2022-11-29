using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ASPNETCOREAPP.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        //public DatabaseContext()
        //{
        //}

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}

        public DbSet<EmploeeModel> Emploees { get; set; }
        public DbSet<Listmodel> Listmodels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e=> e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
