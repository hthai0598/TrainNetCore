using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Suppliers
    {
        public Suppliers()
        {
            Id = Guid.NewGuid();
            Tags = new List<string>();
            Products = new List<Products>();
            FormRequest = new List<FormRequest>();
        }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string BuyerId { get; set; }
        public string InviteToken { get; set; }
        public bool IsActive { get; set; }

        public List<string> Tags;
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Products> Products { get; set; }
        public List<FormRequest> FormRequest { get; set; }
        public List<FormRequestTimeline> FormRequestTimeline { get; set; }
    }
}
