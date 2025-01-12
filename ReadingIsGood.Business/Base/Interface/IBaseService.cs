using ReadingIsGood.Core.DTOs.Base;
using ReadingIsGood.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Business.Base.Interface
{
    public interface IBaseService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : BaseDTO
    {
        Task Add(TDto entity);

        Task Update(TDto entity);

        Task<TEntity> GetById(long id);

        long Count();

        long Count(Expression<Func<TEntity, bool>> filter);

        bool Any(Expression<Func<TEntity, bool>> filter);

        IQueryable<TEntity> Order(IQueryable<TEntity> query, string sorting, string columName);
    }
}
