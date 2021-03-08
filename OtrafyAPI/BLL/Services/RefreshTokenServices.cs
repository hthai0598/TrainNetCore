using BLL.Services.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;

namespace BLL.Services
{
    public class RefreshTokenServices: IRefreshTokenServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public RefreshTokenServices(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
    }
}
