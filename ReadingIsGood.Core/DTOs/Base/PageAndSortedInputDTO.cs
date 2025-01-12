using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.DTOs.Base
{
    public class PagedAndSortedInputDto : PagedInputDto
    {
        public string? Sort { get; set; }
        public string? SortColumn { get; set; }

        public PagedAndSortedInputDto()
        {
        }
    }
}
