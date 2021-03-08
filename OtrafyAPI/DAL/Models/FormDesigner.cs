using DAL.Core;
using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class FormDesigner
    {
        public FormDesigner()
        {
            Id = Guid.NewGuid();
            FormDesignerData = new List<FormDesignerData>();
        }
        public Guid Id { get; set; }
        public FormType FormType { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public List<FormDesignerData> FormDesignerData { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CurrentVersion { get; set; } = 0;
    }
}
