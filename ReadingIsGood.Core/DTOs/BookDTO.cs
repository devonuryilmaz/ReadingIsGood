using ReadingIsGood.Core.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.DTOs
{
    public class BookDTO : BaseDTO
    {
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }
    }
}
