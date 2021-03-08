using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OtrafyAPI.ViewModel
{
    public class ValidTokenViewModel
    {
        [Required]
        public string TokenId { get; set; }
    }
}
