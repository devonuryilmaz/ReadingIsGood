using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Models.Requests
{
    public class BookRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
