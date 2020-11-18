using System;
using System.Collections.Generic;

namespace BH_SocialNetwork_Models.Modelss
{
    public partial class Users
    {
        public Users()
        {
            Articles = new HashSet<Articles>();
            Comment = new HashSet<Comment>();
            Favotired = new HashSet<Favotired>();
            UsersFollowingUser = new HashSet<UsersFollowing>();
            UsersFollowingUserFollowNavigation = new HashSet<UsersFollowing>();
        }
        /// <summary>
        /// Mã User
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Link ánh
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreateAt { get; set; }
        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? UpdateAt { get; set; }

        public ICollection<Articles> Articles { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public ICollection<Favotired> Favotired { get; set; }
        public ICollection<UsersFollowing> UsersFollowingUser { get; set; }
        public ICollection<UsersFollowing> UsersFollowingUserFollowNavigation { get; set; }
    }
}
