using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OtrafyAPI.ViewModel
{
    public class TagCompanyViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
