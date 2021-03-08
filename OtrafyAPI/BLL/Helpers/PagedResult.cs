using System;
using System.Collections.Generic;

namespace BLL.Helpers
{
    public class PagedResult<T>
    {
        public class PagingInfo
        {
            public int PageNumber { get; set; }

            public int PageSize { get; set; }

            public int PageCount { get; set; }

            public long TotalRecordCount { get; set; }

        }
        public List<T> Data { get; private set; }

        public PagingInfo Paging { get; private set; }

        public PagedResult(IEnumerable<T> items, int pageNumber, int pageSize, long totalRecordCount)
        {
            Data = new List<T>(items);
            Paging = new PagingInfo
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecordCount = totalRecordCount,
                PageCount = totalRecordCount > 0  ? (int)Math.Ceiling(totalRecordCount / (double)pageSize): 0
            };
        }
    }
}
