using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace DAL.Models
{
    public class RefreshJWTToken
    {
        public RefreshJWTToken()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}
