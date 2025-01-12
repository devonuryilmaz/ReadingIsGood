using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ReadingIsGood.Core.Entities;

namespace ReadingIsGood.Data.BaseContext
{
    public class DataContext : DbContext, IDataContext
    {
        public Guid InstanceId { get; }

        public DataContext() : base()
        {
            InstanceId = Guid.NewGuid();
        }

        public DataContext(DbContextOptions options) : base(options)
        {
            InstanceId = Guid.NewGuid();
        }

        public object GetEntries<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            return Entry(entity);
        }

        public virtual IDbContextTransaction CurrentTransaction { get { return Database.CurrentTransaction; } }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public override int SaveChanges()
        {
            var result = SaveChangesAsync(true);
            return result.Result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess)
        {
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess);
            return result;
        }

    }
}
