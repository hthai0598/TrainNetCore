using System;
using System.Threading.Tasks;
using DAL.Models;
using BLL.Models;
using BLL.Helpers;

namespace BLL.Services.Interfaces
{
    public interface ICompanyServices
    {
        Task<CommitResult> AddCompany(Company item);
        Task<CompanyResponseModel> GetCompanyDetailByUserCreated(string UserId, string CompanyId);
        PagedResult<CompanyResponseModel> GetAllCompanyByUser(string UserId, GetFilterParams model);
        Task<CommitResult> UpdateCompany(string id, Company model);
        Task<Company> GetCompanyById(Guid id);
        Task<bool> DeleteCompany(string CompanyId);

        BuyerDetailResponseModel GetBuyerDetailById(string buyerId);
        Task<CommitResult> UpdateBuyerProfileById(string buyerId, BuyerParams param);
    }
}
