using System;
using System.Collections.Generic;

namespace Common.Common.Paging
{
    public class PagingData<T>
    {
        /// <summary>
        /// Danh sách các thực thể
        /// </summary>
        public IEnumerable<T> Entities { get; set; }
        /// <summary>
        /// Số bản ghi
        /// </summary>
        public int TotalRecord { get; set; }
        /// <summary>
        /// Số trang
        /// </summary>
        public int TotalPage { get; set; }
    }
}
