using DAL.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class FormRequestTimeline
    {
        public FormRequestTimeline()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string FormRequestId { get; set; }
        public RequestStatus Status { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
