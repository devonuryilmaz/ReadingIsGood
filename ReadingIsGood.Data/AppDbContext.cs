using Microsoft.EntityFrameworkCore;
using ReadingIsGood.Core.Entities;
using ReadingIsGood.Data.BaseContext;
using ReadingIsGood.Data.EntitySettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Data
{
    public class AppDbContext: DataContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntitySetting(modelBuilder.Entity<User>());
        }
    }
}
