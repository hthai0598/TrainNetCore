using System;
namespace DAL.Model
{
    public class D_Customer : Entity
    {
        public Guid Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerGroupID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CompanyName { get; set; }
        public int? Gender { get; set; }
        public string TaxCode { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string MemberCardCode { get; set; }
        public int? CardRank { get; set; }
        public double DebitAmount { get; set; }
        public string Note { get; set; }
        public bool? IsMember5Food { get; set; }
        public bool? StopFollow { get; set; }
    }
}
