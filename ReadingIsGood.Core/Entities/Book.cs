using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }
    }
}
