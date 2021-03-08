using DAL.Core;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Buyers
    {
        public Buyers()
        {
            Id = Guid.NewGuid();
            Permission = new List<BuyerPermission>();
        }
        public Guid Id { get; set; }
        public List<BuyerPermission> Permission { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string InviteToken { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
