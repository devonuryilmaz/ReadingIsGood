using ReadingIsGood.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Data.Base
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Query();

        ICollection<T> GetAll();

        Task<ICollection<T>> GetAllAsync();

        T GetById(long id);

        Task<T> GetByIdAsync(long id);

        T GetByUniqueId(string id);

        Task<T> GetByUniqueIdAsync(string id);
        IQueryable<T> FromSql(string sql);

        T Find(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        T Add(T entity);

        Task<T> AddAsync(T entity);

        void AddAll(IEnumerable<T> entities);

        Task AddAllAsync(IEnumerable<T> entities);

        T Update(T updated);

        Task<T> UpdateAsync(T updated);

        void UpdateAll(IEnumerable<T> entities);

        void Delete(T t, bool soft = true);

        Task<T> DeleteById(long id, bool soft = true);

        void DeleteAll(IEnumerable<T> t, bool soft = true);

        int Count();

        Task<int> CountAsync();

        IEnumerable<T> Filter(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? page = null,
            int? pageSize = null);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        bool Exist(Expression<Func<T, bool>> predicate);
    }
}
