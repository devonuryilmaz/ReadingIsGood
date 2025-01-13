using ReadingIsGood.Business.Base.Concrete;
using ReadingIsGood.Business.Services.Interface;
using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Entities;
using ReadingIsGood.Core.Models;
using ReadingIsGood.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Business.Services.Concrete
{
    public class CustomerService : BaseService<Customer, CustomerDTO>, ICustomerService
    {
        public CustomerService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<Response<CustomerDTO>> Insert(CustomerRequest customerRequest)
        {
            var customerData = new Customer()
            {
                FirstName = customerRequest.FirstName,
                LastName = customerRequest.LastName,
            };

            var customer = await UnitOfWork.Repository<Customer>().AddAsync(customerData);

            return new Response<CustomerDTO>(Mapper.Map<CustomerDTO>(customer));
        }


        public async Task<Response<IEnumerable<OrderDTO>>> GetOrders(CustomerOrderRequest request)
        {
            var orders = UnitOfWork.Repository<Order>()
                .Filter(p => p.CustomerId == request.CustomerId,null,null, request.Page, request.PageSize);   

            return new Response<IEnumerable<OrderDTO>> (Mapper.Map<IEnumerable<OrderDTO>>(orders));
        }
    }
}
