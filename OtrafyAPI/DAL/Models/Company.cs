using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Company
    {
        public Company()
        {
            Id = Guid.NewGuid();
            Tags = new List<string>();
            FormDesigner = new List<FormDesigner>();

        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; }
        public int MaxNumberBuyersAllowed { get; set; }
        public int MaxNumberSuppliersAllowed { get; set; }
        public int MaxNumberFormsAllowed { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserCreated { get; set; }
        public List<string> Tags { get; set; }
        public List<FormDesigner> FormDesigner { get; set; }
    }
}
 
