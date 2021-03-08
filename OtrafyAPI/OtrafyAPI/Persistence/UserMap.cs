using MongoDB.Bson.Serialization;
using DAL.Models;
using MongoDB.Bson.Serialization.IdGenerators;

namespace OtrafyAPI.Persistence
{
    public class UserMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.UserProfiles);
                map.MapMember(x => x.CompanyProfiles);
                map.MapMember(x => x.Username).SetIsRequired(true);
                map.MapMember(x => x.Email).SetIsRequired(true);
                map.MapMember(x => x.IsEmailConfirmed).SetIsRequired(true).SetDefaultValue(false);
                map.MapMember(x => x.PasswordHash).SetIsRequired(true);
                map.MapMember(x => x.PasswordSalt).SetIsRequired(true);
                map.MapMember(x => x.IsEnabled).SetIsRequired(true);
                map.MapMember(x => x.Role).SetIsRequired(true);
                map.MapMember(x => x.JWTToken);
            });
        }
    }
}
