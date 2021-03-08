using BLL.Services.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using BLL.Helpers;
using DAL.Core;
using Microsoft.Extensions.Logging;
using BLL.Models;
using DAL.Models;

namespace BLL.Services
{
    public class AccountServices : IAccountServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
        private readonly IJWTTokenServices _jwtTokenService;
        private readonly ILogger<AccountServices> _logger;
        private const string _prefixLog = "AccountServices->";

        public AccountServices(IConfiguration configuration, IUnitOfWork unitOfWork, ITokenServices tokenServices, IJWTTokenServices jwtTokenService, ILogger<AccountServices> logger)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<User> CreateUser(User item)
        {
            try
            {
                _unitOfWork.accountRepository.Add(item);
                var result = await _unitOfWork.CommitAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "CreateUser: ", ex.Message);
                throw;
            }
        }

        public async Task<UserResponseModel> GetUserInfoById(Guid id)
        {
            try
            {
                var user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Id == id);
                if (user != null)
                {
                    UserResponseModel _user = new UserResponseModel();
                    if (user.Role == Role.buyers.ToString())
                    {
                        var buyer = _unitOfWork.buyersRepository.GetWhere(x => x.UserId == user.Id.ToString()).FirstOrDefault();
                        if (buyer != null)
                        {
                            _user.CompanyId = buyer.Id.ToString();
                        }
                    }
                    _user.Id = user.Id;
                    _user.Username = user.Username;
                    _user.Email = user.Email;
                    _user.IsEmailConfirmed = user.IsEmailConfirmed;
                    _user.IsEnabled = user.IsEnabled;
                    _user.Role = user.Role;
                    _user.UserProfiles = user.UserProfiles;
                    _user.UserProfiles.Avatar = user.UserProfiles.Avatar == null ? _configuration["AppSettings:Backend_Url"] + "/resources/images/default_avatar.png" : _configuration["AppSettings:Backend_Url"] + "/resources/images/" + user.UserProfiles.Avatar;
                    _user.CompanyProfiles = user.CompanyProfiles;

                    return _user;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "GetUserInfoById: ", ex.Message);
                throw;
            }
        }

        public async Task<CommitResult> UpdateProfile(string id, UserProfilesParam param)
        {
            try
            {
                var item = await _unitOfWork.accountRepository.GetById(new Guid(id));
              
                if (item == null)
                {
                    return new CommitResult(false);
                }
                if (!string.IsNullOrEmpty(param.Email) && !string.IsNullOrEmpty(item.Email) && param.Email.Trim().ToLower() != item.Email.Trim().ToLower())
                {
                    var user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Email == param.Email);
                    if (user != null)
                    {
                        return new CommitResult(false).SetMessage("Email already exist");
                    }
                    item.Email = param.Email;
                }
                item.UserProfiles.FirstName = param.FirstName;
                item.UserProfiles.LastName = param.LastName;
                item.UserProfiles.Phone = param.Phone;
                item.UserProfiles.JobTitle = param.JobTitle;
                if (item.CompanyProfiles == null)
                {
                    item.CompanyProfiles = new UserCompanyProfiles()
                    {
                        CompanyName = param.CompanyName,
                        ProductType = param.ProductType,
                        Address = param.Address,
                        Description = param.Description
                    };
                }
                else
                {
                    item.CompanyProfiles.CompanyName = param.CompanyName;
                    item.CompanyProfiles.ProductType = param.ProductType;
                    item.CompanyProfiles.Address = param.Address;
                    item.CompanyProfiles.Description = param.Description;
                }

                _unitOfWork.accountRepository.Update(item);
                var result = await _unitOfWork.CommitAsync();
                return new CommitResult(result, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "UpdateProfile: ", ex.Message);
                throw ex;
            }
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _unitOfWork.accountRepository.GetSingleAsync(x => x.Email == email);
        }

        public async Task<bool> IsEmailConfirmed(Guid Id)
        {
            var user = await _unitOfWork.accountRepository.GetListAsync(x => x.Id == Id && x.IsEmailConfirmed == true);
            return user.Count() > 0;
        }

        public async Task<bool> UpdateUser(string Id, User model)
        {
            try
            {
                var item = await _unitOfWork.accountRepository.GetById(new Guid(Id));
                if (item == null)
                {
                    return false;
                }
                _unitOfWork.accountRepository.Update(model);
                var result = await _unitOfWork.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "UpdateUser: ", ex.Message);
                throw ex;
            }
        }

        public async Task<CommitResult> ActiveInvite(ActiveInviteParam param)
        {
            try
            {
                var token = await _tokenServices.GetToken(new Guid(param.TokenId));
                byte[] passwordHash, passwordSalt;
                Utilities.CreatePasswordHash(param.NewPassword, out passwordHash, out passwordSalt);
                string userid = "";
                if (token.IssueType == IssuerType.InviteBuyer)
                {
                    var buyer = _unitOfWork.buyersRepository.GetWhere(x => x.InviteToken == token.Id.ToString()).FirstOrDefault();
                    if (buyer == null)
                    {
                        return new CommitResult(false);
                    }
                    buyer.isActive = true;
                    _unitOfWork.buyersRepository.Update(buyer);
                    userid = buyer.UserId;

                }

                if (token.IssueType == IssuerType.InviteSupplier)
                {
                    var supplier = _unitOfWork.suppliersRepository.GetWhere(x => x.InviteToken == token.Id.ToString()).FirstOrDefault();
                    if (supplier == null)
                    {
                        return new CommitResult(false);
                    }
                    supplier.IsActive = true;
                    _unitOfWork.suppliersRepository.Update(supplier);
                    userid = supplier.UserId;
                }
                var user = _unitOfWork.accountRepository.GetWhere(x => x.Id == new Guid(userid)).FirstOrDefault();
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.IsEmailConfirmed = true;
                _unitOfWork.accountRepository.Update(user);

                token.ValidUntil = DateTime.UtcNow;
                token.ActivatedDate = DateTime.UtcNow;
                _unitOfWork.tokenRepository.Update(token);

                var result = await _unitOfWork.CommitAsync();
                if (result)
                {
                    var refreshDbToken = await _unitOfWork.refreshTokenRepository.GetSingleAsync(x => x.UserId == user.Id);
                    if (refreshDbToken != null)
                    {
                        _unitOfWork.refreshTokenRepository.Remove(refreshDbToken.Id);
                        await _unitOfWork.CommitAsync();
                    }

                    var newRefreshToken = new RefreshJWTToken
                    {
                        UserId = user.Id,
                        Token = Guid.NewGuid().ToString(),
                        IssuedUtc = DateTime.Now.ToUniversalTime(),
                        ExpiresUtc = DateTime.UtcNow.AddHours(Int32.Parse(_configuration["TokenLifespan:RefreshToken"]))
                    };
                    _unitOfWork.refreshTokenRepository.Add(newRefreshToken);

                    var response = _jwtTokenService.GeneralJWTToken(newRefreshToken.Token, user);
                    return new CommitResult(true, response);
                }
                return new CommitResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "ActiveInvite: ", ex.Message);
                throw ex;
            }
        }
    }
}
