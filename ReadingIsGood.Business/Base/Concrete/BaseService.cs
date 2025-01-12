using Microsoft.EntityFrameworkCore;
using ReadingIsGood.Business.Base.Interface;
using ReadingIsGood.Core.DTOs.Base;
using ReadingIsGood.Core.Entities;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ReadingIsGood.Business.Base.Concrete
{
    public partial class BaseService<TEntity, TDto> : BusinessService, IBaseService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : BaseDTO
    {
        public BaseService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public virtual async Task Add(TDto dto)
        {
            await BaseUnitOfWork().Repository<TEntity>().AddAsync(Mapper.Map<TEntity>(dto));
            return;
        }

        public virtual async Task Update(TDto dto)
        {
            await BaseUnitOfWork().Repository<TEntity>().UpdateAsync(Mapper.Map<TEntity>(dto));
            return;
        }

        public virtual async Task<TEntity> GetById(long id)
        {
            return await BaseUnitOfWork().Repository<TEntity>().GetByIdAsync(id);
        }

        public virtual long Count()
        {
            return BaseUnitOfWork().Repository<TEntity>().Query().Count();
        }

        public virtual IQueryable<TEntity> Order(IQueryable<TEntity> query, string sorting, string columName)
        {
            sorting = String.IsNullOrEmpty(sorting) ? "DESC" : sorting;
            columName = String.IsNullOrEmpty(columName) ? "ID" : columName;
            return query.OrderBy(columName + " " + sorting.ToUpper());
        }

        public virtual long Count(Expression<Func<TEntity, bool>> filter)
        {
            return BaseUnitOfWork().Repository<TEntity>().Query().Count(filter);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter)
        {
            var result = BaseUnitOfWork().Repository<TEntity>().Query().Any(filter);
            return result;
        }

        protected virtual async Task<PagedResultDtoCustom<TDto>> PagedListAsync(BaseFilterDTO filterDTO, IQueryable<TEntity> query)
        {
            var totalRecord = query.Count();

            query = Order(query, filterDTO.Sort, filterDTO.SortColumn);

            if (filterDTO.Page.HasValue && filterDTO.PerPage.HasValue)
                query = query.Skip(Convert.ToInt32((filterDTO.Page - 1) * filterDTO.PerPage))
                             .Take(Convert.ToInt32(filterDTO.PerPage));

            var totalCount = totalRecord;

            var queryList = await query.ToListAsync();

            var BaslangicIndex = queryList.FindIndex(x => x.ID == queryList[0].ID) + 1;
            var BitisIndex = queryList.FindIndex(x => x.ID == queryList[queryList.Count() - 1].ID) + 1;

            var totalpages = 0;
            if (filterDTO.PerPage != null && totalRecord > 0)
            {
                totalpages = Convert.ToInt32(Math.Ceiling((Convert.ToDecimal(totalRecord) / Convert.ToDecimal(filterDTO.PerPage))));
            }

            var dtoList = Mapper.Map<List<TDto>>(queryList);
            var pageResult = new PagedResultDtoCustom<TDto>(BaslangicIndex, BitisIndex, totalpages, totalCount, dtoList);

            return pageResult;
        }

        protected virtual PagedResultDtoCustom<TDto> PagedList(BaseFilterDTO filterDTO, IQueryable<TEntity> query, bool sortByID = true)
        {
            var totalRecord = query.Count();

            query = Order(query, filterDTO.Sort, filterDTO.SortColumn);

            if (filterDTO.Page.HasValue && filterDTO.PerPage.HasValue)
                query = query.Skip(Convert.ToInt32((filterDTO.Page - 1) * filterDTO.PerPage))
                             .Take(Convert.ToInt32(filterDTO.PerPage));

            var totalCount = totalRecord;
            var queryList = sortByID ? query.OrderByDescending(o => o.ID).ToList() : query.ToList();

            var BaslangicIndex = queryList.FindIndex(x => x.ID == queryList[0].ID) + 1;
            var BitisIndex = queryList.FindIndex(x => x.ID == queryList[queryList.Count() - 1].ID) + 1;

            var totalpages = 0;
            if (filterDTO.PerPage != null && totalRecord > 0)
            {
                totalpages = Convert.ToInt32(Math.Ceiling((Convert.ToDecimal(totalRecord) / Convert.ToDecimal(filterDTO.PerPage))));
            }

            var dtoList = Mapper.Map<List<TDto>>(queryList);
            var pageResult = new PagedResultDtoCustom<TDto>(BaslangicIndex, BitisIndex, totalpages, totalCount, dtoList);

            return pageResult;
        }
    }
}
