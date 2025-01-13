using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Models.Requests
{
    public class BookStockUpdateRequest
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
