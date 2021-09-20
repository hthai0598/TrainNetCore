using System;
using System.Collections.Generic;
using Common.Enum;

namespace Common.Common.Paging
{
    public class PagingHelper
    {
        #region Methods

        /// <summary>
        /// Build câu điều kiện từ danh sách các filter
        /// </summary>
        /// <param name="filters">Danh sách các Filter</param>
        /// <returns>Câu điều kiện</returns>
        /// Created date: 22/07/2019
        /// Author: Phan Hồng Nam_PHNam
        public static string BuildContionString(IEnumerable<Filter> iFilters)
        {
            string condition = "",
                where = "";
            var filters = (List<Filter>)iFilters;
            var filterString = new List<string>();

            for (int i = 0, length = filters.Count; i < length; i++)
            {
                switch (filters[i].FilterType)
                {
                    case Enumeration.FilterType.Contain:
                        where = string.Format("{0} LIKE N'%{1}%'", filters[i].FieldName, filters[i].FilterValue);
                        break;
                    case Enumeration.FilterType.NotContain:
                        where = string.Format("{0} NOT LIKE N'%{1}%'", filters[i].FieldName, filters[i].FilterValue);
                        break;
                    case Enumeration.FilterType.BeginWith:
                        where = string.Format("{0} LIKE N'{1}%'", filters[i].FieldName, filters[i].FilterValue);
                        break;
                    case Enumeration.FilterType.EndWith:
                        where = string.Format("{0} LIKE N'%{1}'", filters[i].FieldName, filters[i].FilterValue);
                        break;
                    case Enumeration.FilterType.Greater:
                        where = string.Format("{0} > '{1}'", filters[i].FieldName, filters[i].FilterValue);
                        break;
                    case Enumeration.FilterType.GreaterOrEqual:
                        where = string.Format("{0} >= '{1}'", filters[i].FieldName, filters[i].FilterValue);
                        break;
                    case Enumeration.FilterType.Less:
                        where = string.Format("{0} < '{1}'", filters[i].FieldName, filters[i].FilterValue);
                        break;
                    case Enumeration.FilterType.LessOrEqual:
                        where = string.Format("{0} <= '{1}'", filters[i].FieldName, filters[i].FilterValue);
                        break;
                    default:
                        break;
                }
                filterString.Add(where);
            }

            condition = String.Join(" AND ", filterString.ToArray());

            return condition.ToString();
        }

        /// <summary>
        /// Build câu select
        /// </summary>
        /// <param name="iColumns">Danh sách các trường lấy dữ liệu</param>
        /// <returns>Câu select</returns>
        /// Created date: 22/07/2019
        /// Author: Phan Hồng Nam_PHNam
        public static string BuildSelectString(IEnumerable<string> iColumns)
        {
            string selectString = "";
            var columns = (List<string>)iColumns;
            int length = columns.Count;

            if (length == 0)
            {
                selectString = "*";
            }
            else if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    columns[i] = string.Format("[{0}]", columns[i]);
                }
                selectString = String.Join(",", columns);
            }

            return selectString;
        }

        #endregion
    }
}
