using ReadingIsGood.Core.DTOs.Base;
using ReadingIsGood.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.DTOs
{
    public class OrderDTO : BaseDTO
    {
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Amount { get; set; }


        public virtual Customer Customer { get; set; }
        public virtual Book Book { get; set; }
    }
}
