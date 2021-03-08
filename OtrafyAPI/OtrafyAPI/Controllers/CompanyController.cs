using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using OtrafyAPI.ViewModel;
using System;
using System.Threading.Tasks;
using DAL;
using OtrafyAPI.Helpers;
using System.Net;
using DAL.Core;
using BLL.Services.Interfaces;
using BLL.Models;
using System.ComponentModel.DataAnnotations;
using BLL.Helpers;

namespace OtrafyAPI.Controllers
{

    [AuthorizeRolesAttribute(Role.administrator)]
    [Produces("application/json")]
    [Route("api/company")]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompanyServices _companyServices;

        public CompanyController(IUnitOfWork unitOfWork, ICompanyServices companyServices)
        {
            _unitOfWork = unitOfWork;
            _companyServices = companyServices;
        }

        /// <summary>
        /// Get all company by user created (search, paging, order) (role administrator)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// - **pageNumber**: number of page
        /// - **pageSize**: number of records per page
        /// - **SortType**: 0 = Ascending ; 1 = Descending
        /// - **SortOn**: is the name of field
        /// - **FilterBy**: name of field for filter
        /// - **FilterValue**: value of field for filter      
        /// ex: /api/company?pageSize=10&#38;pageNumber=0&#38;SortType=0&#38;SortOn=DateCreated
        /// ex: /api/company?SortType=0&#38;SortOn=DateCreated&#38;pageNumber=0&#38;pageSize=10&#38;FilterBy=Name&#38;FilterValue=Công ty ABC&#38;FilterBy=address&#38;FilterValue=Toàn
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<PagedResult<CompanyResponseModel>> GetAllCompanyByUser(GetFilterParams model)
        {
            try
            {
                var response = _companyServices.GetAllCompanyByUser(Utilities.GetUserId(this.User), model);
                return new ResponseResult(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get company by Id (role administrator)
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        [HttpGet("{companyid}")]
        public async Task<ActionResult<CompanyResponseModel>> Get(string companyid)
        {
            try
            {
                var company = await _companyServices.GetCompanyDetailByUserCreated(Utilities.GetUserId(this.User), companyid);
                if (company == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Company not found, check company Id");
                }
                return new ResponseResult(HttpStatusCode.OK, company);
            }
            catch (Exception ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Create Company (role administrator)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CompanyViewModel item)
        {
            if (ModelState.IsValid == false)
            {
                return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                var result = await _companyServices.AddCompany(new Company
                {
                    Name = item.Name,
                    Avatar = item.Avatar,
                    Address = item.Address,
                    Email = item.Email,
                    Phone = item.Phone,
                    Website = item.Website,
                    IsActive = item.IsActive,
                    MaxNumberBuyersAllowed = item.MaxNumberBuyersAllowed,
                    MaxNumberFormsAllowed = item.MaxNumberFormsAllowed,
                    MaxNumberSuppliersAllowed = item.MaxNumberSuppliersAllowed,
                    DateCreated = DateTime.Now,
                    UserCreated = Utilities.GetUserId(this.User)
                });
                if (result.Success)
                {
                    return new ResponseResult(HttpStatusCode.OK, result.Item);
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Error");
            }
            catch (Exception ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }

        }

        /// <summary>
        /// Update company by id (role administrator)
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{companyid}")]
        public async Task<ActionResult> Put(string companyid, [FromBody]CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var getcompanyid = await _unitOfWork.companyRepository.GetSingleAsync(x => x.Id == new Guid(companyid));
                    if (getcompanyid == null)
                    {
                        return new ResponseResult(HttpStatusCode.BadRequest, "Company not found");
                    }

                    var result = await _companyServices.UpdateCompany(companyid, new Company
                    {
                        Name = model.Name,
                        Avatar = model.Avatar,
                        Address = model.Address,
                        Email = model.Email,
                        Phone = model.Phone,
                        Website = model.Website,
                        IsActive = model.IsActive,
                        MaxNumberBuyersAllowed = model.MaxNumberBuyersAllowed,
                        MaxNumberSuppliersAllowed = model.MaxNumberSuppliersAllowed,
                        MaxNumberFormsAllowed = model.MaxNumberFormsAllowed
                    });
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, result.Item);
                    }
                    return new ResponseResult(HttpStatusCode.NotFound, "Error");
                }
                catch (Exception ex)
                {
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Delete Company by Id (role administrator)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var getcompanyid = await _unitOfWork.companyRepository.GetSingleAsync(x => x.Id == new Guid(id));
                if (getcompanyid == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Company not found");
                }

                bool result = await _companyServices.DeleteCompany(id);
                if (result)
                {
                    return new ResponseResult(HttpStatusCode.OK, "Delete successfully");
                }
                return new ResponseResult(HttpStatusCode.BadRequest, "Error");
            }
            catch (Exception ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get buyer detail (role administrator) 
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// </remarks>
        /// <param name="buyerId"></param>
        /// <returns></returns>
        [HttpGet("get-buyer-detail")]
        [AuthorizeRolesAttribute(Role.administrator)]
        public ActionResult<BuyerDetailResponseModel> GetBuyerDetailById([Required]string buyerId)
        {
            try
            {
                var result = _companyServices.GetBuyerDetailById(buyerId);
                if (result != null)
                {
                    return new ResponseResult(HttpStatusCode.OK, result);
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Buyer not found");
            }
            catch (Exception ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Update buyer profile by Id (role: administrator)
        /// </summary>
        /// <param name="buyerId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("update-buyer/{buyerId}")]
        [AuthorizeRolesAttribute(Role.administrator)]
        public async Task<IActionResult> UpdateSupplierProfile(string buyerId, [FromBody]BuyerParams param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Email == param.Email || x.Username == param.Username);
                    if (user == null)
                    {
                        var result = await _companyServices.UpdateBuyerProfileById(buyerId, param);
                        if (result.Success)
                        {
                            return new ResponseResult(HttpStatusCode.OK, "Update succesfull");
                        }
                        return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                    }
                    else
                    {
                        return new ResponseResult(HttpStatusCode.BadRequest, "User already exists in the system");
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

    }
}
