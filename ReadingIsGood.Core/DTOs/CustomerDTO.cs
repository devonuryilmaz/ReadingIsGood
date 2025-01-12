using ReadingIsGood.Core.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.DTOs
{
    public class CustomerDTO : BaseDTO
    {
        [StringLength(100)]
        public string FirstName { get; set; } = default!;
        [StringLength(100)]
        public string LastName { get; set; } = default!;
    }
}
