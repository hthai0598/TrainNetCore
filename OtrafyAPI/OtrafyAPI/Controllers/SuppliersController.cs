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
using Microsoft.Extensions.Logging;
using BLL.Helpers;

namespace OtrafyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/supplier")]
    public class SuppliersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendGridSender _sendGridSender;
        private readonly IEmailTemplates _emailTemplates;
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
        private readonly IAccountServices _accountServices;
        private readonly ISuppliersServices _suppliersServices;
        private readonly IBuyersServices _buyersServices;
        private readonly ILogger _logger;

        public SuppliersController(IUnitOfWork unitOfWork, ISendGridSender sendGridSender, IEmailTemplates emailTemplates, IConfiguration configuration, ITokenServices tokenServices, ISuppliersServices suppliersServices, IAccountServices accountServices, IBuyersServices buyersServices, ILogger<SuppliersController> logger)
        {
            _unitOfWork = unitOfWork;
            _sendGridSender = sendGridSender;
            _emailTemplates = emailTemplates;
            _configuration = configuration;
            _tokenServices = tokenServices;
            _suppliersServices = suppliersServices;
            _accountServices = accountServices;
            _buyersServices = buyersServices;
            _logger = logger;
        }

        /// <summary>
        /// Add new supplier (role buyer)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create-supplier")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<ActionResult> AddNewSupplier([FromBody] InviteSupplierParams value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var current_user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Id == new Guid(Utilities.GetUserId(this.User)));
                    var companyid = Utilities.GetCompanyId(this.User);
                    var company = await _unitOfWork.companyRepository.GetById(new Guid(companyid));
                    if (company == null)
                    {
                        return new ResponseResult(HttpStatusCode.NotFound, "Company not found");
                    }
                    var allsupplier = await _unitOfWork.suppliersRepository.GetListAsync(x => x.CompanyId == companyid);
                    if (allsupplier.Count() >= company.MaxNumberSuppliersAllowed)
                    {
                        return new ResponseResult(HttpStatusCode.BadRequest, "Maximum number of supplier allowed:" + company.MaxNumberSuppliersAllowed);
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
                            Role = Role.suppliers.ToString(),
                            UserProfiles = new UserProfiles
                            {
                                Avatar = "default_avatar.png",
                                FirstName = value.FirstName,
                                LastName = value.LastName,
                                JobTitle = "",
                                Message = ""
                            },
                            CompanyProfiles = new UserCompanyProfiles
                            {
                                CompanyName = value.CompanyName,
                                ProductType = "",
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
                            IssueType = IssuerType.InviteSupplier,
                            IssuerSubject = "Send invite supplier",
                            SentDate = DateTime.Now,
                            RetryCount = 0
                        });

                        // Create Supplier
                        await _suppliersServices.CreateSupplier(new Suppliers
                        {
                            UserId = user.Id.ToString(),
                            CompanyId = companyid,
                            BuyerId = Utilities.GetBuyerId(this.User),
                            InviteToken = token_code,
                            IsActive = false,
                            CreatedBy = Utilities.GetUserId(this.User),
                            CreatedDate = DateTime.Now,
                            Tags = value.Tags
                        });

                        if (value.isSendEmailInvitation)
                        {
                            // Send mail invite supplier
                            string invite_url = _configuration["AppSettings:Frontend_Url"] + "/invite/" + token_code;
                            string message = _emailTemplates.GetInviteSupplierEmailTemplate(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, user.Email, invite_url, current_user.UserProfiles.FirstName + "" + current_user.UserProfiles.LastName);
                            await _sendGridSender.SendEmailAsync(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, user.Email, "Invite Supplier", message);
                            _logger.LogInformation("Send email invite supplier to (" + user.Email + ")");

                        }

                        return new ResponseResult(HttpStatusCode.OK, "Invite supplier successfully");
                    }
                    else
                    {
                        return new ResponseResult(HttpStatusCode.BadRequest, "User already exists in the system");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API AddNewSupplier: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Get all supplier of buyer (search, filter, paging, order) (role buyer) 
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// pending = 1: is Supplier new create; inprogress =2; completed = 3; approved = 4; rejected = 5
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<PagedResult<SupplierResponseModel>> GetAllSupplierOfBuyer(GetFilterParams model)
        {
            try
            {
                var response = _suppliersServices.GetAllSupplierOfBuyer(Utilities.GetCompanyId(this.User), Utilities.GetBuyerId(this.User), model);
                return new ResponseResult(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetAllSupplierOfBuyer: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Remove supplier Tags (role buyer)
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{supplierId}")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<ActionResult> Put(string supplierId, [FromBody]TagCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var supplier = _unitOfWork.suppliersRepository.GetSingle(x => x.Id == new Guid(supplierId));
                    if (supplier == null)
                    {
                        return new ResponseResult(HttpStatusCode.NotFound, "Supplier not found");
                    }

                    var result = await _suppliersServices.RemoveTags(supplierId, model.Name);
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, result.Item);
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Tags not found");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API Remove supplier tags: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Get list supplier's company name (role buyer)
        /// </summary>
        /// <returns>List supplier's company name</returns>
        [HttpGet("list-supplier")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<ListSupplierResponseModel> GetListSupplierCompanyName(string name)
        {
            try
            {
                var response = _suppliersServices.GetSupplierCompanyName(Utilities.GetCompanyId(this.User), Utilities.GetBuyerId(this.User), name);

                return new ResponseResult(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetListSupplierCompanyName: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }       

        /// <summary>
        /// Get all form request(search, paging, order) (role supplier) 
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// Status: pending = 1, completed = 2, rejected = 3, approved = 4
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("get-all-form-request")]
        [AuthorizeRolesAttribute(Role.suppliers)]
        public ActionResult<PagedResult<FormRequestResponseModel>> GetAllFormRequest(GetFilterParams model)
        {
            try
            {
                var response = _buyersServices.GetAllFormRequestBySupplierId(Utilities.GetSupplierId(this.User), model);
                return new ResponseResult(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetAllFormRequest: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get form request detail (role supplier) 
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// </remarks>
        /// <param name="formRequestId"></param>
        /// <returns></returns>
        [HttpGet("get-form-request-detail")]
        [AuthorizeRolesAttribute(Role.suppliers)]
        public ActionResult<FormRequestDetailResponseModel> GetFormRequestDetail([Required]string formRequestId)
        {
            try
            {
                var result = _suppliersServices.GetFormRequestDetailById(Utilities.GetSupplierId(this.User), new Guid(formRequestId));
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
        /// Update form request (role: supplier)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// Update form request with multi form
        /// </remarks>
        /// <param name="formrequestId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("update-form-request/{formrequestId}")]
        [AuthorizeRolesAttribute(Role.suppliers)]
        public async Task<IActionResult> UpdateFormRequest(string formrequestId, [FromBody]FormRequestParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _suppliersServices.UpdateFormRequest(Utilities.GetSupplierId(this.User), formrequestId, param);
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, "Update succesfull");
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API UpdateFormRequest: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Update completed form request (role: supplier)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// API allow supplier update status form request to completed
        /// </remarks>
        /// <param name="formrequestId"></param>
        /// <returns></returns>
        [HttpPut("update-completed-form-request/{formrequestId}")]
        [AuthorizeRolesAttribute(Role.suppliers)]
        public async Task<IActionResult> UpdateCompleteFormRequest(string formrequestId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _suppliersServices.UpdateCompleteFormRequest(Utilities.GetSupplierId(this.User), formrequestId);
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, "Update completed form request succesfull");
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API UpdateCompleteFormRequest: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Add comment to form request (role: supplier)
        /// </summary>
        /// <param name="formrequestId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("add-comment-form-request/{formrequestId}")]
        [AuthorizeRolesAttribute(Role.suppliers)]
        public async Task<IActionResult> AddCommentToFormRequest([Required]string formrequestId, [FromBody]FormRequestCommentParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var current_user = Utilities.GetUserId(this.User);
                    var result = await _suppliersServices.AddCommentToFormRequest(current_user, Utilities.GetSupplierId(this.User), formrequestId, param);
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
        /// Get all comment of form request by id (role: supplier)
        /// </summary>
        /// <param name="formrequestId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("get-all-comments")]
        [AuthorizeRolesAttribute(Role.suppliers)]
        public IActionResult GetAllCommentOfFormRequestByID([Required]string formrequestId, CommentsSortByParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _suppliersServices.GetAllCommentOfFormRequestByID(Utilities.GetSupplierId(this.User), formrequestId, param);
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
        /// Create product of supplier (role buyer)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// </remarks>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("create-product")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<ActionResult> CreateProduct([FromBody] ProductParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var supplier = _unitOfWork.suppliersRepository.GetById(new Guid(param.SupplierId));
                    if (supplier == null)
                    {
                        return new ResponseResult(HttpStatusCode.NotFound, "Supplier not found");
                    }

                    //Create Product
                    var result = await _suppliersServices.CreateProduct(param.SupplierId, new Products
                    {
                        Name = param.Name,
                        Code = param.Code,
                        Tags = param.Tags,
                        Grade = param.Grade,
                        Description = param.Description,
                        CreatedBy = Utilities.GetUserId(this.User),
                        CreatedDate = DateTime.Now
                    });

                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, result.Item);
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API CreateProduct: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Get all product by supplierId (paging, search, order) (role buyer)
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
        /// <param name="supplierId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("get-all-products")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<PagedResult<ProductResponseModel>> GetAllProductBySuppliers([Required]string supplierId, GetFilterParams model)
        {
            try
            {
                var suppplier = _unitOfWork.suppliersRepository.GetSingle(x => x.Id == new Guid(supplierId));
                if (suppplier != null)
                {
                    var response = _suppliersServices.GetAllProductBySuppliers(supplierId, model);
                    return new ResponseResult(HttpStatusCode.OK, response);
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Supplier not found");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetAllProductBySuppliers: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get product by id (role buyer)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("get-product-detail")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult GetProductDetail([Required]string productId)
        {
            try
            {
                ProductResponseModel _result = null;
                var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
                _result = (from s in supplier_collection.ToList()
                           from x in s.Products
                           where x.Id == new Guid(productId)
                           select new ProductResponseModel
                           {
                               Id = x.Id,
                               Name = x.Name,
                               Tags = x.Tags,
                               Description = x.Description,
                               Grade = x.Grade,
                               Code = x.Code
                           })
                           .FirstOrDefault();
                if (_result == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Not Found");
                }
                _result = _suppliersServices.GetProductDetail(productId);
                return new ResponseResult(HttpStatusCode.OK, _result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetProductDetail: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }
        /// <summary>
        /// Update product by id (role buyer)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPut("update-product/{productId}")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public async Task<ActionResult> EditProductDetail([Required]string productId, [FromBody] ProductEdit model)
        {
            try
            {
                var _result = await _suppliersServices.EditProductbyId(productId, new Products { Id = Guid.Parse(productId), Grade = model.Grade, Description = model.Description, Code = model.Code, Name = model.Name, Tags = model.Tags });
                if (_result.Success)
                {
                    return new ResponseResult(HttpStatusCode.OK, _result.Item);
                }
                return new ResponseResult(HttpStatusCode.NotFound, "Error");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API EditProductDetail: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get form request by product id (role buyer)
        /// </summary>
        /// <param name="productId">productID</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("get-all-form-request-by-productid")]
        [AuthorizeRolesAttribute(Role.buyers)]
        public ActionResult<PagedResult<RequestFormResponModel>> GetFormRequestByIdProduct([Required]string productId, GetFilterParams model)
        {
            try
            {
                RequestFormResponModel _result = null;
                var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
                _result = (from s in supplier_collection.ToList()
                           from x in s.FormRequest
                           where x.ProductId == productId
                           select new RequestFormResponModel
                           {
                               Title = x.Title,
                               CreatedDate = x.CreatedDate,
                               Status = x.Status,
                               CreatedBy = x.CreatedBy
                           })
                           .FirstOrDefault();
                if (_result == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "Not Found");
                }
                var result =  _suppliersServices.GetRequestFormById(productId,model);
                return new ResponseResult(HttpStatusCode.OK, result);
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetFormRequestByIdProduct: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}