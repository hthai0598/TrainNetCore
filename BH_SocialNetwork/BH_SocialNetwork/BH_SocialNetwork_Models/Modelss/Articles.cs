using System;
using System.Collections.Generic;

namespace BH_SocialNetwork_Models.Modelss
{
    public partial class Articles
    {
        public Articles()
        {
            Comment = new HashSet<Comment>();
            Favotired = new HashSet<Favotired>();
            Tag = new HashSet<Tag>();
        }
        /// <summary>
        /// Mã bài viết
        /// </summary>
        public Guid ArticlesId { get; set; }
        /// <summary>
        /// Tiêu đề bài viết
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Mo tả bài viêt
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Nội dung bài viết
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Thích
        /// </summary>
        public int? FavotiredCount { get; set; }
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
        public string Tags { get; set; }

        public Users User { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public ICollection<Favotired> Favotired { get; set; }
        public ICollection<Tag> Tag { get; set; }
    }
}
