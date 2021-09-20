using System;
namespace DAL.Model
{
    public class Entity
    {
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày sửa đổi gần nhất
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Người sửa đổi gần nhất
        /// </summary>
        //public string ModifiedBy { get; set; }
        ///// <summary>
        ///// Trạng thái
        ///// </summary>
        //public Enumeration.EntityState EntityState { get; set; }
    }
}
