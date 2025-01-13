using ReadingIsGood.Business.Base.Interface;
using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Entities;
using ReadingIsGood.Core.Models;
using ReadingIsGood.Core.Models.Requests;

namespace ReadingIsGood.Business.Services.Interface
{
    public interface ICustomerService : IBaseService<Customer, CustomerDTO>
    { 
        public Task<Response<CustomerDTO>> Insert(CustomerRequest customer);
        public Task<Response<IEnumerable<OrderDTO>>> GetOrders(CustomerOrderRequest request);
    }
}
