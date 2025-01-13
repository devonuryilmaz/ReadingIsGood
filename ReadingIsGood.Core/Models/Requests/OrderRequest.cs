using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Models.Requests
{
    public class OrderRequest
    {
        [Required]
        public int BookId { get; set; }
        
        [Required]
        public int CustomerId { get; set; }

        public int Quantity { get; set; }
    }
}
