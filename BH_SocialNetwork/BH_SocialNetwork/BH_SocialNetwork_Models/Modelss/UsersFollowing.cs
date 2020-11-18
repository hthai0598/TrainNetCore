using System;
using System.Collections.Generic;

namespace BH_SocialNetwork_Models.Modelss
{
    public partial class UsersFollowing
    {
        /// <summary>
        /// Mã
        /// </summary>
        public Guid UserFollowId { get; set; }
        /// <summary>
        /// NGười được follow
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// Người follow
        /// </summary>
        public Guid? UserFollow { get; set; }
        /// <summary>
        /// Trạng thái follow
        /// </summary>
        public bool? State { get; set; }

        public Users User { get; set; }
        public Users UserFollowNavigation { get; set; }
    }
}
