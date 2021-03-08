using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class FormRequestData
    {
        public Guid FormId { get; set; }
        public int Version { get; set; }
        public string SurveyResult { get; set; }
    }
}
