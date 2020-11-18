using System;
using System.Collections.Generic;
using System.Text;

namespace BH_SocialNetwork_Models.Comman
{
    public class Filter
    {
        #region Property
        /// <summary>
        /// Trường Filter
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Giá trị Filter
        /// </summary>
        public string FilterValue { get; set; }
        #endregion
    }
}
