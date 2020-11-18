using System;
using System.Collections.Generic;
using System.Text;

namespace BH_SocialNetwork_Models
{
    public class Result
    {
        #region Property
        /// <summary>
        /// Thành công
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Thông điệp
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Dữ liệu
        /// </summary>
        public object Data { get; set; }
        #endregion
    }
}
