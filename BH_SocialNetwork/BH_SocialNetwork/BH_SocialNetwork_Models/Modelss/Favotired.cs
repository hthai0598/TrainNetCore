using System;
using System.Collections.Generic;

namespace BH_SocialNetwork_Models.Modelss
{
    public partial class Favotired
    {
        /// <summary>
        /// Mã
        /// </summary>
        public Guid FavotiredId { get; set; }
        /// <summary>
        /// Trạng thái thích
        /// </summary>
        public bool? Favotired1 { get; set; }
        /// <summary>
        /// Mã bài thích
        /// </summary>
        public Guid? ArticlesId { get; set; }
        /// <summary>
        /// Mã người thích
        /// </summary>
        public Guid? UserId { get; set; }

        public Articles Articles { get; set; }
        public Users User { get; set; }
    }
}
