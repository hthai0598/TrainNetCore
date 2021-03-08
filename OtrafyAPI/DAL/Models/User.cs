using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Linq;
using MongoDB.Entities;
using MongoDB.Driver.Linq;

namespace DAL.Models
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }        
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsEnabled { get; set; }
        public string Role { get; set; }
        public string JWTToken { get; set; }
        public UserProfiles UserProfiles { get; set; }
        public UserCompanyProfiles CompanyProfiles { get; set; }

    }
}