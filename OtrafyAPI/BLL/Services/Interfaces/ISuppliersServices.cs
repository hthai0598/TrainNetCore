using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using BLL.Models;
using BLL.Helpers;

namespace BLL.Services.Interfaces
{
    public interface ISuppliersServices
    {
        Task<CommitResult> CreateSupplier(Suppliers item);
        PagedResult<SupplierResponseModel> GetAllSupplierOfBuyer(string CompanyId, string BuyerId, GetFilterParams model);

        List<ListSupplierResponseModel> GetSupplierCompanyName(string CompanyId, string BuyerId, string name);

        Task<CommitResult> RemoveTags(string supplierId, string tagsName);
        Task<CommitResult> CreateProduct(string supplierId, Products item);
        FormRequestDetailResponseModel GetFormRequestDetailById(string supplierId, Guid formRequestId);
        PagedResult<ProductResponseModel> GetAllProductBySuppliers(string supplierId, GetFilterParams model);
        Task<CommitResult> UpdateFormRequest(string supplierId, string formrequestId, FormRequestParam model);
        Task<CommitResult> AddCommentToFormRequest(string current_user, string supplierId, string formrequestId, FormRequestCommentParam param);
        List<CommentsResponseModel> GetAllCommentOfFormRequestByID(string supplierId, string formrequestId, CommentsSortByParam param);
        ProductResponseModel GetProductDetail(string productId);
        Task<CommitResult> EditProductbyId(string id, Products product);
        PagedResult<RequestFormResponModel> GetRequestFormById(string id, GetFilterParams model);
        Task<CommitResult> UpdateCompleteFormRequest(string supplierId, string formrequestId);
    }
}
