using BH_SocialNetwork_Models;
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
    public class CommentController : ControllerBase
    {

        BH_SocialNetwork_DBContext db;

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        public CommentController()
        {

            db = new BH_SocialNetwork_DBContext();
        }


        /// <summary>
        /// Xem tất cả comment của 1 bài viêt
        /// </summary>
        /// <param name="id">Mã bài viết</param>
        /// <param name="paging"></param>
        /// <param name="paging.Page">Trang hiện tại</param>
        /// <param name="paging.RecordCount">Số bản ghi muốn lấy</param>
        /// <returns></returns>
        [Route("comment/{id}")]
        [AllowAnonymous]
        [HttpGet]
        public Result GetCommentById(string id,[FromBody]PagingParameter paging)
        {
            Result result = new Result();
            paging.RecordCount = 5;
            try
            {
                using (db)
                {
                    var _comment = from comment in db.Comment
                                join articles in db.Articles on comment.ArticlesId equals articles.ArticlesId
                                join user in db.Users on comment.UserId equals user.UserId
                                select new
                                {
                                    comment.CommentId,
                                    comment.Body,
                                    comment.CreateAt,
                                    comment.UpdateAt,
                                    user.UserId,
                                    user.UserName,
                                    user.Email,
                                    articles.ArticlesId,
                                };
                    var _query = _comment.Skip((paging.Page - 1) * paging.RecordCount).Take(paging.RecordCount).ToList();
                    result.Success = true;
                    result.Message = "Lấy comment thành công";
                    result.Data = _query.Where(x => x.ArticlesId == Guid.Parse(id)).OrderBy(x => x.CreateAt).ToList();
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
        /// Viết comment trong 1 bài viết
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="comment.articlesID">Mã bài viết được thêm, gửi từ client</param>
        /// /// <param name="comment.body">nội dung, gửi từ client</param>
        /// <returns></returns>
        [Route("comment")]
        [Authorize]
        [HttpPost]
        public Result Comment([FromBody]Comment comment)
        {
            Result result = new Result();
            using (db)
            {
                try
                {
                    //Lấy ra mã ID của người dùng trong token
                    var _id = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                    comment.UserId = Guid.Parse(_id.Value);
                    db.Add(comment);
                    db.SaveChanges();
                    result.Success = true;
                    result.Message = "Thêm comment thành công";
                }
                catch (Exception)
                {
                    result.Message = "Đã xảy ra lỗi, vui lòng liên hệ Thai Nh";
                    result.Success = false;
                    throw;
                }
            }
            return result;
        }



        /// <summary>
        /// Sửa comment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id.commentID">Mã comment muốn sửa, gửi từ client</param>
        ///    /// <param name="id.body">Nội dung sửa, gửi từ client</param>
        /// <returns></returns>
        [Route("updatecomment")]
        [Authorize]
        [HttpPut]
        public Result UpdateCommentById([FromBody]Comment id)
        {
            Result result = new Result();
            using (db)
            {
                try
                {
                    //Lấy ra comment cần sửa
                    var comment = db.Comment.First(x => x.CommentId == id.CommentId);
                    comment.Body = id.Body;
                    db.SaveChanges();
                    result.Success = true;
                    result.Message = "Sửa thành công";
                }
                catch (Exception)
                {
                    result.Success = false;
                    result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ THai Nh";
                }
            }
            return result;
        }



        /// <summary>
        /// Xóa comment
        /// </summary>
        /// <param name="id">Mã comment muốn xóa</param>
        /// <returns></returns>
        [Route("deletecomment")]
        [Authorize]
        [HttpPut]
        public Result DeleteCommentById([FromBody]string id)
        {

            Result result = new Result();
            using (db)
            {
                try
                {
                    //Lấy ra comment cần sửa
                    var _comment = db.Comment.First(x => x.CommentId == Guid.Parse(id));
                    db.Comment.Remove(_comment);
                    db.SaveChanges();
                    result.Success = true;
                    result.Message = "Xóa comment thành công";
                }
                catch (Exception)
                {
                    result.Success = false;
                    result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ THaiNh";
                    throw;
                }
            }
            return result;

        }
    }
}