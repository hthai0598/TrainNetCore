using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using OtrafyAPI.ViewModel;
using System;
using System.Threading.Tasks;
using DAL;
using OtrafyAPI.Helpers;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using DAL.Core;
using BLL.Services.Interfaces;
using BLL.Models;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BLL.Helpers;

namespace OtrafyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendGridSender _sendGridSender;
        private readonly IEmailTemplates _emailTemplates;
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
        private readonly IAccountServices _accountServices;
        private readonly ILogger _logger;

        public AccountController(IUnitOfWork unitOfWork, ISendGridSender sendGridSender, IEmailTemplates emailTemplates, IConfiguration configuration, ITokenServices tokenServices, IAccountServices accountServices, ILogger<AccountController> logger)
        {
            _unitOfWork = unitOfWork;
            _sendGridSender = sendGridSender;
            _emailTemplates = emailTemplates;
            _configuration = configuration;
            _tokenServices = tokenServices;
            _accountServices = accountServices;
            _logger = logger;
        }

        /// <summary>
        /// Create User (role administrator)
        /// </summary>
        /// <remarks>
        /// ### Descriptions  ###
        /// - **role**: 0: administrator; 1: buyers; 2: suppliers
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeRolesAttribute(Role.administrator)]
        [Route("create")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterParams value)
        {

            //Validate email format
            string emailRegexEmail = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            Regex re = new Regex(emailRegexEmail);
            if (!re.IsMatch(value.Email))
            {
                ModelState.AddModelError("Email", "Email is not valid");
            }

            //validate Username format
            string emailRegex = @"^[a-zA-Z0-9]+([_ -]?[a-zA-Z0-9])*$";
            Regex re1 = new Regex(emailRegex);
            if (!re1.IsMatch(value.Username))
            {
                ModelState.AddModelError("Email", "Username is not valid");
            }


            if (string.IsNullOrWhiteSpace(value.Password))
                ModelState.AddModelError("Password", "Password is required");
            if (await _unitOfWork.authRepository.CountAsync(Builders<User>.Filter.Eq(_ => _.Username, value.Username)) > 0 || await _unitOfWork.authRepository.CountAsync(Builders<User>.Filter.Eq(_ => _.Email, value.Email)) > 0)
                ModelState.AddModelError("Username", "user name exist is required");

            if (!ModelState.IsValid)
            {
                return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                byte[] passwordHash, passwordSalt;
                Utilities.CreatePasswordHash(value.Password, out passwordHash, out passwordSalt);

                await _accountServices.CreateUser(new User
                {
                    Username = value.Username,
                    Email = value.Email,
                    IsEmailConfirmed = value.IsEmailConfirmed,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    IsEnabled = value.IsEnabled,
                    Role = value.Role.ToString(),
                    UserProfiles = new UserProfiles
                    {
                        FirstName = value.FirstName,
                        LastName = value.LastName,
                        JobTitle = value.JobTitle,
                        Message = ""
                    },
                    CompanyProfiles = new UserCompanyProfiles()
                });
                return new ResponseResult(HttpStatusCode.OK, "Create Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API CreateUser: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get current user info (role administrator, buyer, supplier)
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserResponseModel>> GetCurrentUser()
        {
            try
            {
                var user = await _accountServices.GetUserInfoById(new Guid(Utilities.GetUserId(this.User)));
                if (user != null)
                {
                    return new ResponseResult(HttpStatusCode.OK, user);
                }
                return new ResponseResult(HttpStatusCode.BadRequest, "Error");

            }
            catch (Exception ex)
            {
                _logger.LogError("Error API GetCurrentUser: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Update user profile (role administrator, buyer, supplier)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody]UserProfilesParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _unitOfWork.accountRepository.GetById(new Guid(Utilities.GetUserId(this.User)));
                    if (user == null)
                    {
                        return new ResponseResult(HttpStatusCode.NotFound, "User not found");
                    }
                    var result = await _accountServices.UpdateProfile(user.Id.ToString(), param);
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, "Update succesfull");
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Error");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API UpdateProfile: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Update user profile avatar (role administrator, buyer, supplier)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload-profile-picture"), DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture([FromForm]IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new ResponseResult(HttpStatusCode.BadRequest, "File Error");

                var folderName = Path.Combine("resources", "images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var uniqueFileName = $"{Utilities.GetUserId(this.User)}_profilepic.png";
                var fullPath = Path.Combine(pathToSave, uniqueFileName);
                var dbPath = Path.Combine(folderName, uniqueFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var fullurl = _configuration["AppSettings:Backend_Url"] + "/resources/images/" + uniqueFileName;

                var user = await _unitOfWork.accountRepository.GetSingleAsync(x => x.Id == new Guid(Utilities.GetUserId(this.User)));
                if (user != null)
                {
                    user.UserProfiles.Avatar = uniqueFileName;
                    _unitOfWork.accountRepository.Update(user);
                    await _unitOfWork.CommitAsync();
                }

                return new ResponseResult(HttpStatusCode.OK, new { fullurl });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API UploadProfilePicture: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountServices.FindByEmail(model.Email);
                if (user == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "User not found");
                }

                if (!(await _accountServices.IsEmailConfirmed(user.Id)))
                {
                    return new ResponseResult(HttpStatusCode.NotImplemented, "Email is not confirmed");
                }

                var code = _tokenServices.GenerateToken(new Token
                {
                    ValidUntil = DateTime.UtcNow.AddHours(Int32.Parse(_configuration["TokenLifespan:ResetPassword"])),
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id.ToString(),
                    IssuerName = user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName,
                    IssuerEmail = user.Email,
                    IssueType = IssuerType.ForgotPassword,
                    IssuerSubject = "Send forgot password",
                    SentDate = DateTime.Now
                });

                string forgotpass_url = _configuration["AppSettings:Frontend_Url"] + "/reset-password/" + code;
                string message = _emailTemplates.GetForgotPasswordEmailTemplate(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, model.Email, forgotpass_url);
                await _sendGridSender.SendEmailAsync(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, model.Email, "Reset your password", message);
                _logger.LogInformation("Send email link change password  to the user (" + user.Email + ")");

                return new ResponseResult(HttpStatusCode.OK, "Send forgot password success");
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Check token is valid?
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("valid-token")]
        public IActionResult ValidToken([FromBody]ValidTokenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                bool validToken = _tokenServices.VerifyIsValidToken(model.TokenId);

                if (!validToken)
                {
                    return new ResponseResult(HttpStatusCode.BadRequest, "Token invalid");
                }
                return new ResponseResult(HttpStatusCode.OK, "Token is Valid");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error API ValidToken: " + ex.Message);
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }


        /// <summary>
        /// Active invite
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("active-invite")]
        public async Task<IActionResult> ActiveInvite([FromBody]ActiveInviteParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = await _tokenServices.GetToken(new Guid(param.TokenId));
                    if (token == null || token.ValidUntil < DateTime.Now.ToUniversalTime())
                    {
                        return new ResponseResult(HttpStatusCode.BadRequest, "Token is not Valid");
                    }

                    var result = await _accountServices.ActiveInvite(param);
                    if (result.Success)
                    {
                        return new ResponseResult(HttpStatusCode.OK, result.Item);
                    }
                    return new ResponseResult(HttpStatusCode.BadRequest, "Token is not Valid");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error API ActiveInvite: " + ex.Message);
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }


        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _tokenServices.GetToken(new Guid(model.TokenId));
                if (token == null || token.ValidUntil < DateTime.Now.ToUniversalTime())
                {
                    return new ResponseResult(HttpStatusCode.BadRequest, "Token is not Valid");
                }

                var user = await _unitOfWork.accountRepository.GetById(new Guid(token.CreatedBy));
                if (user == null)
                {
                    return new ResponseResult(HttpStatusCode.NotFound, "User Not Found");
                }

                byte[] passwordHash, passwordSalt;
                Utilities.CreatePasswordHash(model.NewPassword, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                var result = await _accountServices.UpdateUser(user.Id.ToString(), user);
                if (result)
                {
                    // Send a notification email to the user that the password has been changed
                    string message = _emailTemplates.GetChangePasswordAlertEmailTemplate(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName);
                    await _sendGridSender.SendEmailAsync(user.UserProfiles.FirstName + ' ' + user.UserProfiles.LastName, user.Email, "Notice: Password Change Successful", message);
                    _logger.LogInformation("Send a notification email to the user (" + user.Email + ") that the password has been changed");
                    return new ResponseResult(HttpStatusCode.OK, "Reset password success");
                }
                return new ResponseResult(HttpStatusCode.BadRequest, "Token is not Valid");
            }
            return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
        }        
    }
}