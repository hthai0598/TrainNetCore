using BH_SocialNetwork_Models.Comman;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH_SocialNetwork_Models
{
    public class PagingParameter
    {
        #region
        /// <summary>
        /// Danh sách cột cần lấy, lấy hết để mảng rỗng
        /// </summary>
        public IEnumerable<string> Columns { get; set; }
        /// <summary>
        /// Số bản ghi
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// Danh sách Filter
        /// </summary>
        public IEnumerable<Filter> Filters { get; set; }
        /// <summary>
        /// Số trang
        /// </summary>
        public int Page { get; set; }
        #endregion

    }
}
