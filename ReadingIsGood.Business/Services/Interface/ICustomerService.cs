using ReadingIsGood.Core.DTOs;
using ReadingIsGood.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Business.Services.Interface
{
    public interface ICustomerService
    {
        public Task<CustomerDTO> Insert(CustomerRequest customer);
    }
}
