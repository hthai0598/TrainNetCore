using System;
using System.Collections.Generic;

namespace BH_SocialNetwork_Models.Modelss
{
    public partial class Comment
    {
        /// <summary>
        /// Mã comment
        /// </summary>
        public Guid CommentId { get; set; }
        /// <summary>
        /// Nội dung comment
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreateAt { get; set; }
        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? UpdateAt { get; set; }
        /// <summary>
        /// Người viết
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// Bài viết comment
        /// </summary>
        public Guid? ArticlesId { get; set; }

        public Articles Articles { get; set; }
        public Users User { get; set; }
    }
}
