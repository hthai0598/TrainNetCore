using MongoDB.Bson.Serialization;
using DAL.Models;
using MongoDB.Bson.Serialization.IdGenerators;

namespace OtrafyAPI.Persistence
{
    public class CompanyMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Company>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Name).SetIsRequired(true);
                map.MapMember(x => x.Address);
                map.MapMember(x => x.Email);
                map.MapMember(x => x.Phone);
                map.MapMember(x => x.Website);
                map.MapMember(x => x.IsActive).SetIsRequired(true).SetDefaultValue(true);
                map.MapMember(x => x.MaxNumberBuyersAllowed).SetIsRequired(true);
                map.MapMember(x => x.MaxNumberSuppliersAllowed).SetIsRequired(true);
                map.MapMember(x => x.MaxNumberFormsAllowed).SetIsRequired(true);
                map.MapMember(x => x.DateCreated).SetIsRequired(true);
                map.MapMember(x => x.UserCreated).SetIsRequired(true);
                map.MapMember(x => x.Tags);
                map.MapMember(x => x.FormDesigner);
            });
        }
    }
}