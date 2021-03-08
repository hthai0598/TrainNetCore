using BLL.Services.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using DAL.Models;
using BLL.JwtHelpers;
using System.Security.Claims;
using System;
using BLL.Models;
using DAL.Core;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class JWTTokenServices : IJWTTokenServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IJWTTokenServices> _logger;
        private const string _prefixLog = "IJWTTokenServices->";

        public JWTTokenServices(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<IJWTTokenServices> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
        }

        public TokenResponseModel GeneralJWTToken(string refreshtoken, User model)
        {
            try
            {
                var companyid = "";
                var buyerid = "";
                List<BuyerPermission> buyerPermissions = new List<BuyerPermission>();
                if (model.Role == Role.buyers.ToString())
                {
                    var buyer = _unitOfWork.buyersRepository.GetWhere(x => x.UserId == model.Id.ToString()).FirstOrDefault();
                    buyerid = buyer.Id.ToString();
                    companyid = buyer.CompanyId;
                    buyerPermissions = buyer.Permission;
                }

                var supplierid = "";
                if (model.Role == Role.suppliers.ToString())
                {
                    var supplier = _unitOfWork.suppliersRepository.GetWhere(x => x.UserId == model.Id.ToString()).FirstOrDefault();
                    supplierid = supplier.Id.ToString();
                    companyid = supplier.CompanyId;
                }

                var token = new JwtTokenBuilder()
                                       .AddSecurityKey(JwtSecurityKey.Create(_configuration["Authentication:SecurityKey"]))
                                       .AddSubject(model.Username)
                                       .AddIssuer(_configuration["Authentication:Issuer"])
                                       .AddAudience(_configuration["Authentication:Audience"])
                                       .AddClaim(ClaimTypes.Name, model.Id.ToString())
                                       .AddClaim(ClaimTypes.Email, model.Email)
                                       .AddClaim("CompanyId", companyid)
                                       .AddClaim("BuyerId", buyerid)
                                       .AddClaim("SupplierId", supplierid)
                                       .AddRole(model.Role)
                                       .AddExpiryInMinutes(Int32.Parse(_configuration["TokenLifespan:JWTExpired"]))
                                       .Build();

                model.JWTToken = token.Value;
                _unitOfWork.authRepository.Update(model);
                _unitOfWork.Commit();

                var response = new TokenResponseModel
                {
                    AccessToken = token.Value,
                    RefreshToken = refreshtoken,
                    FirstName = model.UserProfiles.FirstName,
                    LastName = model.UserProfiles.LastName,
                    Username = model.Username,
                    Email = model.Email,
                    Role = model.Role,
                    Permissions = buyerPermissions,
                    CompanyId = companyid,
                    //BuyerId = buyerid,
                    //SupplierId = supplierid,
                    TokenExpiration = token.ValidTo
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "GeneralJWTToken: ", ex.Message);
                throw;
            }
        }
    }
}
