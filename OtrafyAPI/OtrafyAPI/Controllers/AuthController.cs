using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DAL;
using OtrafyAPI.ViewModel;
using OtrafyAPI.Helpers;
using System;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using BLL.Services.Interfaces;
using Microsoft.Extensions.Logging;
using BLL.Helpers;

namespace OtrafyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IJWTTokenServices _jwtTokenService;
        private readonly IAccountServices _accountServices;
        private readonly ILogger _logger;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration, IJWTTokenServices jwtTokenService, IAccountServices accountServices, ILogger<AuthController> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
            _accountServices = accountServices;
            _logger = logger;
        }

        /// <summary>
        /// General token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("token")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            try
            {
                if (model.Username.IndexOf('@') > -1)
                {
                    //Validate email format
                    string emailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
                    Regex re = new Regex(emailRegex);
                    if (!re.IsMatch(model.Username))
                    {
                        ModelState.AddModelError("Email", "Email is not valid");
                    }
                }
                else
                {
                    //validate Username format
                    string emailRegex = @"^[a-zA-Z0-9]+([_ . -]?[a-zA-Z0-9])*$";
                    Regex re = new Regex(emailRegex);
                    if (!re.IsMatch(model.Username))
                    {
                        ModelState.AddModelError("Email", "Username is not valid");
                    }
                }
                if (ModelState.IsValid)
                {
                    var user = new User();
                    if (model.Username.IndexOf('@') > -1)
                    {
                        user = await _accountServices.FindByEmail(model.Username);
                    }
                    else
                    {
                        user = await _unitOfWork.authRepository.GetSingleAsync(x => x.Username == model.Username);
                    }
                    if (user == null)
                        return new ResponseResult(HttpStatusCode.BadRequest, "Invalid username or password");

                    if (!Utilities.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                        return new ResponseResult(HttpStatusCode.BadRequest, "Invalid username or password");


                    if (!user.IsEnabled)
                    {
                        return new ResponseResult(HttpStatusCode.Forbidden, "Account Suspended");
                    }

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
                    return new ResponseResult(HttpStatusCode.OK, response);
                }
                return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API CreateToken: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }            
        }


        /// <summary>
        /// Refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel refreshToken)
        {
            var refreshTokenFromDatabase = await _unitOfWork.refreshTokenRepository.GetSingleAsync(x => x.Token == refreshToken.RefreshToken);

            if (refreshTokenFromDatabase == null)
                return new ResponseResult(HttpStatusCode.NotFound, "Refresh token not found");

            if (refreshTokenFromDatabase.ExpiresUtc < DateTime.Now.ToUniversalTime())
                return new ResponseResult(HttpStatusCode.Unauthorized);

            var user = await _unitOfWork.authRepository.GetById(refreshTokenFromDatabase.UserId);
            if (user == null)
                return new ResponseResult(HttpStatusCode.NotFound, "User not found");

            var response = _jwtTokenService.GeneralJWTToken(refreshTokenFromDatabase.Token, user);
                       
            return new ResponseResult(HttpStatusCode.OK, response);

        }

    }
}