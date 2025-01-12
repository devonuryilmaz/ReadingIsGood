using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Entities
{
    public class User : BaseEntity
    {
        [StringLength(100)]
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        [StringLength(100)]
        public string FirstName { get; set; } = default!;
        [StringLength(100)]
        public string LastName { get; set; } = default!;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
    }
}
