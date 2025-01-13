using Microsoft.EntityFrameworkCore;
using ReadingIsGood.Core.Entities;
using ReadingIsGood.Data.BaseContext;
using System.Linq.Expressions;

namespace ReadingIsGood.Data.Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public Repository(DataContext context, IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
            _context = context;
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().Where(x => !x.IsDeleted).AsQueryable();
        }

        #region Get

        public ICollection<T> GetAll()
        {
            return _context.Set<T>().Where(x => x.IsDeleted == false).ToList();
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().Where(x => x.IsDeleted == false).ToListAsync();
        }

        public T GetById(long id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(p => p.ID == id);
        }

        public T GetByUniqueId(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByUniqueIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        #endregion Get

        #region Find

        public T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }

        public IQueryable<T> FromSql(string sql)
        {
            return _context.Set<T>().FromSqlRaw(sql);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }

        #endregion Find

        #region Add

        public T Add(T entity)
        {
            FillEntity(entity, true);

            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            FillEntity(entity, true);
            await _context.Set<T>().AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }

        public void AddAll(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(entity =>
            {
                FillEntity(entity, true);
            });

            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();
        }

        public async Task AddAllAsync(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(entity =>
            {
                FillEntity(entity, true);
            });

            await _context.Set<T>().AddRangeAsync(entities);
            _context.SaveChanges();
        }

        #endregion Add

        #region Update

        public T Update(T entity)
        {
            if (entity == null)
            {
                return null;
            }

            FillEntity(entity);
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                return null;
            }

            FillEntity(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            await _unitOfWork.Commit();
            return entity;
        }

        public void UpdateAll(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(entity =>
            {
                FillEntity(entity);
            });
            _context.Set<T>().UpdateRange(entities);
            _context.SaveChanges();
        }

        #endregion Update

        #region Delete

        public void Delete(T entity, bool soft = true)
        {
            if (soft)
            {
                entity.IsDeleted = true;
                FillEntity(entity);

                _context.Set<T>().Update(entity);
            }
            else
            {
                _context.Set<T>().Remove(entity);
            }
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task<T> DeleteById(long id, bool soft = true)
        {
            T entity = await _context.Set<T>().FindAsync(id);

            if (entity == null) return null;
            if (soft)
            {
                entity.IsDeleted = true;
                FillEntity(entity);
                _context.Set<T>().Update(entity);
            }
            else
            {
                _context.Set<T>().Remove(entity);
            }
            _context.SaveChanges();
            return entity;
        }

        public void DeleteAll(IEnumerable<T> entity, bool soft = true)
        {
            if (soft)
            {
                entity.ToList().ForEach(x => { x.IsDeleted = true; FillEntity(x); });
                _context.Set<T>().UpdateRange(entity);
            }
            else
            {
                _context.Set<T>().RemoveRange(entity);
            }
            _context.SaveChanges();
        }

        #endregion Delete

        #region Count/Filter/Exist

        public int Count()
        {
            return _context.Set<T>().Where(x => x.IsDeleted == false).Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().Where(x => x.IsDeleted == false).CountAsync();
        }

        public IEnumerable<T> Filter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? page = null,
            int? pageSize = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includeProperties != null)
            {
                foreach (
                    var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query.ToList();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            var exist = _context.Set<T>().Where(predicate);
            return exist.Any() ? true : false;
        }

        #endregion Count/Filter/Exist

        private void FillEntity(T entity, bool insertMode = false)
        {
            entity.ModifiedBy = _unitOfWork?.User == null ? null : _unitOfWork.User.ID;
            entity.ModifyDate = DateTime.Now;

            if (insertMode)
            {
                entity.CreatedBy = _unitOfWork?.User == null ? null : _unitOfWork.User.ID;
                entity.CreatedDate = DateTime.Now;
            }
        }
    }
}
