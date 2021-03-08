using DAL.Core;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class FormRequest
    {
        public FormRequest()
        {
            Id = Guid.NewGuid();
            FormRequestData = new List<FormRequestData>();
            Comments = new List<Comments>();
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ProductId { get; set; }
        public List<FormRequestData> FormRequestData { get; set; }       
        public string Message { get; set; }
        public RequestStatus Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<Comments> Comments { get; set; }
    }
}