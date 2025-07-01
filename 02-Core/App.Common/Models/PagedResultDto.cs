using System.Collections.Generic;

namespace App.Common.Models
{
    public sealed class PagedResultDto<TModel>
    {
        public List<TModel> Records { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; }
        public bool IsFirstPage => PageNumber <= 1;
        public bool IsLastPage => TotalRecords / (PageSize == 0 ? 1 : PageSize) == PageNumber;
    }
}
