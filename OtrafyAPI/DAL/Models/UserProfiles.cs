using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MongoDB.Entities;
using MongoDB.Driver.Linq;

namespace DAL.Models
{
    public class UserProfiles
    {
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string JobTitle  { get; set; }
        public string Message { get; set; }
    }
}
