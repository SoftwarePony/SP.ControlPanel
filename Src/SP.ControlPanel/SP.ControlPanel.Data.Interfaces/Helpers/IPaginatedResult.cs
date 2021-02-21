using System.Collections.Generic;

namespace SP.ControlPanel.Data.Interfaces.Helpers
{
    public interface IPaginatedResult<T> where T:class
    {
        public IEnumerable<T> Items { get; set; }
        public long TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}