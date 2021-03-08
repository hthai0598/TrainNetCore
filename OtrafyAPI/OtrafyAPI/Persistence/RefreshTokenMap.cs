using MongoDB.Bson.Serialization;
using DAL.Models;
using MongoDB.Bson.Serialization.IdGenerators;

namespace OtrafyAPI.Persistence
{
    public class RefreshTokenMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<RefreshJWTToken>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.IssuedUtc).SetIsRequired(true);
                map.MapMember(x => x.ExpiresUtc).SetIsRequired(true);
                map.MapMember(x => x.Token).SetIsRequired(true);
                map.MapMember(x => x.UserId).SetIsRequired(true);
            });
        }
    }
}