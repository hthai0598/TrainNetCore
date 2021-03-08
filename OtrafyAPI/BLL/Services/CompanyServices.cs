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
using Microsoft.Extensions.Logging;
using BLL.Helpers;

namespace BLL.Services
{
    public class CompanyServices : ICompanyServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CompanyServices> _logger;
        private const string _prefixLog = "CompanyServices->";

        public CompanyServices(IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<CompanyServices> logger)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CompanyResponseModel> GetCompanyDetailByUserCreated(string UserId, string CompanyId)
        {
            var supplier_collection = _unitOfWork.suppliersRepository.QueryAll();
            var totalbuyercreated = await _unitOfWork.buyersRepository.GetListAsync(x => x.CompanyId == CompanyId);
            var company = await _unitOfWork.companyRepository.GetSingleAsync(x => x.Id == new Guid(CompanyId) && x.UserCreated == UserId);
            if (company != null)
            {
                CompanyResponseModel comp = new CompanyResponseModel()
                {
                    CompanyId = company.Id.ToString(),
                    Name = company.Name,
                    Avatar = company.Avatar,
                    Address = company.Address,
                    Email = company.Email,
                    Phone = company.Phone,
                    Website = company.Website,
                    IsActive = company.IsActive,
                    MaxNumberBuyersAllowed = company.MaxNumberBuyersAllowed,
                    TotalBuyersCreated = totalbuyercreated.Count(),
                    MaxNumberSuppliersAllowed = company.MaxNumberSuppliersAllowed,
                    TotalSuppliersInvited = supplier_collection.Where(x => x.CompanyId == company.Id.ToString()).AsEnumerable().Count(),
                    MaxNumberFormsAllowed = company.MaxNumberFormsAllowed,
                    TotalFormsCreated = company.FormDesigner.Count(),
                    DateCreated = company.DateCreated
                };
                return comp;
            }
            return null;
        }

        public async Task<CommitResult> AddCompany(Company item)
        {
            try
            {
                _unitOfWork.companyRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, item);

            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "AddCompany: ", ex.Message);
                throw;
            }
        }

        public async Task<CommitResult> UpdateCompany(string id, Company model)
        {
            try
            {
                var item = await _unitOfWork.companyRepository.GetById(new Guid(id));
                if (item == null)
                {
                    return new CommitResult(false);
                }
                item.Name = model.Name;
                item.Avatar = model.Avatar;
                item.Address = model.Address;
                item.Email = model.Email;
                item.Phone = model.Phone;
                item.Website = model.Website;
                item.IsActive = model.IsActive;
                item.MaxNumberBuyersAllowed = model.MaxNumberBuyersAllowed;
                item.MaxNumberSuppliersAllowed = model.MaxNumberSuppliersAllowed;
                item.MaxNumberFormsAllowed = model.MaxNumberFormsAllowed;

                _unitOfWork.companyRepository.Update(item);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "UpdateCompany: ", ex.Message);
                throw ex;
            }
        }

        public async Task<bool> DeleteCompany(string CompanyId)
        {
            try
            {
                var item = await _unitOfWork.companyRepository.GetById(new Guid(CompanyId));
                if (item != null)
                {
                    _unitOfWork.companyRepository.Remove(new Guid(CompanyId));
                    return await _unitOfWork.CommitAsync();
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "DeleteCompany: ", ex.Message);
                throw ex;
            }
        }

        public PagedResult<CompanyResponseModel> GetAllCompanyByUser(string UserId, GetFilterParams model)
        {
            bool SortOnValid = false;
            if (!string.IsNullOrEmpty(model.SortOn))
            {
                foreach (PropertyInfo p in typeof(CompanyResponseModel).GetProperties())
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
                        foreach (PropertyInfo p in typeof(CompanyResponseModel).GetProperties())
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
            var result = _unitOfWork.companyRepository.QueryAll().AsEnumerable()
                         .Where(c => c.UserCreated == UserId)
                         .Select(c => new CompanyResponseModel()
                         {
                             CompanyId = c.Id.ToString(),
                             Name = c.Name,
                             Avatar = c.Avatar,
                             Address = c.Address,
                             Email = c.Email,
                             Phone = c.Phone,
                             Website = c.Website,
                             IsActive = c.IsActive,
                             MaxNumberBuyersAllowed = c.MaxNumberBuyersAllowed,
                             TotalBuyersCreated = _unitOfWork.buyersRepository.GetWhere(x => x.CompanyId == c.Id.ToString()).AsEnumerable().Count(),
                             MaxNumberSuppliersAllowed = c.MaxNumberSuppliersAllowed,
                             TotalSuppliersInvited = supplier_collection.Where(x => x.CompanyId == c.Id.ToString()).AsEnumerable().Count(),
                             MaxNumberFormsAllowed = c.MaxNumberFormsAllowed,
                             TotalFormsCreated = c.FormDesigner.Count(),
                             DateCreated = c.DateCreated
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
                        case "CompanyId":
                            {
                                result = result.Where(x => x.CompanyId != null ? _filterValues.Where(z => x.CompanyId.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                break;
                            }
                        case "Email":
                            {
                                result = result.Where(x => x.Email != null ? _filterValues.Where(z => x.Email.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                break;
                            }
                        case "Phone":
                            {
                                result = result.Where(x => x.Phone != null ? _filterValues.Where(z => x.Phone.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                break;
                            }
                        case "Name":
                            {
                                result = result.Where(x => x.Name != null ? _filterValues.Where(z => x.Name.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                break;
                            }
                        case "Address":
                            {
                                result = result.Where(x => x.Address != null ? _filterValues.Where(z => x.Address.ToLower().Contains(z.ToLower())).Count() > 0 : false);
                                break;
                            }

                        case "DateCreated":
                            {
                                result = result.Where(x => x.DateCreated != null ? _filterValues.Where(z => x.DateCreated.ToString().ToLower().Contains(z.ToLower())).Count() > 0 : false);
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
                result = result.OrderBy(x => x.DateCreated);
            }

            var totalpage = result.Count();

            if (model.pageNumber != null && model.pageSize != null)
            {
                int skip = ((int)model.pageNumber - 1) * (int)model.pageSize;
                result = result.Skip(skip).Take((int)model.pageSize);
            }
            #endregion

            return new PagedResult<CompanyResponseModel>(result.ToList(), model.pageNumber ?? 0, model.pageSize ?? totalpage, totalpage);

        }

        public async Task<Company> GetCompanyById(Guid id)
        {
            return await _unitOfWork.companyRepository.GetById(id);
        }

        public BuyerDetailResponseModel GetBuyerDetailById(string buyerId)
        {
            var buyer = _unitOfWork.buyersRepository.GetSingle(x => x.Id == new Guid(buyerId));
            if (buyer == null)
            {
                return null;
            }
            var user = _unitOfWork.accountRepository.GetSingle(x => x.Id == new Guid(buyer.UserId));
            var result = new BuyerDetailResponseModel()
            {
                BuyerId = buyer.Id.ToString(),
                Username = user.Username,
                Email = user.Email,
                Permission = buyer.Permission,
                FirstName = user.UserProfiles.FirstName,
                LastName = user.UserProfiles.LastName,
                JobTitle = user.UserProfiles.JobTitle,
                Message = user.UserProfiles.Message
            };
            return result;
        }

        public async Task<CommitResult> UpdateBuyerProfileById(string buyerId, BuyerParams param)
        {
            try
            {
                var buyer = await _unitOfWork.buyersRepository.GetById(new Guid(buyerId));
                if (buyer == null)
                {
                    return new CommitResult(false);
                }

                var user = await _unitOfWork.accountRepository.GetById(new Guid(buyer.UserId));
                if (user == null)
                {
                    return new CommitResult(false);
                }

                user.Email = param.Email;
                user.Username = param.Username;
                user.UserProfiles.FirstName = param.FirstName;
                user.UserProfiles.LastName = param.LastName;
                user.UserProfiles.JobTitle = param.JobTitle;
                _unitOfWork.accountRepository.Update(user);

                // Update Tags of supplier
                buyer.Permission = param.Permission;
                _unitOfWork.buyersRepository.Update(buyer);

                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "UpdateBuyerProfileById: ", ex.Message);
                throw ex;
            }
        }
    }
}
