using System;
namespace DAL.Model
{
    public class D_CustomerGroup : Entity
    {
        public int Id { get; set; }
        public string CustomerGroupCode { get; set; }
        public string CustomerGroupName { get; set; }
        public int? ParentID { get; set; }
        public string Description { get; set; }
    }
}
