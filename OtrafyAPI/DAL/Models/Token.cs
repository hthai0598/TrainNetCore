using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using DAL.Core;
namespace DAL.Models
{
    public class Token
    {
        public Token()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public DateTime ValidUntil { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string IssuerName { get; set; }
        public string IssuerEmail { get; set; }
        public IssuerType IssueType { get; set; }
        public string IssuerSubject { get; set; }
        public DateTime ActivatedDate { get; set; }
        public int RetryCount { get; set; }
        public DateTime SentDate { get; set; }
    }
}
