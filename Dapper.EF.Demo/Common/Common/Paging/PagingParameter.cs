using System;
using System.Collections.Generic;

namespace Common.Common.Paging
{
    public class PagingParameter
    {
        /// <summary>
        /// Danh sách cột cần lấy, lấy hết để mảng rỗng
        /// </summary>
        public IEnumerable<string> Columns { get; set; }

        /// <summary>
        /// Danh sách Filter
        /// </summary>
        public IEnumerable<Filter> Filters { get; set; }

        /// <summary>
        /// Số bản ghi
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// Số trang
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Sắp xếp theo
        /// </summary>
        public string OrderBy { get; set; }
    }
}
