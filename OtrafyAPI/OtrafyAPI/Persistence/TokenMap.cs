using MongoDB.Bson.Serialization;
using DAL.Models;
using MongoDB.Bson.Serialization.IdGenerators;

namespace OtrafyAPI.Persistence
{
    public class TokenMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Token>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.ValidUntil).SetIsRequired(true);
                map.MapMember(x => x.CreatedDate).SetIsRequired(true);
                map.MapMember(x => x.CreatedBy).SetIsRequired(true);
                map.MapMember(x => x.IssuerName);
                map.MapMember(x => x.IssuerEmail).SetIsRequired(true);
                map.MapMember(x => x.IssueType).SetIsRequired(true);
                map.MapMember(x => x.IssuerSubject);
                map.MapMember(x => x.ActivatedDate);
                map.MapMember(x => x.RetryCount);
                map.MapMember(x => x.SentDate);                
            });
        }
    }
}
