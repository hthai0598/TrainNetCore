using System;
using System.Threading.Tasks;
using DAL;
using DAL.Context;
using DAL.Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Dapper.EF.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        IDapperRepository dapper;
        IUnitOfWork unitOfWork;
        public BaseController(IDapperRepository dapper, IUnitOfWork unitOfWork)
        {
            this.dapper = dapper;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await unitOfWork.studentRepo.GetAll());
        }
    }
}
