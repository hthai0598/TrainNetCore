using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OtrafyAPI.ViewModel
{
    public class RefreshTokenViewModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
