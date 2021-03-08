using System;

namespace DAL.Models
{
    public class Comments
    {
        public Comments()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}