using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using DAL;
using OtrafyAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using DAL.Core;
using BLL.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.WindowsAzure.Storage.Blob;
using BLL.Models;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.Helpers;

namespace OtrafyAPI.Controllers
{
    [Route("api/system")]
    [AllowAnonymous]
    public class SystemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IAccountServices _accountServices;
        private readonly ILogger _logger;
        private readonly IBlobStorageService _blobStorageService;

        public SystemController(IUnitOfWork unitOfWork, IConfiguration configuration, IAccountServices accountServices, ILogger<SystemController> logger, IBlobStorageService blobStorageService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _accountServices = accountServices;
            _logger = logger;
            _blobStorageService = blobStorageService;
        }

        /// <summary>
        /// Create Database and collection User with defaults user
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        /// <response code="400"></response>
        [HttpGet("{setting}")]
        public async Task<IActionResult> Get(string setting)
        {
            if (setting == "init")
            {

                byte[] passwordHash, passwordSalt;
                Utilities.CreatePasswordHash("bh@132", out passwordHash, out passwordSalt);

               await _accountServices.CreateUser(new User
                {
                    Username = "admin",
                    Email = "admin@bachasoftware.com",
                    IsEmailConfirmed = true,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    IsEnabled = true,
                    Role = Role.administrator.ToString(),
                    UserProfiles = new UserProfiles
                    {
                        FirstName = "Admin",
                        LastName = "Bachasoftware",
                        JobTitle = "Dev",
                        Message = ""
                    },
                    CompanyProfiles = new UserCompanyProfiles()
                });
                return new ResponseResult(HttpStatusCode.BadRequest, "Database " + _configuration["ConnectionStrings:DatabaseName"] + " was created, and collection User was filled with 1 sample items");
            }

            return new ResponseResult(HttpStatusCode.BadRequest, "Error");
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileParam param)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var blobs = await _blobStorageService.UploadFiles<CloudBlockBlob>(param.Files, param.CompanyId);
                    //foreach (var blob in blobs)
                    //{
                    //    var media = new Media();
                    //    media.Title = blob.Name;
                    //    media.PathUrl = blob.StorageUri.PrimaryUri.ToString();
                    //    media.Uploaded = DateTime.Now;
                    //    _context.Media.Add(media);
                    //}
                    //await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new ResponseResult(HttpStatusCode.InternalServerError, ex);
                }
                return new ResponseResult(HttpStatusCode.OK, "Success");
            }
            else
            {
                return new ResponseResult(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpGet("list-files")]
        public async Task<IActionResult> ListFiles([Required] string container)
        {
            try
            {
                var listfile = await _blobStorageService.ListFiles(container);
                return new ResponseResult(HttpStatusCode.OK, listfile);
            }
            catch (Exception ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("delete-file")]
        [HttpGet]
        public async Task<IActionResult> DeleteFile([Required]List<string> fileName, [Required] string container)
        {
            try
            {
                var x = await _blobStorageService.Delete(fileName, container);
                return new ResponseResult(HttpStatusCode.OK, x);
            }
            catch (Exception ex)
            {
                return new ResponseResult(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}