using Abp.Runtime.Validation;

namespace ReadingIsGood.Core.DTOs.Base
{
    public class BaseFilterDTO : PagedAndSortedInputDto, IShouldNormalize
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public string? q { get; set; }



        public DateTime? CreatedDateStart { get; set; }
        public DateTime? CreatedDateEnd { get; set; }
        public int? Page { get; set; }
        public int? PerPage { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sort))
            {
                Sort = "ASC";
            }

            Sort = Sort.Replace("editionDisplayName", "Edition.DisplayName");
        }
    }
}
