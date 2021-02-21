using System.Collections.Generic;
using SP.ControlPanel.Data.Interfaces.Helpers;

namespace SP.ControlPanel.Data.Helpers
{
    public class PaginatedResult<T> : IPaginatedResult<T> where T:class
    {
        public IEnumerable<T> Items { get; set; }
        public long TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PaginatedResult(IEnumerable<T> items, long totalItems, int page, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            Page = page;
            PageSize = pageSize;
        }
    }
}