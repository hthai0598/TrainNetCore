using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OtrafyAPI.ViewModel
{
    public class CompanyViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; }
        public int MaxNumberBuyersAllowed { get; set; }
        public int MaxNumberSuppliersAllowed { get; set; }
        public int MaxNumberFormsAllowed { get; set; }
    }
}
