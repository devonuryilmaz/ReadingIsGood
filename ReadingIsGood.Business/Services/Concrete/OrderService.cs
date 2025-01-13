using ReadingIsGood.Business.Base.Concrete;
using ReadingIsGood.Business.Services.Interface;
using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Entities;
using ReadingIsGood.Core.Models;
using ReadingIsGood.Core.Models.Requests;
using ReadingIsGood.Core.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Business.Services.Concrete
{
    public class OrderService : BaseService<Order, OrderDTO>, IOrderService
    {
        public OrderService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<Response<IEnumerable<StatisticMonthlyReportModel>>> GetMonthlyStatistics()
        {
            var orders = (await UnitOfWork.Repository<Order>().GetAllAsync()).ToList();

            var groupedData = orders.GroupBy(x => new { x.OrderDate.Month }).
                Select(p => new StatisticMonthlyReportModel()
                {
                    Month = p.Key.Month,
                    TotalBookCount = p.Sum(g => g.Quantity),
                    TotalOrderCount = p.Sum(g => g.ID),
                    TotalPurchasedAmount = p.Sum(g => g.Amount)
                })
                .OrderBy(x => x.Month);

            return new Response<IEnumerable<StatisticMonthlyReportModel>>(groupedData);
        }

        public async Task<Response<OrderDTO>> Insert(OrderRequest order)
        {
            var book = (await UnitOfWork.Repository<Book>()
                .GetByIdAsync(order.BookId));

            if (book == null)
            {
                return new Response<OrderDTO>(null, false)
                {
                    Errors = new List<Error>()
                    {
                        new Error()
                        {
                            ErrorMessage = "Book record not found."
                        }
                    }
                };
            }
            else if (book.Quantity <= 0 || order.Quantity > book.Quantity)
            {
                return new Response<OrderDTO>(null, false)
                {
                    Errors = new List<Error>()
                    {
                        new Error()
                        {
                            ErrorMessage = "Book quantity is not enough."
                        }
                    }
                };
            }

            var customer = (await UnitOfWork.Repository<Customer>()
                .GetByIdAsync(order.CustomerId));

            if (customer == null)
            {
                return new Response<OrderDTO>(null, false)
                {
                    Errors = new List<Error>()
                    {
                        new Error()
                        {
                            ErrorMessage = "Customer record not found."
                        }
                    }
                };
            }

            var newOrder = new Order()
            {
                CustomerId = order.CustomerId,
                BookId = order.BookId,
                Amount = order.Quantity * book.Price,
                Quantity = order.Quantity,
                OrderDate = DateTime.Now,
            };

            var result = await UnitOfWork.Repository<Order>().AddAsync(newOrder);

            book.Quantity = book.Quantity - order.Quantity;
            _ = UnitOfWork.Repository<Book>().Update(book);

            return new Response<OrderDTO>(Mapper.Map<OrderDTO>(result), true);
        }

        public async Task<Response<IEnumerable<OrderDTO>>> ListOrderById()
        {
            var result = (await UnitOfWork.Repository<Order>().GetAllAsync()).OrderBy(p => p.ID);

            return new Response<IEnumerable<OrderDTO>>(Mapper.Map<IEnumerable<OrderDTO>>(result), true);
        }

    }
}
