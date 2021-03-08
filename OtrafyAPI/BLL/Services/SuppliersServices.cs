using BLL.Services.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using DAL.Models;
using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using BLL.Models;
using System.Collections.Generic;
using DAL.Core;
using System.Reflection;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using BLL.Helpers;

namespace BLL.Services
{
    public class SuppliersServices : ISuppliersServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
        private readonly ISendGridSender _sendGridSender;
        private readonly IEmailTemplates _emailTemplates;
        private readonly IMongoContext _context;
        private readonly ILogger<SuppliersServices> _logger;
        private const string _prefixLog = "SuppliersServices->";

        public SuppliersServices(IConfiguration configuration, IUnitOfWork unitOfWork, ITokenServices tokenServices, ISendGridSender sendGridSender, IEmailTemplates emailTemplates, IMongoContext context, ILogger<SuppliersServices> logger)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
            _sendGridSender = sendGridSender;
            _emailTemplates = emailTemplates;
            _context = context;
            _logger = logger;
        }

        public async Task<CommitResult> CreateSupplier(Suppliers item)
        {
            try
            {
                _unitOfWork.suppliersRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "CreateSupplier: ", ex.Message);
                throw;
            }
        }

        public PagedResult<SupplierResponseModel> GetAllSupplierOfBuyer(string CompanyId, string BuyerId, GetFilterParams model)
        {
            bool SortOnValid = false;
            if (!string.IsNullOrEmpty(model.SortOn))
            {
                foreach (PropertyInfo p in typeof(SupplierResponseModel).GetProperties())
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
                        foreach (PropertyInfo p in typeof(SupplierResponseModel).GetProperties())
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
            var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
            var company_collection = _unitOfWork.companyRepository.QueryAll();
            var result = (from u in user_collection.AsEnumerable()
                          join b in supplier_collection.AsEnumerable() on u.Id.ToString() equals b.UserId
                          where (b.CompanyId == CompanyId)
                          select new SupplierResponseModel()
                          {
                              Id = b.Id.ToString(),
                              Name = u.UserProfiles.FirstName + ' ' + u.UserProfiles.LastName,
                              Email = u.Email,
                              CompanyName = u.CompanyProfiles.CompanyName,
                              Products = (int)b.Products.Count(),
                              Tags = b.Tags,
                              IsActive = b.IsActive,
                              CreatedBy = b.CreatedBy,
                              CreatedDate = b.CreatedDate,
                              FormRequest = b.FormRequest.Select(x => new ListSupplierRequestResponse()
                              {
                                  Title = x.Title,
                                  Status = x.Status,
                                  ProductName = b.Products.Where(x1 => x1.Id == new Guid(x.ProductId)).FirstOrDefault() != null ? b.Products.Where(x1 => x1.Id == new Guid(x.ProductId)).FirstOrDefault().Name : "",
                                  CreatedDate = x.CreatedDate,
                                  UpdatedDate = x.UpdatedDate,
                                  FormDesignerResponse = x.FormRequestData.Select(x1 => new FormDesignerResponse()
                                  {
                                      FormId = x1.FormId,
                                      Version = x1.Version,
                                      FormName = company_collection.AsEnumerable().Where(x11 => x11.Id == new Guid(CompanyId)).Select(x2 => x2.FormDesigner.Where(x3 => x3.Id == x1.FormId).FirstOrDefault()).FirstOrDefault() != null ? company_collection.AsEnumerable().Where(x11 => x11.Id == new Guid(CompanyId)).Select(x2 => x2.FormDesigner.Where(x3 => x3.Id == x1.FormId).FirstOrDefault()).FirstOrDefault().Name : ""
                                  }).ToList()
                              }).ToList()
                          });

            #region Filter companies
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
                        case "FormName":
                            {
                                result = result.Select(b => new SupplierResponseModel()
                                {
                                    Id = b.Id,
                                    Name = b.Name,
                                    Email = b.Email,
                                    CompanyName = b.CompanyName,
                                    Products = b.Products,
                                    Tags = b.Tags,
                                    IsActive = b.IsActive,
                                    CreatedBy = b.CreatedBy,
                                    CreatedDate = b.CreatedDate,
                                    FormRequest = b.FormRequest.Select(x => new ListSupplierRequestResponse()
                                    {
                                        Title = x.Title,
                                        Status = x.Status,
                                        ProductName = x.ProductName,
                                        UpdatedDate = x.UpdatedDate,
                                        FormDesignerResponse = x.FormDesignerResponse.Where(y => _filterValues.Where(z => y.FormName.ToLower().Contains(z.ToLower())).Count() > 0).ToList(),
                                    }).ToList()
                                });
                                result = result.Select(t => new SupplierResponseModel()
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                    Email = t.Email,
                                    CompanyName = t.CompanyName,
                                    Products = t.Products,
                                    Tags = t.Tags,
                                    IsActive = t.IsActive,
                                    CreatedBy = t.CreatedBy,
                                    CreatedDate = t.CreatedDate,
                                    FormRequest = t.FormRequest.Where(x => x.FormDesignerResponse.Count() > 0).ToList()
                                });
                                result = result.Where(x => x.FormRequest.Count() > 0);
                                break;
                            }
                        case "Status":
                            {
                                result = result.Select(t => new SupplierResponseModel()
                                {
                                    Id = t.Id,
                                    Name = t.Name,
                                    Email = t.Email,
                                    CompanyName = t.CompanyName,
                                    Products = t.Products,
                                    Tags = t.Tags,
                                    IsActive = t.IsActive,
                                    CreatedBy = t.CreatedBy,
                                    CreatedDate = t.CreatedDate,
                                    FormRequest = t.FormRequest.Where(a => _filterValues.Where(z=>(int)a.Status == Convert.ToInt16(z)).Count()>0).ToList(),
                                }).ToList();
                                result = result.Where(x => x.FormRequest.Count() > 0);
                                break;
                            }

                        default:
                            break;
                    }
                }
            }
            #endregion

            #region Sort companies
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

            return new PagedResult<SupplierResponseModel>(result.ToList(), model.pageNumber ?? 0, model.pageSize ?? totalpage, totalpage);

        }

        public List<ListSupplierResponseModel> GetSupplierCompanyName(string CompanyId, string BuyerId, string name)
        {
            var user_collection = _unitOfWork.accountRepository.QueryAll();
            var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();

            var result = (from u in user_collection.AsEnumerable()
                          join b in supplier_collection.AsEnumerable() on u.Id.ToString() equals b.UserId
                          where (b.CompanyId == CompanyId && b.Products.Count() > 0)
                          select new ListSupplierResponseModel()
                          {
                              Id = b.Id.ToString(),
                              SupplierEmail = u.Email,
                              CompanyName = u.CompanyProfiles.CompanyName,
                              FullName = u.UserProfiles.FirstName + " " + u.UserProfiles.LastName
                          });
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(x => x.CompanyName.ToLower().Contains(name.ToLower()));
            }
            return result.OrderBy(x => x.CompanyName).ThenBy(x => x.SupplierEmail).ToList();
        }

        public async Task<CommitResult> RemoveTags(string supplierId, string tagsName)
        {
            var query = _unitOfWork.suppliersRepository.GetWhere(x => x.Id == new Guid(supplierId) && x.Tags.Contains(tagsName)).FirstOrDefault();
            if (query != null)
            {
                query.Tags.Remove(tagsName);
                _unitOfWork.suppliersRepository.Update(query);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, query);
            }
            return new CommitResult(false);

            //UpdateResult actionResult = await _context.GetCollection<Suppliers>("Suppliers").UpdateOneAsync(Builders<Suppliers>.Filter.Where(w => w.Id == new Guid(supplierId)), Builders<Suppliers>.Update.Pull(x => x.Tags, tagsName));
            //return new CommitResult(actionResult.IsAcknowledged && actionResult.ModifiedCount > 0);
        }

        public async Task<CommitResult> CreateProduct(string supplierId, Products item)
        {
            try
            {
                var supplier = _unitOfWork.suppliersRepository.GetWhere(x => x.Id == new Guid(supplierId)).FirstOrDefault();
                if (supplier.Products != null)
                {
                    supplier.Products.Add(item);
                }
                else
                {
                    supplier.Products = new List<Products>();
                    supplier.Products.Add(item);
                }

                _unitOfWork.suppliersRepository.Update(supplier);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, supplier.Products);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "CreateProduct: ", ex.Message);
                throw;
            }
        }
        public FormRequestDetailResponseModel GetFormRequestDetailById(string supplierId, Guid formRequestId)
        {
            var companyid = _unitOfWork.suppliersRepository.GetWhere(x => x.Id == new Guid(supplierId)).FirstOrDefault().CompanyId;
            var company_collection = _unitOfWork.companyRepository.QueryAll();
            var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
            var _list_formsrequest = supplier_collection.Where(x => x.Id == new Guid(supplierId)).SelectMany(x => x.FormRequest);
            var formsrequest = _list_formsrequest.Where(x => x.Id == formRequestId).FirstOrDefault();
            if (formsrequest != null)
            {
                var result = new FormRequestDetailResponseModel()
                {
                    Id = formsrequest.Id,
                    Title = formsrequest.Title,
                    Status = formsrequest.Status,
                    CreatedDate = formsrequest.CreatedDate,
                    DateUpdated = formsrequest.UpdatedDate,
                    CreatedBy = formsrequest.CreatedBy,
                    Comments = formsrequest.Comments,
                    Products = supplier_collection.AsEnumerable().Where(x => x.Id == new Guid(supplierId)).Select(x1 => x1.Products.Where(x11 => x11.Id == new Guid(formsrequest.ProductId)).FirstOrDefault()),
                    FormRequestDataDetail = formsrequest.FormRequestData.Select(x1 => new FormRequestDataDetail()
                    {
                        FormId = x1.FormId,
                        Version = x1.Version,
                        SurveyResult = x1.SurveyResult,
                        SurveyDesigner = company_collection.AsEnumerable().Where(x11 => x11.Id == new Guid(companyid)).Select(x2 => x2.FormDesigner.Where(x3 => x3.Id == x1.FormId).FirstOrDefault()).FirstOrDefault().FormDesignerData.Where(xx1 => xx1.Version == 1).FirstOrDefault().SurveyDesigner
                    }).ToList()
                };
                return result;
            }
            return null;
        }

        public async Task<CommitResult> UpdateFormRequest(string supplierId, string formrequestId, FormRequestParam param)
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
                    if (param.FormRequestDataParam != null && param.FormRequestDataParam.Count() > 0)
                    {
                        foreach (var item in param.FormRequestDataParam)
                        {
                            var _formrequest_data = _formrequest.FormRequestData.Where(x => x.FormId == new Guid(item.FormId)).FirstOrDefault();
                            if (_formrequest_data != null)
                            {
                                var serveyJson = JsonConvert.SerializeObject(item.SurveyResult);
                                _formrequest_data.SurveyResult = serveyJson;
                            }
                        }
                        _formrequest.Status = RequestStatus.inprogress;
                        _formrequest.UpdatedDate = DateTime.Now;
                        _formrequest.UpdatedBy = supplier.UserId;
                        supplier.FormRequestTimeline = new List<FormRequestTimeline>() { new FormRequestTimeline { FormRequestId = formrequestId, Status = RequestStatus.inprogress, UpdatedBy = supplier.UserId, UpdatedDate = DateTime.Now } };
                        _unitOfWork.suppliersRepository.Update(supplier);

                        var result = await _unitOfWork.CommitAsync();
                        return new CommitResult(result, _formrequest);
                    }
                }
                return new CommitResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "UpdateFormRequest: ", ex.Message);
                throw ex;
            }
        }

        public async Task<CommitResult> UpdateCompleteFormRequest(string supplierId, string formrequestId)
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
                    _formrequest.Status = RequestStatus.completed;
                    _formrequest.UpdatedDate = DateTime.Now;
                    _formrequest.UpdatedBy = supplier.UserId;
                    supplier.FormRequestTimeline = new List<FormRequestTimeline>() { new FormRequestTimeline { FormRequestId = formrequestId, Status = RequestStatus.completed, UpdatedBy = supplier.UserId, UpdatedDate = DateTime.Now } };
                    _unitOfWork.suppliersRepository.Update(supplier);
                    var result = await _unitOfWork.CommitAsync();
                    return new CommitResult(result, _formrequest);
                }
                return new CommitResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "UpdateCompleteFormRequest: ", ex.Message);
                throw ex;
            }
        }

        public async Task<CommitResult> AddCommentToFormRequest(string current_user, string supplierId, string formrequestId, FormRequestCommentParam param)
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
                    if (!string.IsNullOrEmpty(param.Comments))
                    {
                        var _comments = new Comments()
                        {
                            Message = param.Comments,
                            CreatedBy = current_user,
                            CreatedDate = DateTime.Now
                        };
                        _formrequest.Comments.Add(_comments);

                        _unitOfWork.suppliersRepository.Update(supplier);
                        var result = await _unitOfWork.CommitAsync();
                        return new CommitResult(result, _formrequest);
                    }
                }
                return new CommitResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "AddCommentToFormRequest: ", ex.Message);
                throw ex;
            }
        }
        public List<CommentsResponseModel> GetAllCommentOfFormRequestByID(string supplierId, string formrequestId, CommentsSortByParam param)
        {
            var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
            var user_collection = _unitOfWork.accountRepository.QueryAll();

            var result = (from u in supplier_collection.AsEnumerable()
                          where (u.Id == new Guid(supplierId))
                          from f in u.FormRequest.AsEnumerable().Where(x => x.Id == new Guid(formrequestId)).SelectMany(x => x.Comments)
                          select new CommentsResponseModel()
                          {
                              Id = f.Id,
                              Message = f.Message,
                              CreatedDate = f.CreatedDate,
                              CreatedBy = user_collection.AsEnumerable().Where(x11 => x11.Id == new Guid(f.CreatedBy)).Select(x2 => x2.UserProfiles).Select(xx => new { Fullname = xx.FirstName + " " + xx.LastName }).FirstOrDefault() != null ? user_collection.AsEnumerable().Where(x11 => x11.Id == new Guid(f.CreatedBy)).Select(x2 => x2.UserProfiles).Select(xx => new { Fullname = xx.FirstName + " " + xx.LastName }).FirstOrDefault().Fullname : "",
                          });

            if (param.SortType == SortType.Ascending)
            {
                result = result.OrderBy(x => x.GetType().GetProperty("CreatedDate").GetValue(x, null));
            }
            else
            {
                result = result.OrderByDescending(x => x.GetType().GetProperty("CreatedDate").GetValue(x, null));
            }

            return result.ToList();
        }

        public PagedResult<ProductResponseModel> GetAllProductBySuppliers(string supplierId, GetFilterParams model)
        {
            try
            {
                bool SortOnValid = false;
                if (!string.IsNullOrEmpty(model.SortOn))
                {
                    foreach (PropertyInfo p in typeof(ProductResponseModel).GetProperties())
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
                            foreach (PropertyInfo p in typeof(ProductResponseModel).GetProperties())
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

                //GET product by supplierID
                var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
                var _result = from sup in supplier_collection.AsEnumerable()
                              where sup.Id == new Guid(supplierId)
                              select sup;
                IEnumerable<ProductResponseModel> result = null;
                foreach (var item in _result)
                {
                    result = from s in item.Products
                             select new ProductResponseModel
                             {
                                 Name = s.Name,
                                 Tags = s.Tags,
                                 Code = s.Code,
                                 Id = s.Id,
                                 Description = s.Description,
                                 CreatedDate = s.CreatedDate,
                                 Grade = s.Grade
                             };
                }
                #region Filter
                if (filterValid && model.FilterValue != null)
                {
                    for (int i = 0; i < model.FilterValue.Count(); i++)
                    {
                        string _filterBy = model.FilterBy[i];
                        List<string> _filterValues = model.FilterValue[i].Where(x => x != null).ToList();
                        switch (_filterBy)
                        {
                            case "Name":
                                var _result_filtername = result.Where(x => x.Name != null ? _filterValues.Where(z => x.Name.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                if (_result_filtername.Count() <= 0)
                                {
                                    goto case "Tags";
                                }
                                result = _result_filtername;
                                break;
                            case "Tags":
                                result = result.Where(x => x.Tags != null ? _filterValues.Where(z => x.Tags.Any(s => s.ToLower().Contains(z.ToLower()))).Count() > 0 : false);
                                break;
                            default:
                                break;
                        }
                        break; // exit for
                    }
                }
                #endregion
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

                return new PagedResult<ProductResponseModel>(result.ToList(), model.pageNumber ?? 0, model.pageSize ?? totalpage, totalpage);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ProductResponseModel GetProductDetail(string productId)
        {
            try
            {
                ProductResponseModel product = new ProductResponseModel();
                var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
                product = (from s in supplier_collection.ToList()
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
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "GetProductDetail: ", ex.Message);
                throw;
            }
        }

        public async Task<CommitResult> EditProductbyId(string id, Products product)
        {
            try
            {
                var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
                var supplier = from x in supplier_collection select x;
                Products product_edit = null;
                bool result = false;

                foreach (var item in supplier)
                {
                    product_edit = item.Products.Where(x => x.Id == new Guid(id)).FirstOrDefault();
                    if (product_edit == null)
                    {
                        result = false;
                        return new CommitResult(result, product_edit);
                    }
                    product_edit.Grade = product.Grade;
                    product_edit.Description = product.Description;
                    product_edit.Tags = product.Tags;
                    product_edit.Name = product.Name;
                    product_edit.Code = product.Code;
                    _unitOfWork.suppliersRepository.Update(item);
                    result = await _unitOfWork.CommitAsync();
                    return new CommitResult(result, product_edit);
                }
                return new CommitResult(result, product_edit);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "EditProductbyId: ", ex.Message);
                throw;
            }
        }

        public PagedResult<RequestFormResponModel> GetRequestFormById(string id, GetFilterParams model)
        {
            try
            {
                bool SortOnValid = false;
                if (!string.IsNullOrEmpty(model.SortOn))
                {
                    foreach (PropertyInfo p in typeof(RequestFormResponModel).GetProperties())
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
                            foreach (PropertyInfo p in typeof(RequestFormResponModel).GetProperties())
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


                var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
                var account_collection = _unitOfWork.accountRepository.QueryAll();
                var company_collection = _unitOfWork.companyRepository.QueryAll();
                var form = (from s in supplier_collection.AsEnumerable()
                            from x in s.FormRequest
                            from z in x.FormRequestData
                            where x.ProductId == id
                            select new
                            {
                                Id = x.Id, //ID form request
                                Title = x.Title, //Name form request
                                CreatedDate = x.CreatedDate, // create date form request
                                CreateBy = x.CreatedBy, //ID user create 
                                Status = x.Status, // Status
                                FullName = account_collection.AsEnumerable().Where(x => x.Id == new Guid(s.CreatedBy))
                                                                            .Select(a1 => a1.UserProfiles.FirstName + " " + a1.UserProfiles.LastName)
                                                                            .FirstOrDefault(), //Full Name get collection Users by (IDUserCreate == Iduser) 
                                FormDate = z.FormId //ID form requestData
                            });

                var result = (
                              from cre in form
                              from company in company_collection
                              from formdata in company.FormDesigner
                              where formdata.Id == cre.FormDate  //compare IdFormrequestData == IDformdesignerData (into Company)
                              select new RequestFormResponModel
                              {
                                  FullName = cre.FullName,
                                  Title = cre.Title,
                                  Status = cre.Status,
                                  CreatedDate = cre.CreatedDate,
                                  CreatedBy = cre.CreateBy,
                                  Id = cre.Id,
                                  Tags = formdata.Tags //Tags formdesignerData (into Company)
                              });

                if (filterValid && model.FilterValue != null)
                {
                    for (int i = 0; i < model.FilterValue.Count(); i++)
                    {
                        string _filterBy = model.FilterBy[i];
                        List<string> _filterValues = model.FilterValue[i].Where(x => x != null).ToList();
                        switch (_filterBy)
                        {
                            case "Title":
                                result = result.Where(x => x.Title != null ? _filterValues.Where(z => x.Title.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                break;
                            default:
                                break;
                        }
                    }
                }
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

                return new PagedResult<RequestFormResponModel>(result.ToList(), model.pageNumber ?? 0, model.pageSize ?? totalpage, totalpage);

            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "GetRequestFormById: ", ex.Message);
                throw;
            }
        }
    }
}
