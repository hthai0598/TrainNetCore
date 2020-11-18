using System;
using System.Collections.Generic;

namespace BH_SocialNetwork_Models.Modelss
{
    public partial class Tag
    {
        /// <summary>
        /// Mã tag
        /// </summary>
        public Guid TagId { get; set; }
        /// <summary>
        /// Tên tag
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// Mã bài có tag đó
        /// </summary>
        public Guid? ArticlesId { get; set; }

        public Articles Articles { get; set; }
    }
}
