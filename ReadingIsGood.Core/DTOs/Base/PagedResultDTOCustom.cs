using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.DTOs.Base
{
    public class PagedResultDtoCustom<T> : PagedResultDto<T>
    {
        public PagedResultDtoCustom() : base()
        {
        }

        public PagedResultDtoCustom(int startIndex, int endIndex, int totalPages, int totalCount, IReadOnlyList<T> items) : base(totalCount, items)
        {
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
            this.TotalPages = totalPages;
            Total = totalCount;
        }

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int TotalPages { get; set; }
        public int Total { get; set; }
    }
}
