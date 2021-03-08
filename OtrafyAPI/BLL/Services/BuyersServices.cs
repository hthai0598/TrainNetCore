using BLL.Services.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using DAL.Models;
using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using BLL.Models;
using DAL.Core;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using BLL.Helpers;

namespace BLL.Services
{
    public class BuyersServices : IBuyersServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
        private readonly ISendGridSender _sendGridSender;
        private readonly IEmailTemplates _emailTemplates;
        private readonly ILogger<IBuyersServices> _logger;
        private const string _prefixLog = "BuyersServices->";

        public BuyersServices(IConfiguration configuration, IUnitOfWork unitOfWork, ITokenServices tokenServices, ISendGridSender sendGridSender, IEmailTemplates emailTemplates, ILogger<IBuyersServices> logger)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
            _sendGridSender = sendGridSender;
            _emailTemplates = emailTemplates;
            _logger = logger;
        }
        public async Task<CommitResult> CreateBuyers(Buyers item)
        {
            try
            {
                _unitOfWork.buyersRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "CreateBuyers: ", ex.Message);
                throw;
            }
        }
        public PagedResult<BuyersResponseModel> GetAllBuyerOfCompany(string CompanyId, GetFilterParams model)
        {
            bool SortOnValid = false;
            if (!string.IsNullOrEmpty(model.SortOn))
            {
                foreach (PropertyInfo p in typeof(BuyersResponseModel).GetProperties())
                {
                    string propertyName = p.Name;
                    if (model.SortOn.ToLower() == propertyName.ToLower())
                    {
                        model.SortOn = propertyName;
                        SortOnValid = true;
                        break;
                    }
                }
            }

            bool filterValid = false;
            string type = null;

            List<string> filterBy = new List<string>();

            if (model.FilterBy != null && model.FilterValue != null && model.FilterBy.Count() == model.FilterValue.Count())
            {

                if (model.FilterValue.Where(x => x != null && x.Any()).Count() == model.FilterBy.Count() && model.FilterBy.Where(x => string.IsNullOrEmpty(x)).Count() == 0)
                {

                    for (var i = 0; i < model.FilterBy.Count(); i++)
                    {
                        foreach (PropertyInfo p in typeof(BuyersResponseModel).GetProperties())
                        {
                            type = p.PropertyType.Name.ToLower();
                            string propertyName = p.Name;
                            if (!filterBy.Contains(propertyName) && propertyName.ToLower() == model.FilterBy[i].ToLower())
                            {
                                filterBy.Add(propertyName);
                                break;
                            }
                        }
                    }
                }
                if (filterBy.Count() == model.FilterBy.Count())
                {
                    filterValid = true;
                    model.FilterBy = filterBy;
                }
            }

            var user_collection = _unitOfWork.accountRepository.QueryAll();
            var buyer_collection = _unitOfWork.buyersRepository.QueryAll();

            var result = (from u in user_collection.AsEnumerable()
                          join b in buyer_collection.AsEnumerable() on u.Id.ToString() equals b.UserId
                          where (b.CompanyId == CompanyId)
                          select new BuyersResponseModel()
                          {
                              Id = b.Id.ToString(),
                              Name = u.UserProfiles.FirstName + ' ' + u.UserProfiles.LastName,
                              Username = u.Username,
                              Email = u.Email,
                              Permission = b.Permission,
                              JobTitle = u.UserProfiles.JobTitle,
                              isActive = b.isActive,
                              CreatedBy = b.CreatedBy,
                              CreatedDate = b.CreatedDate
                          });

            #region Filter
            if (filterValid)
            {
                for (int i = 0; i < model.FilterBy.Count(); i++)
                {
                    string _filterBy = model.FilterBy[i];
                    List<string> _filterValues = model.FilterValue[i].Where(x => x != null).ToList();

                    switch (_filterBy)
                    {
                        case "Name":
                            {
                                var _result_filtername = result.Where(x => x.Name != null ? _filterValues.Where(z => x.Name.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                if (_result_filtername.Count() <= 0)
                                {
                                    goto case "Username";
                                }
                                result = _result_filtername;
                                break;
                            }
                        case "Username":
                            {
                                result = result.Where(x => x.Username != null ? _filterValues.Where(z => x.Username.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                break;
                            }
                        default:
                            break;
                    }
                    break; // exit for
                }
            }
            #endregion

            #region Sort
            if (SortOnValid && model.SortType != null)
            {
                if (model.SortType == SortType.Ascending)
                {
                    result = result.OrderBy(x => x.GetType().GetProperty(model.SortOn).GetValue(x, null));
                }
                else
                {
                    result = result.OrderByDescending(x => x.GetType().GetProperty(model.SortOn).GetValue(x, null));
                }
            }
            else
            {
                result = result.OrderBy(x => x.CreatedDate);
            }

            var totalpage = result.Count();

            if (model.pageNumber != null && model.pageSize != null)
            {
                int skip = ((int)model.pageNumber - 1) * (int)model.pageSize;
                result = result.Skip(skip).Take((int)model.pageSize);
            }
            #endregion

            return new PagedResult<BuyersResponseModel>(result.ToList(), model.pageNumber ?? 0, model.pageSize ?? totalpage, totalpage);

        }

        public async Task<bool> ResendInviteBuyer(string current_userid, Buyers buyer)
        {
            var current_user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Id == new Guid(current_userid));
            var current_company = await _unitOfWork.companyRepository.GetSingleAsync(x => x.Id == new Guid(buyer.CompanyId));

            var token = await _tokenServices.GetToken(new Guid(buyer.InviteToken));
            if (token != null && token.ValidUntil < DateTime.Now.ToUniversalTime())
            {
                token.ValidUntil = DateTime.UtcNow.AddHours(Int32.Parse(_configuration["TokenLifespan:InviteUser"]));
            }
            token.RetryCount = token.RetryCount + 1;
            _unitOfWork.tokenRepository.Update(token);
            await _unitOfWork.CommitAsync();

            var user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Id == new Guid(buyer.UserId));
            // Send mail invite buyer
            string invite_url = _configuration["AppSettings:Frontend_Url"] + "/invite/" + buyer.InviteToken;
            string message = _emailTemplates.GetInviteBuyerEmailTemplate(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, user.Email, invite_url, current_user.UserProfiles.FirstName + " " + current_user.UserProfiles.LastName, current_company.Name);
            var sendmail_reps = await _sendGridSender.SendEmailAsync(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, user.Email, "Invite Buyer", message);
            _logger.LogInformation("Resend email invite buyer to (" + user.Email + ")");

            return sendmail_reps.Successful;
        }

        public async Task<bool> DeleteBuyer(string BuyerId)
        {
            try
            {
                var buyer = await _unitOfWork.buyersRepository.GetById(new Guid(BuyerId));
                if (buyer != null)
                {
                    _unitOfWork.tokenRepository.Remove(new Guid(buyer.InviteToken));
                    _unitOfWork.accountRepository.Remove(new Guid(buyer.UserId));
                    _unitOfWork.buyersRepository.Remove(new Guid(BuyerId));
                    return await _unitOfWork.CommitAsync();
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "DeleteBuyer: ", ex.Message);
                throw ex;
            }
        }

        public async Task<CommitResult> CreateFormsDesigner(string companyId, FormDesigner item)
        {
            try
            {
                var company = _unitOfWork.companyRepository.GetWhere(x => x.Id == new Guid(companyId)).FirstOrDefault();
                if (company.FormDesigner != null)
                {
                    company.FormDesigner.Add(item);
                }
                else
                {
                    company.FormDesigner = new List<FormDesigner>();
                    company.FormDesigner.Add(item);
                }

                _unitOfWork.companyRepository.Update(company);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, company.FormDesigner);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "CreateFormsDesigner: ", ex.Message);
                throw;
            }
        }

        public async Task<CommitResult> UpdateTags(string companyId, List<string> model)
        {

            //check tag
            var company = _unitOfWork.companyRepository.GetWhere(x => x.Id == new Guid(companyId)).FirstOrDefault();
            if (company.Tags == null)
            {
                company.Tags = new List<string>();
                foreach (var item in model)
                {
                    company.Tags.Add(item);
                }
                _unitOfWork.companyRepository.Update(company);
                var _result = await _unitOfWork.CommitAsync();
                return new CommitResult(_result, company.Tags);
            }
            else
            {
                foreach (var item in model)
                {
                    company.Tags.Add(item);
                }
                _unitOfWork.companyRepository.Update(company);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, company.Tags);
            }
        }

        public List<string> FilterTags(string companyId, string name)
        {
            List<string> _result = null;
            var _company = _unitOfWork.companyRepository.QueryAll();
            var _tagList = (from com in _company.AsEnumerable()
                            where com.Id == new Guid(companyId)
                            select new { Tags = com.Tags });
            if (_tagList != null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    _result = _tagList.SelectMany(x => x.Tags).ToList();
                }
                else
                {
                    foreach (var item in _tagList)
                    {
                        _result = item.Tags.Where(x => x.StartsWith(name)).ToList();
                    }
                }
            }
            return _result;
        }

        public List<ListProductResponseModel> GetListProductOfSupplierById(Guid supplierId)
        {
            var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
            var list_products = from s in supplier_collection
                                where (s.Id == supplierId)
                                from p in s.Products
                                select new ListProductResponseModel()
                                {
                                    Id = p.Id,
                                    ProductName = p.Name
                                };
            return list_products.ToList();
        }

        public List<ListFormResponseModel> GetListFormsOfCompanyById(Guid companyId, string formname)
        {
            var company_collection = _unitOfWork.companyRepository.QueryAll();
            var list_forms = from s in company_collection.AsEnumerable()
                             where (s.Id == companyId)
                             from p in s.FormDesigner
                             select new ListFormResponseModel()
                             {
                                 Id = p.Id,
                                 FormName = p.Name
                             };
            if (formname == null)
            {
                return list_forms.ToList();
            }
            list_forms = list_forms.Where(x => x.FormName.ToLower().Contains(formname.ToLower()));
            return list_forms.ToList();
        }

        public List<ListBuyerResponseModel> GetListBuyerInContact(Guid companyId, string fullname)
        {
            var company_collection = _unitOfWork.companyRepository.QueryAll();
            var account_collection = _unitOfWork.accountRepository.QueryAll();
            var buyer_collection = _unitOfWork.buyersRepository.QueryAll();
            var buyer = from s in buyer_collection.AsEnumerable()
                        where s.CompanyId == Convert.ToString(companyId)
                        select new
                        {
                            BuyerId = s.UserId,
                        };
            var nameBuyer = from s in account_collection.AsEnumerable()
                            from w in buyer
                            where s.Id == new Guid(w.BuyerId)
                            select new ListBuyerResponseModel
                            {
                                FullName = s.UserProfiles.FirstName + " " + s.UserProfiles.LastName,
                                Id = w.BuyerId
                            };
            if (fullname == null)
            {
                return nameBuyer.ToList();
            }
            nameBuyer = nameBuyer.Where(x => x.FullName.ToLower().Contains(fullname.ToLower()));
            return nameBuyer.ToList();
        }

        public async Task<CommitResult> CreateNewRequest(string supplierId, FormRequest item)
        {
            try
            {
                var supplier = _unitOfWork.suppliersRepository.GetWhere(x => x.Id == new Guid(supplierId)).FirstOrDefault();
                if (supplier.FormRequest != null)
                {
                    supplier.FormRequest.Add(item);
                }
                else
                {
                    supplier.FormRequest = new List<FormRequest>();
                    supplier.FormRequest.Add(item);
                }
                supplier.FormRequestTimeline = new List<FormRequestTimeline>() { new FormRequestTimeline { FormRequestId = item.Id.ToString(), Status = RequestStatus.pending, UpdatedBy = item.CreatedBy, UpdatedDate = DateTime.Now } };

                _unitOfWork.suppliersRepository.Update(supplier);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, supplier.FormRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "CreateNewRequest: ", ex.Message);
                throw;
            }
        }

        public SupplierDetailResponseModel GetSupplierDetailById(Guid supplierId)
        {
            var user_collection = _unitOfWork.accountRepository.QueryAll();
            var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();

            var result = (from user in user_collection.AsEnumerable()
                          join b in supplier_collection.AsEnumerable() on user.Id.ToString() equals b.UserId
                          where (b.Id == supplierId)
                          select new SupplierDetailResponseModel()
                          {
                              SupplierId = b.Id.ToString(),
                              Email = user.Email,
                              Tags = b.Tags,
                              UserProfiles = user.UserProfiles,
                              CompanyProfiles = user.CompanyProfiles
                          }).FirstOrDefault();
            return result;
        }

        public PagedResult<FormRequestResponseModel> GetAllFormRequestBySupplierId(string supplierId, GetFilterParams model)
        {
            bool SortOnValid = false;
            if (!string.IsNullOrEmpty(model.SortOn))
            {
                foreach (PropertyInfo p in typeof(FormRequestResponseModel).GetProperties())
                {
                    string propertyName = p.Name;
                    if (model.SortOn.ToLower() == propertyName.ToLower())
                    {
                        model.SortOn = propertyName;
                        SortOnValid = true;
                        break;
                    }
                }
            }

            bool filterValid = false;
            string type = null;

            List<string> filterBy = new List<string>();

            if (model.FilterBy != null && model.FilterValue != null && model.FilterBy.Count() == model.FilterValue.Count())
            {

                if (model.FilterValue.Where(x => x != null && x.Any()).Count() == model.FilterBy.Count() && model.FilterBy.Where(x => string.IsNullOrEmpty(x)).Count() == 0)
                {

                    for (var i = 0; i < model.FilterBy.Count(); i++)
                    {
                        foreach (PropertyInfo p in typeof(FormRequestResponseModel).GetProperties())
                        {
                            type = p.PropertyType.Name.ToLower();
                            string propertyName = p.Name;
                            if (!filterBy.Contains(propertyName) && propertyName.ToLower() == model.FilterBy[i].ToLower())
                            {
                                filterBy.Add(propertyName);
                                break;
                            }
                        }
                    }
                }
                if (filterBy.Count() == model.FilterBy.Count())
                {
                    filterValid = true;
                    model.FilterBy = filterBy;
                }
            }

            var user_collection = _unitOfWork.accountRepository.QueryAll();
            var company_collection = _unitOfWork.companyRepository.QueryAll();
            var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
            var result = from s in supplier_collection.AsEnumerable()
                         where (s.Id == new Guid(supplierId))
                         from f in s.FormRequest
                         select new FormRequestResponseModel()
                         {
                             Id = f.Id,
                             Title = f.Title,
                             Status = f.Status,
                             CreatedDate = f.CreatedDate,
                             DateUpdated = f.UpdatedDate,
                             BuyerInCharge = user_collection.AsEnumerable().Where(x11 => x11.Id == new Guid(f.CreatedBy)).Select(x2 => x2.UserProfiles).Select(xx => new { Fullname = xx.FirstName + " " + xx.LastName }).FirstOrDefault() != null ? user_collection.AsEnumerable().Where(x11 => x11.Id == new Guid(f.CreatedBy)).Select(x2 => x2.UserProfiles).Select(xx => new { Fullname = xx.FirstName + " " + xx.LastName }).FirstOrDefault().Fullname : "",
                             CompanyName = company_collection.AsEnumerable().Where(x12 => x12.Id == new Guid(s.CompanyId)).Select(x => x.Name).FirstOrDefault(),
                             ProductName = s.Products.Where(x13 => x13.Id == new Guid(f.ProductId)).FirstOrDefault() != null ? s.Products.Where(x13 => x13.Id == new Guid(f.ProductId)).FirstOrDefault().Name : "",
                             Description = ""
                         };

            #region Filter
            if (filterValid)
            {
                for (int i = 0; i < model.FilterBy.Count(); i++)
                {
                    string _filterBy = model.FilterBy[i];
                    List<string> _filterValues = model.FilterValue[i].Where(x => x != null).ToList();

                    switch (_filterBy)
                    {
                        case "Title":
                            {
                                result = result.Where(x => x.Title != null ? _filterValues.Where(z => x.Title.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            #endregion

            #region Sort
            if (SortOnValid && model.SortType != null)
            {
                if (model.SortType == SortType.Ascending)
                {
                    result = result.OrderBy(x => x.GetType().GetProperty(model.SortOn).GetValue(x, null));
                }
                else
                {
                    result = result.OrderByDescending(x => x.GetType().GetProperty(model.SortOn).GetValue(x, null));
                }
            }
            else
            {
                result = result.OrderBy(x => x.DateUpdated);
            }

            var totalpage = result.Count();

            if (model.pageNumber != null && model.pageSize != null)
            {
                int skip = ((int)model.pageNumber - 1) * (int)model.pageSize;
                result = result.Skip(skip).Take((int)model.pageSize);
            }
            #endregion

            return new PagedResult<FormRequestResponseModel>(result.ToList(), model.pageNumber ?? 0, model.pageSize ?? totalpage, totalpage);
        }

        public Statistical Statistical(string buyerId, string companyId)
        {
            Statistical dashBoards = new Statistical();
            var _supplier_Collection = _unitOfWork.suppliersRepository.QueryAll();
            var _company_Collection = _unitOfWork.companyRepository.QueryAll();
            var _suppliers = _unitOfWork.suppliersRepository.GetList(x => x.BuyerId == buyerId);

            dashBoards.TotalSuppliers = _suppliers.Count();

            var _FormRequest = (from sup in _supplier_Collection where sup.BuyerId == buyerId select sup);
            foreach (var item in _FormRequest)
            {
                var total = item.FormRequest.Where(x => (int)(x.Status) == 1).Count();
                dashBoards.PendingRequest = dashBoards.PendingRequest + total;
            }
        
            var _company = _unitOfWork.companyRepository.GetSingle(x => x.Id == new Guid(companyId));
            if (_company != null)
            {
                dashBoards.TotalForm = _company.FormDesigner.Count();
            }

            return dashBoards;
        }

        public PagedResult<FormResponseModel> GetAllForms(string companyId, GetFilterParams model)
        {
            bool SortOnValid = false;
            if (!string.IsNullOrEmpty(model.SortOn))
            {
                foreach (PropertyInfo p in typeof(FormResponseModel).GetProperties())
                {
                    string propertyName = p.Name;
                    if (model.SortOn.ToLower() == propertyName.ToLower())
                    {
                        model.SortOn = propertyName;
                        SortOnValid = true;
                        break;
                    }
                }
            }

            bool filterValid = false;
            string type = null;

            List<string> filterBy = new List<string>();

            if (model.FilterBy != null && model.FilterValue != null && model.FilterBy.Count() == model.FilterValue.Count())
            {

                if (model.FilterValue.Where(x => x != null && x.Any()).Count() == model.FilterBy.Count() && model.FilterBy.Where(x => string.IsNullOrEmpty(x)).Count() == 0)
                {

                    for (var i = 0; i < model.FilterBy.Count(); i++)
                    {
                        foreach (PropertyInfo p in typeof(FormResponseModel).GetProperties())
                        {
                            type = p.PropertyType.Name.ToLower();
                            string propertyName = p.Name;
                            if (!filterBy.Contains(propertyName) && propertyName.ToLower() == model.FilterBy[i].ToLower())
                            {
                                filterBy.Add(propertyName);
                                break;
                            }
                        }
                    }
                }
                if (filterBy.Count() == model.FilterBy.Count())
                {
                    filterValid = true;
                    model.FilterBy = filterBy;
                }
            }

            var company_collection = _unitOfWork.companyRepository.QueryAll();
            var result = from s in company_collection.AsEnumerable()
                         where (s.Id == new Guid(companyId))
                         from f in s.FormDesigner
                         select new FormResponseModel()
                         {
                             Id = f.Id,
                             FormType = f.FormType,
                             Name = f.Name,
                             Tags = f.Tags,
                             FormDesignerData = f.FormDesignerData,
                             Description = f.Description,
                             CreatedBy = f.CreatedBy,
                             CreatedDate = f.CreatedDate,
                             UpdatedBy = f.UpdatedBy,
                             UpdatedDate = f.UpdatedDate,
                             CurrentVersion = f.CurrentVersion
                         };

            #region Filter
            if (filterValid)
            {
                for (int i = 0; i < model.FilterBy.Count(); i++)
                {
                    string _filterBy = model.FilterBy[i];
                    List<string> _filterValues = model.FilterValue[i].Where(x => x != null).ToList();

                    switch (_filterBy)
                    {
                        case "Name":
                            {
                                result = result.Where(x => x.Name != null ? _filterValues.Where(z => x.Name.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            #endregion

            #region Sort
            if (SortOnValid && model.SortType != null)
            {
                if (model.SortType == SortType.Ascending)
                {
                    result = result.OrderBy(x => x.GetType().GetProperty(model.SortOn).GetValue(x, null));
                }
                else
                {
                    result = result.OrderByDescending(x => x.GetType().GetProperty(model.SortOn).GetValue(x, null));
                }
            }
            else
            {
                result = result.OrderBy(x => x.UpdatedDate);
            }

            var totalpage = result.Count();

            if (model.pageNumber != null && model.pageSize != null)
            {
                int skip = ((int)model.pageNumber - 1) * (int)model.pageSize;
                result = result.Skip(skip).Take((int)model.pageSize);
            }
            #endregion

            return new PagedResult<FormResponseModel>(result.ToList(), model.pageNumber ?? 0, model.pageSize ?? totalpage, totalpage);
        }

        public async Task<CommitResult> UpdateSupplierProfile(string supplierId, SupplierProfilesParam param)
        {
            try
            {
                var supplier = await _unitOfWork.suppliersRepository.GetById(new Guid(supplierId));
                if (supplier == null)
                {
                    return new CommitResult(false);
                }

                var user = await _unitOfWork.accountRepository.GetById(new Guid(supplier.UserId));
                if (user == null)
                {
                    return new CommitResult(false);
                }
                if (user.Email != param.Email)
                {
                    user.Email = param.Email;
                }
                if (param.FirstName != null && param.LastName != null && param.Phone != null && param.JobTitle != null)
                {
                    user.UserProfiles.FirstName = param.FirstName;
                    user.UserProfiles.LastName = param.LastName;
                    user.UserProfiles.Phone = param.Phone;
                    user.UserProfiles.JobTitle = param.JobTitle;
                }

                if (user.CompanyProfiles == null)
                {
                    user.CompanyProfiles = new UserCompanyProfiles()
                    {
                        CompanyName = param.CompanyName,
                        Address = param.Address,
                        Description = param.Description
                    };
                }
                else
                {
                    if (param.CompanyName != null)
                        user.CompanyProfiles.CompanyName = param.CompanyName;

                    if (param.Address != null)
                        user.CompanyProfiles.Address = param.Address;

                    user.CompanyProfiles.Description = param.Description;
                }
                _unitOfWork.accountRepository.Update(user);

                // Update Tags of supplier
                if (param.Tags != null)
                    supplier.Tags = param.Tags;


                _unitOfWork.suppliersRepository.Update(supplier);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "UpdateSupplierProfile: ", ex.Message);
                throw ex;
            }
        }

        public async Task<CommitResult> UpdateForm(string companyId, string currentUserId, string formrId, FormParam param)
        {
            try
            {
                var _company = await _unitOfWork.companyRepository.GetById(new Guid(companyId));
                if (_company == null)
                {
                    return new CommitResult(false);
                }
                var _formDesigner = _company.FormDesigner.Where(x => x.Id == new Guid(formrId)).FirstOrDefault();
                if (_formDesigner != null)
                {
                    var _currversion = _formDesigner.FormDesignerData.Max(x => x.Version);

                    var serveyJson = JsonConvert.SerializeObject(param.SurveyDesigner);
                    FormDesignerData _surveyformVer = new FormDesignerData()
                    {
                        SurveyDesigner = serveyJson,
                        Version = _currversion + 1,
                        CreatedBy = currentUserId,
                        CreatedDate = DateTime.Now
                    };
                    _formDesigner.FormDesignerData.Add(_surveyformVer);
                    _formDesigner.CurrentVersion = _currversion + 1;
                    _formDesigner.FormType = param.FormType;
                    _formDesigner.Name = param.Name;
                    _formDesigner.Tags = param.Tags;
                    _formDesigner.Description = param.Description;

                    _unitOfWork.companyRepository.Update(_company);
                    var result = await _unitOfWork.CommitAsync();
                    return new CommitResult(result, _formDesigner);
                }
                return new CommitResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "UpdateForm: ", ex.Message);
                throw ex;
            }
        }

        public FormDetailResponseModel GetFormDetailById(string companyId, Guid formId)
        {
            var company_collection = _unitOfWork.companyRepository.QueryAll();
            var _list_formsDesigner = company_collection.AsEnumerable().Where(x => x.Id == new Guid(companyId)).SelectMany(x => x.FormDesigner);
            var _formsDesigner = _list_formsDesigner.Where(x => x.Id == formId).FirstOrDefault();
            if (_formsDesigner != null)
            {
                var result = new FormDetailResponseModel()
                {
                    Id = _formsDesigner.Id,
                    FormType = _formsDesigner.FormType,
                    Name = _formsDesigner.Name,
                    Tags = _formsDesigner.Tags,
                    FormDesignerData = new List<FormDesignerData>() { _formsDesigner.FormDesignerData.OrderByDescending(item => item.Version).FirstOrDefault() },
                    Description = _formsDesigner.Description,
                    CreatedBy = _formsDesigner.CreatedBy,
                    CreatedDate = _formsDesigner.CreatedDate,
                    UpdatedBy = _formsDesigner.UpdatedBy,
                    UpdatedDate = _formsDesigner.UpdatedDate,
                    CurrentVersion = _formsDesigner.CurrentVersion
                };
                return result;
            }
            return null;
        }

        public async Task<bool> ResendFormRequestToSupplier(string current_userid, ResendFormRequestParam param)
        {
            var _supplier = await _unitOfWork.suppliersRepository.GetById(new Guid(param.SupplierId));
            var _formsRequest = _supplier.FormRequest.Where(x => x.Id == new Guid(param.FormRequestId)).FirstOrDefault();
            if (_formsRequest != null)
            {
                //update form request with form new version
                if (_formsRequest.FormRequestData != null && _formsRequest.FormRequestData.Count() > 0)
                {
                    foreach (var item in _formsRequest.FormRequestData)
                    {
                        var company_collection = _unitOfWork.companyRepository.QueryAll();
                        var _list_formsDesigner = company_collection.Where(x => x.Id == new Guid(_supplier.CompanyId)).SelectMany(x => x.FormDesigner);
                        var _formDesigner = _list_formsDesigner.Where(x => x.Id == item.FormId).FirstOrDefault();
                        if (item.Version < _formDesigner.FormDesignerData.Max(x => x.Version))
                        {
                            var _formdesignerdata = _formDesigner.FormDesignerData.OrderByDescending(xx => xx.Version).FirstOrDefault();
                            item.Version = _formdesignerdata.Version;
                            _unitOfWork.suppliersRepository.Update(_supplier);
                        }
                    }
                    await _unitOfWork.CommitAsync();
                }

                var token = await _tokenServices.GetToken(new Guid(_supplier.InviteToken));
                if (token != null && token.ValidUntil < DateTime.Now.ToUniversalTime())
                {
                    token.ValidUntil = DateTime.UtcNow.AddHours(Int32.Parse(_configuration["TokenLifespan:InviteUser"]));
                }
                token.RetryCount = token.RetryCount + 1;
                _unitOfWork.tokenRepository.Update(token);
                await _unitOfWork.CommitAsync();

                var user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Id == new Guid(_supplier.UserId));
                var current_user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Id == new Guid(current_userid));
                // Send mail invite form request
                string message = _emailTemplates.GetSendFormRequestEmailTemplate(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, user.Email, _formsRequest.Message, current_user.UserProfiles.FirstName + " " + current_user.UserProfiles.LastName);
                var sendmail_reps = await _sendGridSender.SendEmailAsync(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, user.Email, "New Request", message);
                _logger.LogInformation("Resend email form request to (" + user.Email + ")");

                return sendmail_reps.Successful;
            }
            return false;
        }

        public async Task<CommitResult> UpdateStatusFormRequest(string current_userid, string supplierId, string formrequestId, FormRequestStatusParam param)
        {
            try
            {
                var supplier = await _unitOfWork.suppliersRepository.GetById(new Guid(supplierId));
                if (supplier == null)
                {
                    return new CommitResult(false);
                }
                var _formrequest = supplier.FormRequest.Where(x => x.Id == new Guid(formrequestId)).FirstOrDefault();
                if (_formrequest != null)
                {
                    _formrequest.Status = (RequestStatus)param.Status;
                    _formrequest.UpdatedDate = DateTime.Now;
                    _formrequest.UpdatedBy = current_userid;
                    supplier.FormRequestTimeline = new List<FormRequestTimeline>() { new FormRequestTimeline { FormRequestId = formrequestId, Status = (RequestStatus)param.Status, UpdatedBy = current_userid, UpdatedDate = DateTime.Now } };

                    var result = await _unitOfWork.CommitAsync();
                    return new CommitResult(result, _formrequest);
                }
                return new CommitResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "UpdateStatusFormRequest: ", ex.Message);
                throw ex;
            }
        }

        public async Task<bool> DeleteFormRequest(string supplierId, string formRequestId)
        {
            try
            {
                var suppliers = await _unitOfWork.suppliersRepository.GetById(new Guid(supplierId));
                if (suppliers != null)
                {
                    var item = suppliers.FormRequest.AsEnumerable().Where(x => x.Id == new Guid(formRequestId)).FirstOrDefault();
                    if (item != null)
                    {
                        suppliers.FormRequest.Remove(item);
                        _unitOfWork.suppliersRepository.Update(suppliers);
                        return await _unitOfWork.CommitAsync();
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "DeleteFormRequest: ", ex.Message);
                throw ex;
            }
        }

        public async Task<bool> DeleteForm(string companyId, string formId)
        {
            try
            {
                var company = await _unitOfWork.companyRepository.GetById(new Guid(companyId));
                if (company != null)
                {
                    var item = company.FormDesigner.AsEnumerable().Where(x => x.Id == new Guid(formId)).FirstOrDefault();
                    if (item != null)
                    {
                        company.FormDesigner.Remove(item);
                        _unitOfWork.companyRepository.Update(company);
                        return await _unitOfWork.CommitAsync();
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "DeleteForm: ", ex.Message);
                throw ex;
            }
        }
    }
}
