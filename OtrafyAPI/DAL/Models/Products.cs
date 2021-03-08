using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class Products
    {
        public Products()
        {
            Id = Guid.NewGuid();
            Tags = new List<string>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<string> Tags { get; set; }
        public int Grade { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
