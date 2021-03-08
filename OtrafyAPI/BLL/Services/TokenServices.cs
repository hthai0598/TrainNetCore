using BLL.Services.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using DAL.Models;
using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class TokenServices : ITokenServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenServices> _logger;
        private const string _prefixLog = "TokenServices->";

        public TokenServices(IConfiguration configuration, IUnitOfWork unitOfWork, ILogger<TokenServices> logger)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public string GenerateToken(Token item)
        {
            var token = new Token
            {
                ValidUntil = item.ValidUntil,
                CreatedDate = item.CreatedDate,
                CreatedBy = item.CreatedBy,
                IssuerName = item.IssuerName,
                IssuerEmail = item.IssuerEmail,
                IssueType = item.IssueType,
                IssuerSubject = item.IssuerSubject,
                ActivatedDate = item.ActivatedDate,
                RetryCount = item.RetryCount,
                SentDate = item.SentDate
            };
            _unitOfWork.tokenRepository.Add(token);
            _unitOfWork.Commit();
            return token.Id.ToString();
        }

        public bool VerifyIsValidToken(string Id)
        {
            return _unitOfWork.tokenRepository.GetWhere(x => x.Id == new Guid(Id) && x.ValidUntil > DateTime.Now.ToUniversalTime()).Any();
        }

        public async Task<Token> GetToken(Guid id)
        {
            try
            {
                return await _unitOfWork.tokenRepository.GetSingleAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(_prefixLog + "GetToken: ", ex.Message);
                throw;
            }
        }
    }
}
