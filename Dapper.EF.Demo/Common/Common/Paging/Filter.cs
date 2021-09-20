using System;
using Common.Enum;

namespace Common.Common.Paging
{
    public class Filter
    {
        #region Declerations

        /// <summary>
        /// Trường Filter
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Kiểu Filter
        /// </summary>
        public Enumeration.FilterType FilterType { get; set; }

        /// <summary>
        /// Kiểu dữ liệu
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Giá trị Filter
        /// </summary>
        public string FilterValue { get; set; }

        #endregion
    }
}
