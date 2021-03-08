using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using BLL.Models;
using BLL.Helpers;

namespace BLL.Services.Interfaces
{
    public interface IBuyersServices
    {
        Task<CommitResult> CreateBuyers(Buyers item);
        PagedResult<BuyersResponseModel> GetAllBuyerOfCompany(string CompanyId, GetFilterParams model);
        Task<bool> ResendInviteBuyer(string current_userid, Buyers buyer);
        Task<bool> DeleteBuyer(string BuyerId);
        Task<CommitResult> CreateFormsDesigner(string companyId, FormDesigner item);
        Task<CommitResult> UpdateTags(string companyId, List<string> model);
        List<string> FilterTags(string companyId, string name);
        List<ListProductResponseModel> GetListProductOfSupplierById(Guid supplierId);
        List<ListFormResponseModel> GetListFormsOfCompanyById(Guid companyId, string formname);
        Task<CommitResult> CreateNewRequest(string supplierId, FormRequest model);
        SupplierDetailResponseModel GetSupplierDetailById(Guid supplierId);
        Statistical Statistical(string buyerId, string companyId);
        PagedResult<FormRequestResponseModel> GetAllFormRequestBySupplierId(string supplierId, GetFilterParams model);
        PagedResult<FormResponseModel> GetAllForms(string companyId, GetFilterParams model);
        Task<CommitResult> UpdateSupplierProfile(string supplierId, SupplierProfilesParam model);
        Task<CommitResult> UpdateForm(string companyId, string currentUserId, string formId, FormParam model);
        FormDetailResponseModel GetFormDetailById(string companyId, Guid formId);
        Task<bool> ResendFormRequestToSupplier(string current_userid, ResendFormRequestParam param);
        Task<CommitResult> UpdateStatusFormRequest(string current_userid, string supplierId, string formrequestId, FormRequestStatusParam param);
        Task<bool> DeleteFormRequest(string supplierId, string formRequestId);
        List<ListBuyerResponseModel> GetListBuyerInContact(Guid companyId, string fullname);
        Task<bool> DeleteForm(string companyId, string formId);
    }
}
