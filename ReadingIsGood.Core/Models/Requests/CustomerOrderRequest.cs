using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Models.Requests
{
    public class CustomerOrderRequest
    {
        [Required]
        public int CustomerId { get; set; }

        [DefaultValue(1)]
        public int Page { get; set; }

        [DefaultValue(10)]
        public int PageSize { get; set; }
    }
}
