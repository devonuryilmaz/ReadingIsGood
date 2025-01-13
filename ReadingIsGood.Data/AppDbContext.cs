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
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntitySetting(modelBuilder.Entity<User>());
        }
    }
}
