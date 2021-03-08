using MongoDB.Bson.Serialization;
using DAL.Models;
using MongoDB.Bson.Serialization.IdGenerators;

namespace OtrafyAPI.Persistence
{
    public class SuppliersMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Suppliers>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.UserId).SetIsRequired(true);
                map.MapMember(x => x.CompanyId).SetIsRequired(true);
                map.MapMember(x => x.InviteToken).SetIsRequired(true);
                map.MapMember(x => x.CreatedBy).SetIsRequired(true);
                map.MapMember(x => x.CreatedDate).SetIsRequired(true);
                map.MapMember(x => x.IsActive).SetIsRequired(true).SetDefaultValue(true);
                map.MapMember(x => x.Tags);
                map.MapMember(x => x.Products);
                map.MapMember(x => x.FormRequest);
            });
        }
    }
}
