using BH_SocialNetwork_Models;
using BH_SocialNetwork_Models.Comman;
using BH_SocialNetwork_Models.Modelss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BH_SocialNetwork_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        BH_SocialNetwork_DBContext db;

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        public UsersController()
        {

            db = new BH_SocialNetwork_DBContext();
        }


        /// <summary>
        /// Tạo tài khoản user
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("createuser")]
        public Result CreateUser([FromBody]Users r)
        {

            Result result = new Result();
            using (db)
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var query = db.Users.Where(x => x.Email == r.Email).FirstOrDefault();
                        //Check xem email có trùng hay không
                        if (query == null)
                        {
                            db.Users.Add(r);
                            db.SaveChanges();


                            //Lấy ra ID mới nhất vừa thêm
                            var _userId = (from user in db.Users select user).Take(1).OrderByDescending(c => c.CreateAt).Select(x => x.UserId).FirstOrDefault();
                            //Thêm người vừa đc tạo vào bảng user follow để chuẩn bị được người khác follow
                            UsersFollowing usersFollowing = new UsersFollowing();
                            usersFollowing.UserId = _userId;
                            db.UsersFollowing.Add(usersFollowing);
                            db.SaveChanges();
                            trans.Commit();
                            result.Success = true;
                            result.Message = "Tạo tài khoản thành công";
                        }
                        else
                        {
                            result.Message = "Email đã tồn tại";
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ ThaiNH";
                        result.Success = false;
                        throw;
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///Đăng nhấp
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public Result Login([FromBody]Users users)
        {
            TokenController tokenController = new TokenController();
            Result result = new Result();
            using (db)
            {
                try
                {
                    if (users != null)
                    {
                        var query = db.Users.Where(x => x.Email == users.Email).Where(y => y.Password == users.Password).FirstOrDefault();
                        if (query != null)
                        {
                            result.Data = tokenController.GenerateToken(query.UserId.ToString());
                            result.Success = true;
                            result.Message = "Đăng nhập thành công";
                        }
                        else
                        {
                            result.Message = "Tài khoản hoặc mật khẩu sai";
                            result.Success = true;
                        }
                    }
                }
                catch (Exception)
                {
                    result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ ThaiNH";
                    result.Success = false;

                    throw;
                }
            }
            return result;
        }


        /// <summary>
        /// Chức năng follow 
        /// </summary>
        /// <param name="id">ID người muốn follow</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("follow")]
        public Result Follow([FromBody] string id)
        {
            Result result = new Result();
            using (db)
            {
                try
                {
                    //Lấy ra mã ID của người dùng trong token
                    var _id = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));

                    //Lấy ra thông tin của bảng userfollow với id = token của UserID đăng nhập hiện tại
                    var _userFollow = db.UsersFollowing.SingleOrDefault(x => x.UserId == Guid.Parse(_id.Value) && x.UserFollow == Guid.Parse(id));
                    if (_userFollow != null)
                    {
                        //Nếu mã người follow != người muốn follow (Tự follow chính mình)
                        if (_id.Value != id)
                        {
                            // Unfollow
                            if (_userFollow.State == true && _userFollow.UserFollow != null && _userFollow.UserFollow == Guid.Parse(id))
                            {
                                _userFollow.State = false;
                                db.SaveChanges();
                                result.Message = "Unfollow thành công";
                            }
                            //follow lại
                            else if (_userFollow.State == false && _userFollow.UserFollow != null && _userFollow.UserFollow == Guid.Parse(id))
                            {
                                _userFollow.UserFollow = Guid.Parse(id);
                                _userFollow.State = true;
                                db.SaveChanges();
                                result.Message = "Follow thành công";
                            }
                            //Chưa từng follow => follow
                            else if (_userFollow.State == false && _userFollow.UserFollow == null)
                            {
                                _userFollow.UserFollow = Guid.Parse(id);
                                _userFollow.State = true;
                                db.SaveChanges();
                                result.Message = "Follow thành công";
                            }
                            //Trường hợp khác
                            else
                            {
                                UsersFollowing usersFollowing = new UsersFollowing();

                                usersFollowing.UserId = Guid.Parse(_id.Value);
                                usersFollowing.UserFollow = Guid.Parse(id);
                                usersFollowing.State = true;

                                db.UsersFollowing.Add(usersFollowing);
                                db.SaveChanges();
                                result.Message = "Follow thành công";
                            }

                            result.Success = true;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Không thể tự follow chính mình";
                        }

                    }
                    else
                    {
                        UsersFollowing usersFollowing = new UsersFollowing();

                        usersFollowing.UserId = Guid.Parse(_id.Value);
                        usersFollowing.UserFollow = Guid.Parse(id);
                        usersFollowing.State = true;

                        db.UsersFollowing.Add(usersFollowing);
                        db.SaveChanges();
                        result.Message = "Follow thành công";
                    }

                   

                }
                catch (Exception)
                {
                    result.Success = false;
                    result.Message = "Đã có lỗi xảy ra vui lòng liên hệ Thai NH";
                    throw;
                }
            }
            return result;
        }


        /// <summary>
        /// Sửa thông tin user đang đăng nhập
        /// </summary>
        /// <param name="users_Edit"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("updateuser")]
        public Result UpdateUser([FromBody]Users users_Edit)
        {
            Result result = new Result();
            using (db)
            {
                try
                {
                    //Lấy ra mã ID của người dùng trong token
                    var _id = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));

                    //Lấy ra user có mã là _id (_id là người đă đăng nhập)
                    var user = db.Users.First(x => x.UserId == Guid.Parse(_id.Value));

                    user.UserName = users_Edit.UserName;
                    user.Password = users_Edit.Password;
                    user.Img = users_Edit.Img;
                    user.UpdateAt = DateTime.Now;
                    db.SaveChanges();
                    result.Success = true;
                    result.Message = "Sửa người dùng thành công";
                }
                catch (Exception)
                {
                    result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ Thai NH";
                    result.Success = false;
                    throw;
                }
            }
            return result;

        }



        /// <summary>
        /// Lấy dữ liệu User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user")]
        [Authorize]
        public Result GetUser()
        {
            Result result = new Result();
            try
            {
                using (db)
                {
                    result.Data = db.Users.ToList().OrderBy(x => x.CreateAt);
                    result.Success = true;
                }
            }
            catch (Exception)
            {
                result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ ThaiNH";
                result.Success = false;
                throw;
            }
            return result;
        }


        /// <summary>
        /// Tìm kiếm user
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("usersearch")]
        //[AllowAnonymous]
        //public Result GetUser([FromBody]PagingParameter paging)
        //{
        //    Result result = new Result();
        //    paging.RecordCount = 5;
        //    paging.Page = 1;

        //    var filters = (List<Filter>) paging.Filters;

        //    try
        //    {
        //        using (db)
        //        {
        //            IQueryable<Users> users = null;
        //            for (int i = 0; i < filters.Count; i++)
        //            {
        //                users = db.Users.Where(x => x.UserName == filters[i].FilterValue);
        //            }
        //                 result.Success = true;
        //                 result.Data = users.ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ ThaiNH";
        //        result.Success = false;
        //        throw;
        //    }
        //    return result;
        //}






    }

}
