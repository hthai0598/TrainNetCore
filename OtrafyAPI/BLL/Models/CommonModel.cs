using DAL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using DAL.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace BLL.Models
{
    public class StatusResponse
    {
        public StatusResponse()
        {

        }
        public StatusResponse(int _code, string _mes)
        {
            this.StatusCode = _code;
            this.Message = _mes;
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class StatusInvalidParamsResponse : StatusResponse
    {
        public StatusInvalidParamsResponse() { }
        public StatusInvalidParamsResponse(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary _modelState, string _message = "Params Invalid")
        {
            Dictionary<string, List<string>> errorList = _modelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => (string.IsNullOrWhiteSpace(e.ErrorMessage) || string.IsNullOrEmpty(e.ErrorMessage)) ? e.Exception.InnerException.Message : e.ErrorMessage).ToList()
                );

            this.StatusCode = (int)HttpStatusCode.BadRequest;
            this.Message = _message;
            this.Description = errorList;
        }
        public Dictionary<string, List<string>> Description { get; set; }
    }
    public class CommitResult
    {
        public bool Success { get; set; }
        public object Item { get; }
        public string Message { get; set; }
        public CommitResult(bool success = false, object item = null)
        {
            Success = success;
            Item = item;
        }
        public CommitResult SetMessage(string message)
        {
            this.Message = message;
            return this;
        }

    }

    public class Statistical
    {
        public int TotalSuppliers { get; set; } = 0;
        public int PendingRequest { get; set; } = 0;
        public int TotalForm { get; set; } = 0;
    }

    public class RegisterParams
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool IsEmailConfirmed { get; set; } = false;
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsEnabled { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string JobTitle { get; set; }
    }

    public class GetFilterParams
    {
        public int? pageSize { get; set; }

        [Range(0, int.MaxValue)]
        public int? pageNumber { get; set; }

        [DisplayName("Sort Type")]
        [Range(0, 1)]
        public SortType? SortType { get; set; }
        public string SortOn { get; set; }

        [DisplayName("Filter By")]
        public List<string> FilterBy { get; set; }

        [DisplayName("Filter Value")]
        public List<List<string>> FilterValue { get; set; }
    }


    public class CompanyResponseModel
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; }
        public int MaxNumberBuyersAllowed { get; set; } = 0;
        public int TotalBuyersCreated { get; set; } = 0;
        public int MaxNumberSuppliersAllowed { get; set; } = 0;
        public int TotalSuppliersInvited { get; set; } = 0;
        public int MaxNumberFormsAllowed { get; set; } = 0;
        public int TotalFormsCreated { get; set; } = 0;
        public DateTime DateCreated { get; set; }
    }
    public class ActiveInviteParam
    {
        [Required]
        public string TokenId { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
    public class UserResponseModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsEnabled { get; set; }
        public string Role { get; set; }
        public string CompanyId { get; set; }
        public UserProfiles UserProfiles { get; set; }
        public UserCompanyProfiles CompanyProfiles { get; set; }
    }

    public class InviteBuyerParams
    {
        [Required]
        public string CompanyId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string JobTitle { get; set; }

        public List<BuyerPermission> Permission { get; set; }
        public string Message { get; set; }
    }

    public class InviteSupplierParams
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<string> Tags;
        public bool isSendEmailInvitation { get; set; } = false;
    }

    public class NewRequestParams
    {
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public List<string> Froms { get; set; }
        [Required]
        public string Message { get; set; }
    }
    public class BuyersResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<BuyerPermission> Permission { get; set; }
        public string JobTitle { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class BuyerDetailResponseModel
    {
        public string BuyerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<BuyerPermission> Permission { get; set; }
        public string JobTitle { get; set; }
        public string Message { get; set; }
    }
    public class UserProfilesParam
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string ProductType { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }

    public class TokenResponseModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
        public List<BuyerPermission> Permissions { get; set; }
        public string CompanyId { get; set; }
        //public string BuyerId { get; set; }
        //public string SupplierId { get; set; }
        public DateTime TokenExpiration { get; set; }
    }

    public class SupplierResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public int Products { get; set; }
        public List<string> Tags { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FormName { get; set; }
        public string Status { get; set; }
        public List<ListSupplierRequestResponse> FormRequest { get; set; }
    }

    public class ProductResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<string> Tags { get; set; }
        public int Grade { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class RequestFormResponModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public RequestStatus Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FullName { get; set; }
        public List<string> Tags { get; set; }
    }

    public class ListSupplierResponseModel
    {
        public string Id { get; set; }
        public string SupplierEmail { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get; set; }
    }

    public class ListProductResponseModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
    }

    public class ListFormResponseModel
    {
        public Guid Id { get; set; }
        public string FormName { get; set; }
    }
    public class ListBuyerResponseModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
    }
    public class ProductParam
    {
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public List<string> Tags { get; set; }
        public string Description { get; set; }
        [Required]
        public int Grade { get; set; } = 0;
    }
    public class ProductEdit
    {
        public int Grade { get; set; }
        public List<string> Tags { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class FormResponseModel
    {
        public Guid Id { get; set; }
        public FormType FormType { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public List<FormDesignerData> FormDesignerData { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CurrentVersion { get; set; } = 0;

    }
    public class FormParam
    {
        public FormType FormType { get; set; }
        [Required, StringLength(1000)]
        public string Name { get; set; }
        [Required]
        public List<string> Tags { get; set; }
        public string Description { get; set; }
        public List<object> SurveyDesigner { get; set; }
    }

    public class NewRequestParam
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public List<string> Form { get; set; }
        public string Message { get; set; }
    }
    public class ListSupplierRequestResponse
    {
        public string Title { get; set; }
        public RequestStatus Status { get; set; }
        public List<FormDesignerResponse> FormDesignerResponse { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class FormDesignerResponse
    {
        public Guid FormId { get; set; }
        public int Version { get; set; }
        public string FormName { get; set; }
    }

    public class SupplierDetailResponseModel
    {
        public string SupplierId { get; set; }
        public string Email { get; set; }
        public List<string> Tags { get; set; }
        public UserProfiles UserProfiles { get; set; }
        public UserCompanyProfiles CompanyProfiles { get; set; }
    }

    public class FormRequestResponseModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateUpdated { get; set; }
        public string BuyerInCharge { get; set; }
        public string CompanyName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
    }
    public class FormRequestDetailResponseModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public IEnumerable<Products> Products { get; set; }
        public List<FormRequestDataDetail> FormRequestDataDetail { get; set; }
        public List<Comments> Comments { get; set; }
    }
    
    public class FormRequestDataDetail
    {
        public Guid FormId { get; set; }
        public int Version { get; set; }
        public string SurveyResult { get; set; }
        public string SurveyDesigner { get; set; }
    }

    public class SupplierProfilesParam
    {
        public string Email { get; set; }
        public List<string> Tags { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }

    public class FormRequestParam
    {
        public List<FormRequestDataParam> FormRequestDataParam { get; set; }
    }

    public class FormRequestDataParam
    {
        public string FormId { get; set; }
        public List<object> SurveyResult { get; set; }
    }

    public class FormRequestCommentParam
    {
        public string Comments { get; set; }
    }

    public class BuyerParams
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string JobTitle { get; set; }

        public List<BuyerPermission> Permission { get; set; }
    }

    public class CommentsResponseModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CommentsSortByParam
    {
        [DisplayName("Sort Type")]
        [Range(0, 1)]
        public SortType? SortType { get; set; }
    }

    public class FormDetailResponseModel
    {
        public Guid Id { get; set; }
        public FormType FormType { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public List<FormDesignerData> FormDesignerData { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CurrentVersion { get; set; } = 0;
    }
    public class ResendFormRequestParam
    {
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public string FormRequestId { get; set; }
    }

    public class UploadFileParam
    {
        public string CompanyId { get; set; }
        public List<IFormFile> Files { get; set; }
    }

    public class FormRequestStatusParam
    {
        /// <summary>
        /// 4: approve; 5: reject;
        /// </summary>
        [Range(4, 5)]
        public int Status { get; set; }
    }
    public class ResponseModel
    {
        public ResponseModel()
        {
            this.Message = "OK";
        }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public void SetErrorMessage(string message) 
        {
            this.IsSuccess = false;
            this.Message = message;
        }
        public void SetSuccessMessage(string message)
        {
            this.IsSuccess = true;
            this.Message = message;
        }
    }
}