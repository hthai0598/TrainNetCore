using BLL.Services.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;

namespace BLL.Services
{
    public class AuthServices : IAuthServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthServices(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
    }
}
