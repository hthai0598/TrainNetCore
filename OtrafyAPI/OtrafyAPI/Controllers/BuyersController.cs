using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using OtrafyAPI.ViewModel;
using System;
using System.Threading.Tasks;
using DAL;
using OtrafyAPI.Helpers;
using System.Net;
using System.Linq;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using DAL.Core;
using System.ComponentModel.DataAnnotations;
using BLL.Services.Interfaces;
using BLL.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using BLL.Helpers;

namespace OtrafyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/buyers")]

    public class BuyersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendGridSender _sendGridSender;
        private readonly IEmailTemplates _emailTemplates;
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
        private readonly IBuyersServices _buyersServices;
        private readonly IAccountServices _accountServices;
        private readonly ISuppliersServices _supplierServices;
        private readonly ILogger _logger;

        public BuyersController(IUnitOfWork unitOfWork, ISendGridSender sendGridSender, IEmailTemplates emailTemplates, IConfiguration configuration, ITokenServices tokenServices, IBuyersServices buyersServices, IAccountServices accountServices, ISuppliersServices supplierServices, ILogger<BuyersController> logger)
        {
            _unitOfWork = unitOfWork;
            _sendGridSender = sendGridSender;
            _emailTemplates = emailTemplates;
            _configuration = configuration;
            _tokenServices = tokenServices;
            _buyersServices = buyersServices;
            _accountServices = accountServices;
            _supplierServices = supplierServices;
            _logger = logger;
        }

        /// <summary>
        /// Create buyer (role administrator)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// - **Permission**: 0: Run report; 1: View all supplier; 2: Create form template; 3: Create new supplier
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create-buyer")]
        [AuthorizeRolesAttribute(Role.administrator)]
        public async Task<ActionResult> CreateBuyer([FromBody] InviteBuyerParams value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var curent_user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Id == new Guid(Utilities.GetUserId(this.User)));
                    var company = await _unitOfWork.companyRepository.GetById(new Guid(value.CompanyId));
                    if (company == null)
                    {
                        return new ResponseResult(HttpStatusCode.NotFound, "Company not found");
                    }
                    var allbuyer = await _unitOfWork.buyersRepository.GetListAsync(x => x.CompanyId == value.CompanyId);
                    if (allbuyer.Count() >= company.MaxNumberBuyersAllowed)
                    {
                        return new ResponseResult(HttpStatusCode.BadRequest, "Maximum number of buyers allowed:" + company.MaxNumberBuyersAllowed);
                    }
                    //check user exist
                    var user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Email == value.Email || x.Username == value.Username);
                    if (user == null)
                    {
                        byte[] passwordHash, passwordSalt;
                        Utilities.CreatePasswordHash(Utilities.GenerateRandomPassword(), out passwordHash, out passwordSalt);

                        //Create user
                        user = await _accountServices.CreateUser(new User
                        {
                            Username = value.Username,
                            Email = value.Email,
                            IsEmailConfirmed = false,
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt,
                            IsEnabled = true,
                            Role = Role.buyers.ToString(),
                            UserProfiles = new UserProfiles
                            {
                                Avatar = "default_avatar.png",
                                FirstName = value.FirstName,
                                LastName = value.LastName,
                                JobTitle = value.JobTitle,
                                Message = value.Message
                            },
                            CompanyProfiles = new UserCompanyProfiles
                            {
                                CompanyName = "",
                                Address = "",
                                Description = ""
                            }
                        });


                        //Create token
                        var token_code = _tokenServices.GenerateToken(new Token
                        {
                            ValidUntil = DateTime.UtcNow.AddHours(Int32.Parse(_configuration["TokenLifespan:InviteUser"])),
                            CreatedDate = DateTime.Now,
                            CreatedBy = Utilities.GetUserId(this.User),
                            IssuerName = value.FirstName + ' ' + value.LastName,
                            IssuerEmail = value.Email,
                            IssueType = IssuerType.InviteBuyer,
                            IssuerSubject = "Send invite buyer",
                            SentDate = DateTime.Now,
                            RetryCount = 0
                        });

                        //Create buyer
                        await _buyersServices.CreateBuyers(new Buyers
                        {
                            Permission = value.Permission,
                            UserId = user.Id.ToString(),
                            CompanyId = value.CompanyId,
                            InviteToken = token_code,
                            isActive = false,
                            CreatedBy = Utilities.GetUserId(this.User),
                            CreatedDate = DateTime.Now
                        });

                        // Send mail invite buyer
                        string invite_url = _configuration["AppSettings:Frontend_Url"] + "/invite/" + token_code;
                        string message = _emailTemplates.GetInviteBuyerEmailTemplate(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, user.Email, invite_url, curent_user.UserProfiles.FirstName + " " + curent_user.UserProfiles.LastName, company.Name);
                        await _sendGridSender.SendEmailAsync(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, user.Email, "Invite Buyer", message);
                        _logger.LogInformation("Send email invite buyer to (" + user.Email + ")");

                        return new ResponseResult(HttpStatusCode.OK, "Invite Buyers Successfully");
                    }
                    else
                    {
                        return new ResponseResult(HttpStatusCode.BadRequest, "User already exists in the system");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API CreateBuyer: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Get all buyer of company (search, paging, order)  (role administrator)
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
        /// ex: Filter/Search Multiple Fields in one textbox: /api/buyers/get-all-buyers?pageSize=10&#38;pageNumber=1&#38;CompanyId=5e04e609-f056-4d7b-87b0-aada009ad9c2&#38;FilterBy=Name&#38;FilterBy=Username&#38;FilterValue=trung&#38;FilterValue=trung001
        /// </remarks>
        /// <param name="CompanyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("get-all-buyers")]
        [AuthorizeRolesAttribute(Role.administrator, Role.buyers)]
        public ActionResult<PagedResult<BuyersResponseModel>> GetAllBuyerOfCompany([Required]string CompanyId, GetFilterParams model)
        {
            try
            {
                var company = _unitOfWork.companyRepository.GetSingle(x => x.Id == new Guid(CompanyId));
                if (company != null)
                {
                    var response = _buyersServices.GetAllBuyerOfCompany(CompanyId, model);
                    return new ResponseResult(HttpStatusCode.OK, response);
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Company not found");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetAllBuyerOfCompany: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Resend invite buyer (role administrator)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AuthorizeRolesAttribute(Role.administrator)]
        [HttpPost("resend")]
        public async Task<IActionResult> Resend([FromBody]ResendInviteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var buyer = await _unitOfWork.buyersRepository.GetById(new Guid(model.BuyerId));
                if (buyer == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Buyer not found");
                }
                if (buyer.isActive)
                {
                    return new ResponseResult(HttpStatusCode.BadRequest, "The buyer has been activated");
                }

                try
                {
                    var result = await _buyersServices.ResendInviteBuyer(Utilities.GetUserId(this.User), buyer);
                    if (result)
                    {
                        return new ResponseResult(HttpStatusCode.OK, "Resend invite buyers successfully");
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Unable to send invite");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API Resend invite buyer: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Delete Buyer by Id (role administrator)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRolesAttribute(Role.administrator)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var getcompanyid = await _unitOfWork.buyersRepository.GetSingleAsync(x => x.Id == new Guid(id));
                if (getcompanyid == null)
                {
                    return new ResponseResult(HttpStatusCode.BadRequest, "Buyer not found");
                }

                bool result = await _buyersServices.DeleteBuyer(id);
                if (result)
                {
                    return new ResponseResult(HttpStatusCode.OK, "Delete successfully");
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Unable to find buyer");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API delete buyer by id: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Create Form (role buyer)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// </remarks>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("create-form")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<ActionResult> CreateFormsDesigner([FromBody] FormParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var companyid = Utilities.GetCompanyId(this.User);
                    var company = await _unitOfWork.companyRepository.GetById(new Guid(companyid));
                    if (company == null)
                    {
                        return new ResponseResult(HttpStatusCode.NotFound, "Company not found");
                    }

                    if (company.FormDesigner.Count() >= company.MaxNumberFormsAllowed)
                    {
                        return new ResponseResult(HttpStatusCode.BadRequest, "Maximum number of form allowed:" + company.MaxNumberFormsAllowed);
                    }

                    // Create Formversion
                    var serveyJson = JsonConvert.SerializeObject(param.SurveyDesigner);
                    List<FormDesignerData> _surveyformVer = new List<FormDesignerData>();
                    _surveyformVer.Add(new FormDesignerData
                    {
                        SurveyDesigner = serveyJson,
                        Version = 1,
                        CreatedBy = Utilities.GetUserId(this.User),
                        CreatedDate = DateTime.Now
                    });
                    //Create Form
                    var result = await _buyersServices.CreateFormsDesigner(Utilities.GetCompanyId(this.User), new FormDesigner
                    {
                        FormType = param.FormType,
                        Name = param.Name,
                        Tags = param.Tags,
                        FormDesignerData = _surveyformVer,
                        Description = param.Description,
                        CreatedBy = Utilities.GetUserId(this.User),
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        UpdatedBy = Utilities.GetUserId(this.User),
                        CurrentVersion = 1
                    });

                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, result.Item);
                    }
                    return new ResponseResult(HttpStatusCode.InternalServerError, "Unable to CreateFormsDesigner");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API CreateFormsDesigner: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Delete form by Id (role buyer)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="formId"></param>
        /// <returns></returns>
        [HttpDelete("delete-form/{formId}")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<ActionResult> DeleteForm([Required]string companyId, [Required]string formId)
        {
            try
            {
                var company = await _unitOfWork.companyRepository.GetSingleAsync(x => x.Id == new Guid(companyId));
                if (company == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Company not found");
                }

                bool result = await _buyersServices.DeleteForm(companyId, formId);
                if (result)
                {
                    return new ResponseResult(HttpStatusCode.OK, "Delete successfully");
                }
                return new ResponseResult(HttpStatusCode.BadRequest, "Failed to delete this record");
            }
            catch (Exception ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get all forms of company (search, paging, order) (role buyer) 
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// </remarks>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("get-all-forms")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<PagedResult<FormResponseModel>> GetAllForms(GetFilterParams param)
        {
            try
            {
                var response = _buyersServices.GetAllForms(Utilities.GetCompanyId(this.User), param);
                return new ResponseResult(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetAllForms: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get list forms (role buyer)
        /// </summary>
        /// <returns>List drp form</returns>
        [HttpGet("list-forms")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<ListFormResponseModel> GetListForms(string formname)
        {
            try
            {
                var company = _unitOfWork.companyRepository.GetWhere(x => x.Id == new Guid(Utilities.GetCompanyId(this.User))).FirstOrDefault();
                if (company == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Company not found");
                }
                var response = _buyersServices.GetListFormsOfCompanyById(company.Id, formname);

                return new ResponseResult(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetListForms: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }
        /// <summary>
        /// Get list last buyer in contact (role buyer)
        /// </summary>
        /// <returns>List drp form</returns>
        [HttpGet("list-last-buyer")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<ListFormResponseModel> GetListBuyerInContact(string fullname)
        {
            try
            {
                var company = _unitOfWork.companyRepository.GetWhere(x => x.Id == new Guid(Utilities.GetCompanyId(this.User))).FirstOrDefault();
                if (company == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Company not found");
                }
                var response = _buyersServices.GetListBuyerInContact(company.Id, fullname);

                return new ResponseResult(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetListForms: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get form detail (role buyer) 
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// </remarks>
        /// <param name="formId"></param>
        /// <returns></returns>
        [HttpGet("get-form-detail")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<FormDetailResponseModel> GetFormDetail([Required]string formId)
        {
            try
            {
                var result = _buyersServices.GetFormDetailById(Utilities.GetCompanyId(this.User), new Guid(formId));
                if (result != null)
                {
                    return new ResponseResult(HttpStatusCode.OK, result);
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Form not found");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetFormDetail: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Update Form by Id (role: buyer)
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("update-form/{formId}")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<IActionResult> UpdateFormById([Required]string formId, [FromBody]FormParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _buyersServices.UpdateForm(Utilities.GetCompanyId(this.User), Utilities.GetUserId(this.User), formId, param);
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, result.Item);
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API UpdateFormById: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Create New Request (role buyer)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// productId: Choose supplier to get productId
        /// form: ["formId","formid"..]
        /// </remarks>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("create-form-request")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<ActionResult> CreateFormRequest([FromBody] NewRequestParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Create Formversion
                    List<FormRequestData> _formlist = new List<FormRequestData>();
                    foreach (var item in param.Form)
                    {
                        var company_collection = _unitOfWork.companyRepository.QueryAll();
                        var _list_forms = company_collection.Where(x => x.Id == new Guid(Utilities.GetCompanyId(this.User))).SelectMany(x => x.FormDesigner);
                        var forms = _list_forms.Where(x => x.Id == new Guid(item)).FirstOrDefault();
                        if (forms != null)
                        {
                            _formlist.Add(new FormRequestData
                            {
                                FormId = forms.Id,
                                Version = forms.CurrentVersion,
                                SurveyResult = ""
                            });
                        }
                        else
                        {
                            return new ResponseResult(HttpStatusCode.NotFound, "Form Id: " + item + " not exist");
                        }
                    }

                    //Create Form
                    var result = await _buyersServices.CreateNewRequest(param.SupplierId, new FormRequest
                    {
                        Title = param.Title,
                        ProductId = param.ProductId,
                        FormRequestData = _formlist,
                        Message = param.Message,
                        Status = RequestStatus.pending,
                        CreatedBy = Utilities.GetUserId(this.User),
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        UpdatedBy = Utilities.GetUserId(this.User)
                    });

                    if (result.Success)
                    {
                        var current_user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Id == new Guid(Utilities.GetUserId(this.User)));
                        // Send mail invite
                        var user_collection = _unitOfWork.accountRepository.QueryAll();
                        var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();

                        var supplier_info = (from u in user_collection.AsEnumerable()
                                             join s in supplier_collection.AsEnumerable() on u.Id.ToString() equals s.UserId
                                             where (s.Id == new Guid(param.SupplierId))
                                             select new
                                             {
                                                 UserProfiles = u.UserProfiles,
                                                 Email = u.Email
                                             }).FirstOrDefault();

                        var token_code = _tokenServices.GenerateToken(new Token
                        {
                            ValidUntil = DateTime.UtcNow.AddHours(Int32.Parse(_configuration["TokenLifespan:InviteUser"])),
                            CreatedDate = DateTime.Now,
                            CreatedBy = Utilities.GetUserId(this.User),
                            IssuerName = supplier_info.UserProfiles.FirstName + ' ' + supplier_info.UserProfiles.LastName,
                            IssuerEmail = supplier_info.Email,
                            IssueType = IssuerType.InviteNewRequest,
                            IssuerSubject = "Send invite new request",
                            SentDate = DateTime.Now,
                            RetryCount = 0
                        });
                        string message = _emailTemplates.GetSendFormRequestEmailTemplate(supplier_info.UserProfiles.FirstName + ' ' + supplier_info.UserProfiles.LastName, supplier_info.Email, param.Message, current_user.UserProfiles.FirstName + " " + current_user.UserProfiles.LastName);
                        await _sendGridSender.SendEmailAsync(supplier_info.UserProfiles.FirstName + ' ' + supplier_info.UserProfiles.LastName, supplier_info.Email, "New Request", message);
                        _logger.LogInformation("Send email form request to (" + supplier_info.Email + ")");

                        return new ResponseResult(HttpStatusCode.OK, result.Item);
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API CreateFormRequest: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Get all form request by supplier id (search, paging, order) (role buyer) 
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// Status: pending = 1; inprogress =2; completed = 3; rejected = 4; approved = 5
        /// </remarks>
        /// <param name="supplierId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("get-all-form-request-by-supplier")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<PagedResult<FormRequestResponseModel>> GetAllFormRequestBySupplierId([Required]string supplierId, GetFilterParams param)
        {
            try
            {
                var supplier = _unitOfWork.suppliersRepository.GetSingle(x => x.Id == new Guid(supplierId));
                if (supplier != null)
                {
                    var response = _buyersServices.GetAllFormRequestBySupplierId(supplierId, param);
                    return new ResponseResult(HttpStatusCode.OK, response);
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Supplier not found");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetAllFormRequestBySupplierId: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// View Filled form request detail (role buyer) 
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// </remarks>
        /// <param name="formRequestId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        [HttpGet("view-filled-form-request-detail")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<FormRequestDetailResponseModel> GetFormRequestDetail([Required]string supplierId, [Required]string formRequestId)
        {
            try
            {
                var result = _supplierServices.GetFormRequestDetailById(supplierId, new Guid(formRequestId));
                if (result != null)
                {
                    return new ResponseResult(HttpStatusCode.OK, result);
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Form request not found");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetFormRequestDetail: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Update status for form request (Approve, Reject) (role: buyer)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// API allow buyer update status form request to (status = 5 approve; status = 4 reject)
        /// </remarks>
        /// <param name="supplierId"></param>
        /// <param name="formRequestId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("update-status-form-request/{formrequestId}")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<IActionResult> UpdateStatusFormRequest([Required]string supplierId, [Required]string formRequestId, FormRequestStatusParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var current_userid = Utilities.GetUserId(this.User);
                    var result = await _buyersServices.UpdateStatusFormRequest(current_userid, supplierId, formRequestId, param);
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, result.Item);
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API UpdateStatusFormRequest: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Delete form request by Id (role buyer)
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="formRequestId"></param>
        /// <returns></returns>
        [HttpDelete("delete-form-request/{formRequestId}")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<ActionResult> DeleteFormRequest([Required]string supplierId, [Required]string formRequestId)
        {
            try
            {
                var supplier = await _unitOfWork.suppliersRepository.GetSingleAsync(x => x.Id == new Guid(supplierId));
                if (supplier == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Supplier not found");
                }

                bool result = await _buyersServices.DeleteFormRequest(supplierId, formRequestId);
                if (result)
                {
                    return new ResponseResult(HttpStatusCode.OK, "Delete successfully");
                }
                return new ResponseResult(HttpStatusCode.BadRequest, "Failed to delete this record");
            }
            catch (Exception ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Resend form request (role buyer)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AuthorizeRolesAttribute(Role.buyers)]
        [HttpPost("resend-form-request")]
        public async Task<IActionResult> ResendFormRequest([FromBody]ResendFormRequestParam model)
        {
            if (ModelState.IsValid)
            {
                var supplier = await _unitOfWork.suppliersRepository.GetById(new Guid(model.SupplierId));
                if (supplier == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Supplier not found");
                }

                try
                {
                    var result = await _buyersServices.ResendFormRequestToSupplier(Utilities.GetUserId(this.User), model);
                    if (result)
                    {
                        return new ResponseResult(HttpStatusCode.OK, "Resend form request successfully");
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Unable to send form request");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API ResendFormRequest: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Add comment to form request (role: buyer)
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="formrequestId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("add-comment-formrequest/{formrequestId}")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<IActionResult> AddCommentToFormRequest([Required]string supplierId, [Required]string formrequestId, [FromBody]FormRequestCommentParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var current_user = Utilities.GetUserId(this.User);
                    var result = await _supplierServices.AddCommentToFormRequest(current_user, supplierId, formrequestId, param);
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, result.Item);
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API AddCommentToFormRequest: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Get all comment of form request by id (role: buyer)
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="formrequestId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("get-all-comments-form-request")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public IActionResult GetAllCommentOfFormRequestByID([Required]string supplierId, [Required]string formrequestId, CommentsSortByParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _supplierServices.GetAllCommentOfFormRequestByID(supplierId, formrequestId, param);
                    return new ResponseResult(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API GetAllCommentOfFormRequestByID: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Add Tags the buyer's company (role buyer)
        /// </summary>        
        /// <remarks>
        /// ### Descriptions  ###
        /// API allow buyer update new tags the buyer's company
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("add-tag")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<ActionResult> UpdateTags([FromBody] List<string> value)
        {
            if (value.All(x => !string.IsNullOrWhiteSpace(x)))
            {
                try
                {
                    List<string> duplicate = new List<string>();
                    var duplicateCL = value.GroupBy(x => x)
                                      .Where(g => g.Count() > 1)
                                      .Select(y => y.Key)
                                      .ToList();
                    if (duplicateCL.Count > 0)
                    {
                        var total = String.Join(",", duplicateCL.ToArray());
                        return new ResponseResult(HttpStatusCode.BadRequest, "Tagname duplicate " + total);
                    }
                    //Check Company already exist TagName
                    foreach (var item in value)
                    {
                        var _checkTagName = await _unitOfWork.companyRepository.GetSingleAsync(x => x.Id == new Guid(Utilities.GetCompanyId(this.User)) && x.Tags.Contains(item));
                        if (_checkTagName != null)
                        {
                            duplicate.Add(item);
                        }
                    }

                    if (duplicate.Count > 0)
                    {
                        var total = String.Join(",", duplicate.ToArray());
                        return new ResponseResult(HttpStatusCode.BadRequest, "Company areadly tag name " + total);
                    }
                    else
                    {
                        var _result = await _buyersServices.UpdateTags(Utilities.GetCompanyId(this.User), value);
                        if (_result.Success)
                        {
                            return new ResponseResult(HttpStatusCode.OK, _result.Item);
                        }
                        return new ResponseResult(HttpStatusCode.NotFound, "Error add tags");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API UpdateTags: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Get All Tags or Filter by Tags Name the buyer's company (role buyer)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// Get all tags the buyer's company
        /// if Name == null then Get All Tags
        /// if Name != null then Filter Tags by Name
        /// </remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("tags")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult FilterTags(string name)
        {
            try
            {
                var _result = _buyersServices.FilterTags(Utilities.GetCompanyId(this.User), name);
                return new ResponseResult(HttpStatusCode.OK, _result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API FilterTags: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get list products of supplier (role buyer)
        /// </summary>
        /// <returns>List drp products</returns>
        [HttpGet("list-products")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<ListProductResponseModel> GetListProducts([Required]string supplierId)
        {
            try
            {
                var supplier = _unitOfWork.suppliersRepository.GetWhere(x => x.Id == new Guid(supplierId)).FirstOrDefault();
                if (supplier == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Supplier not found");
                }
                var response = _buyersServices.GetListProductOfSupplierById(supplier.Id);

                return new ResponseResult(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetListProducts: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get supplier detail by id (role buyer)
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-supplier-details")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<SupplierDetailResponseModel> GetSupplierDetails([Required]string supplierId)
        {
            try
            {
                var user = _buyersServices.GetSupplierDetailById(new Guid(supplierId));
                if (user != null)
                {
                    return new ResponseResult(HttpStatusCode.OK, user);
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Supplier not found");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetSupplierDetails: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Update supplier profile by Id (role: buyer)
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("update-supplier/{supplierId}")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<IActionResult> UpdateSupplierProfile(string supplierId, [FromBody]SupplierProfilesParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user_collection = _unitOfWork.accountRepository.QueryAll();
                    var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();

                    var supplier_info = (from u in user_collection.AsEnumerable()
                                         join s in supplier_collection.AsEnumerable() on u.Id.ToString() equals s.UserId
                                         where (s.Id == new Guid(supplierId))
                                         select new
                                         {
                                             Email = u.Email
                                         }).FirstOrDefault();

                    if (supplier_info.Email.Trim().ToLower() != param.Email.Trim().ToLower())
                    {
                        var user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Email == param.Email);
                        if (user != null)
                        {
                            return new ResponseResult(HttpStatusCode.BadRequest, "Email already exists in the system");
                        }
                    }
                    var result = await _buyersServices.UpdateSupplierProfile(supplierId, param);
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, "Update succesfull");
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API UpdateSupplierProfile: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Get buyer dashboard statistical (role buyer)
        /// </summary>
        /// <returns></returns>
        [HttpGet("dashboard-statistical")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult Statistical()
        {
            try
            {
                var _id = Utilities.GetBuyerId(this.User);
                var _companyId = Utilities.GetCompanyId(this.User);
                var _result = _buyersServices.Statistical(_id, _companyId);
                return new ResponseResult(HttpStatusCode.OK, _result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API Statistical: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}