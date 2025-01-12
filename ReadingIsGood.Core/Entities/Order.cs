using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public int Qunatity { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Amount { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Book Book { get; set; }
    }
}
