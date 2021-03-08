using System.ComponentModel.DataAnnotations;

namespace OtrafyAPI.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
