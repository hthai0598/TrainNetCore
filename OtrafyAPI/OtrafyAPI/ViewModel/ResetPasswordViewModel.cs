using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OtrafyAPI.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string TokenId { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
