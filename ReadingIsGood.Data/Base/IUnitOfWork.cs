using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using ReadingIsGood.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Data.Base
{
    public interface IUnitOfWork<TContext> : IUnitOfWork
    {
    }

    public interface IUnitOfWork : IDisposable
    {
        public User User { get; set; }

        IRepository<T> Repository<T>() where T : BaseEntity;

        Task<int> Commit();

        int SaveChanges();

        void Rollback();

        bool CommitTransaction();

        void RollbackTransaction();

        Task<IList<TModel>> ExecuteSqlQueryAsync<TModel>(string query) where TModel : class, new();

        Task ExecuteNonProcedure(string query, IList<SqlParameter> parameters);

        IDbContextTransaction CurrentTransaction { get; }

        IDbContextTransaction BeginTransaction();

        void Dispose(bool disposing);
    }
}
