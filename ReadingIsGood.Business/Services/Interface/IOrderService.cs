using ReadingIsGood.Business.Base.Interface;
using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Entities;
using ReadingIsGood.Core.Models;
using ReadingIsGood.Core.Models.Requests;
using ReadingIsGood.Core.Models.Responses;

namespace ReadingIsGood.Business.Services.Interface
{
    public interface IOrderService : IBaseService<Order, OrderDTO>
    {
        public Task<Response<OrderDTO>> Insert(OrderRequest order);

        public Task<Response<IEnumerable<OrderDTO>>> ListOrderById();
        public Task<Response<IEnumerable<StatisticMonthlyReportModel>>> GetMonthlyStatistics();
    }
}
