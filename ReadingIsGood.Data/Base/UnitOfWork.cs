using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ReadingIsGood.Data.BaseContext;

namespace ReadingIsGood.Data.Base
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DataContext
    {
        private TContext _dbContext;
        private IDbContextTransaction _transaction;
        private bool _disposed;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public Dictionary<Type, object> Repositories
        {
            get { return _repositories; }
            set { Repositories = value; }
        }

        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IRepository<T>;
            }

            IRepository<T> repo = new Repository<T>(_dbContext, this);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        #region Execute Query
        public async Task<IList<TModel>> ExecuteSqlQueryAsync<TModel>(string query) where TModel : class, new()
        {
            List<TModel> result = new List<TModel>();

            var type = typeof(TModel);
            var members = type.GetMembers();

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    command.Connection.Open();
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    using (var dr = await command.ExecuteReaderAsync())
                    {
                        while (dr.Read())
                        {
                            TModel item = (TModel)Activator.CreateInstance(type);
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                if (!dr.IsDBNull(i))
                                {
                                    string fieldName = dr.GetName(i);
                                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        PropertyInfo piInstance = type.GetProperty(fieldName);
                                        piInstance.SetValue(item, dr.GetValue(i));
                                    }
                                }
                            }
                            result.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
            return result;
        }

        public async Task ExecuteNonProcedure(string query, IList<SqlParameter> parameters)
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    command.Connection.Open();
                    command.CommandText = query;
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null && parameters.Count() > 0) command.Parameters.AddRange(parameters.ToArray());

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }
        #endregion

        #region Unit of Work Transactions

        public virtual IDbContextTransaction CurrentTransaction
        {
            get
            {
                return _dbContext.CurrentTransaction;
            }
        }

        public User User { get; set; }

        public IDbContextTransaction BeginTransaction()
        {
            _transaction = _dbContext.BeginTransaction();
            return _transaction;
        }

        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public void Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }

        public bool CommitTransaction()
        {
            _transaction.Commit();
            return true;
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }

            _disposed = true;
        }

        #endregion
    }
}
