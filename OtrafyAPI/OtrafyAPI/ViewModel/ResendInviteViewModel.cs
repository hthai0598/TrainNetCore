using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OtrafyAPI.ViewModel
{
    public class ResendInviteViewModel
    {
        [Required]
        public string BuyerId { get; set; }
    }
}
