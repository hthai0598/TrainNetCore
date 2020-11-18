using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BH_SocialNetwork_Models;
using BH_SocialNetwork_Models.Modelss;
using Microsoft.AspNetCore.Authorization;

namespace BH_SocialNetwork_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        BH_SocialNetwork_DBContext db;
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        public ArticlesController()
        {
            db = new BH_SocialNetwork_DBContext();
        }

        /// <summary>
        /// Lấy tất cả Articles(GOLBAL)
        /// </summary>
        /// <returns></returns>
        [Route("articles")]
        [AllowAnonymous]
        [HttpGet]
        public Result GetArticles([FromBody]PagingParameter paging)
        {
            Result result = new Result();
            paging.RecordCount = 5;
            try
            {
                using (db)
                {
                    var _articles = (from articles in db.Articles
                                     join user in db.Users on articles.UserId equals user.UserId
                                     select new
                                     {
                                         articles.ArticlesId,
                                         articles.Title,
                                         articles.Description,
                                         articles.Body,
                                         articles.CreateAt,
                                         articles.UpdateAt,
                                         user.UserId,
                                         user.UserName,
                                         articles.FavotiredCount,

                                     }).OrderBy(x => x.CreateAt);
                    var _query1 = _articles.Skip((paging.Page - 1) * paging.RecordCount).Take(paging.RecordCount).ToList();
                    result.Success = true;
                    result.Data = _query1;
                    result.Message = "Lấy bài viết thành công";
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
        /// Lấy ra bài viết theo mã
        /// </summary>
        /// <param name="id">Mã bài viết</param>
        /// <returns></returns>
        [Route("articles/{id}")]
        [AllowAnonymous]
        [HttpGet]
        public Result GetArticlesByID(string id)
        {
            Result result = new Result();
            try
            {
                using (db)
                {
                    var query = (from articles in db.Articles
                                 join user in db.Users on articles.UserId equals user.UserId
                                 join comment in db.Comment on articles.ArticlesId equals comment.ArticlesId

                                 select new
                                 {
                                     articles.ArticlesId,
                                     articles.Title,
                                     articles.Description,
                                     articles.Body,
                                     articles.CreateAt,
                                     articles.UpdateAt,
                                     user.UserId,
                                     user.UserName,
                                     articles.FavotiredCount,
                                     comment = comment.Body

                                 }).OrderBy(x => x.CreateAt).ToList();
                    var total = query.Where(x => x.ArticlesId == Guid.Parse(id)).OrderBy(x => x.CreateAt);
                    result.Success = true;
                    result.Data = query;
                    result.Message = "Lấy bài viết thành công";
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
        /// Người dùng tạo mới một bài viết
        /// </summary>
        /// <param name = "r" ></ param >
        /// <param name = "r.title" >Tiêu đề bài viết</ param >
        /// <param name = "r.description" >Mô tả bài viết</ param >
        /// <param name = "r.body" >Nội dung bài viết</ param >
        /// <param name = "r.tag" >List tag bài viết</ param >
        /// < returns ></ returns >
        [HttpPost]
        [Authorize]
        [Route("createarticles")]
        public Result CreateUser([FromBody]Articles r)
        {

            Result result = new Result();
            using (db)
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        //ID của user trong Token lấy ra từ claims khi user đăng nhập.
                        var _id = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                        if (_id.Value != null)
                        {
                            r.UserId = Guid.Parse(_id.Value);
                            // Thêm bài viết vào bảng articles
                            db.Articles.Add(r);
                            db.SaveChanges();

                            //Lấy ra ID bài viết mới nhất
                            var _articlesId = (from articles in db.Articles select articles).Take(1).OrderByDescending(c => c.CreateAt).Select(x => x.ArticlesId).FirstOrDefault();
                            //Thêm ID bài viết vừa tạo vào bẳng favotired
                            Favotired favotired = new Favotired();
                            favotired.ArticlesId = _articlesId;
                            db.Favotired.Add(favotired);
                            db.SaveChanges();
                            result.Success = true;
                            result.Message = "Thêm thành công";

                            trans.Commit();
                        }

                        else
                        {
                            result.Success = false;
                            result.Message = "Thêm thất bại";
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ ThaiNH";
                        result.Success = false;

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy ra tất cả các bài viết của người mình đang theo dõi
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="paging.Page">Số trang hiện tại</param>
        /// <param name="paging.RecordCount">Số bản ghi muốn lấy</param>
        /// <returns></returns>
        [Route("usersfollowing")]
        [Authorize]
        [HttpGet]
        public Result GetArticlesUserFollowing([FromBody]PagingParameter paging)
        {
            Result result = new Result();
            paging.RecordCount = 5;
            try
            {
                using (db)
                {
                    var _id = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                    //Lấy ra tất cả người dùng theo dõi by ID 
                    var user = (from users in db.Users
                                join userfl in db.UsersFollowing on users.UserId equals userfl.UserId
                                select new
                                {
                                    userfl.State,
                                    userfl.UserFollowId,
                                    users.UserName,
                                    users.UserId
                                }).Where(x => x.State == true);

                    var following = user.Where(x => x.UserId == Guid.Parse(_id.Value)); //following = những người theo dõi user có mã là "id"


                    //Lấy ra tất cả bài viết của những người được follow bởi id

                    var _articles = from follow in following
                                    join articles in db.Articles on follow.UserId equals articles.UserId
                                    select new
                                    {
                                        articles.ArticlesId,
                                        articles.Title,
                                        articles.Description,
                                        articles.Body,
                                        articles.CreateAt,
                                        articles.UpdateAt,
                                        articles.FavotiredCount,
                                        follow.UserId,
                                        follow.UserName,
                                    };
                    var _query = _articles.Skip((paging.Page - 1) * paging.RecordCount).Take(paging.RecordCount).ToList();

                    result.Success = true;
                    result.Data = _query;
                    result.Message = "Lấy dữ liệu thành công";

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
        /// Cập nhật lượt Like và số lần Like của 1 bài viết
        /// </summary>
        /// <param name="favotired">Mã bài viết</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("updatearticlesfavorited")]
        public Result UpdateArticles([FromBody]string favotired)
        {
            Result result = new Result();
            using (db)
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Lấy ra mã ID của người dùng trong token
                        var _id = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));

                        //Bài viết được Like
                        var _articlesLike = db.Favotired.SingleOrDefault(x => x.ArticlesId == Guid.Parse(favotired) && x.UserId == Guid.Parse(_id.Value));

                        if (_articlesLike != null)
                        {
                            //unlike
                            if (_articlesLike.Favotired1 == true && _articlesLike.UserId != null && _articlesLike.UserId == Guid.Parse(_id.Value))
                            {
                                _articlesLike.Favotired1 = false;
                                db.SaveChanges();
                                result.Message = "Unlike Thành công";
                            }
                            //Like lại
                            else if (_articlesLike.Favotired1 == false && _articlesLike.UserId != null && _articlesLike.UserId == Guid.Parse(_id.Value))
                            {
                                _articlesLike.UserId = Guid.Parse(_id.Value);
                                _articlesLike.Favotired1 = true;
                                db.SaveChanges();
                                result.Message = "Like Thành công";
                            }
                            //Like lần đầu
                            else if (_articlesLike.Favotired1 == false && _articlesLike.UserId == null)
                            {
                                _articlesLike.UserId = Guid.Parse(_id.Value);
                                _articlesLike.Favotired1 = true;
                                db.SaveChanges();
                                result.Message = "Like Thành công";
                            }
                            //Trường hợp khác
                            else
                            {
                                Favotired fa = new Favotired();
                                fa.UserId = Guid.Parse(_id.Value);
                                fa.Favotired1 = true;
                                fa.ArticlesId = Guid.Parse(favotired);
                                db.Favotired.Add(fa);
                                db.SaveChanges();
                                result.Message = "Like thành công";
                            }
                        }
                        else
                        {
                            Favotired fa = new Favotired();
                            fa.UserId = Guid.Parse(_id.Value);
                            fa.Favotired1 = true;
                            fa.ArticlesId = Guid.Parse(favotired);
                            db.Favotired.Add(fa);
                            db.SaveChanges();
                            result.Message = "Like thành công";
                        }



                        //Lấy ra tổng sổ lượt thích bài viết
                        var _count = db.Favotired.Where(x => x.ArticlesId == Guid.Parse(favotired));
                        var _result = _count.AsEnumerable().Sum(x => Convert.ToInt32(x.Favotired1));

                        //Update tổng số lượt thích vào bài viết
                        var update = db.Articles.First(x => x.ArticlesId == Guid.Parse(favotired));
                        update.FavotiredCount = _result;
                        db.SaveChanges();
                        result.Success = true;

                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        result.Success = false;
                        result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ Thai Nh";
                      
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// Lấy ra những tag phổ biến nhất
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("gettag")]
        public Result Tags()
        {
            Result result = new Result();
            try
            {
                using (db)
                {
                    var query = db.Tag.GroupBy(p => p.TagName).Select(g => g.Key).Take(20).ToList();
                    result.Success = true;
                    result.Message = "Lấy dữ liệu thành công";
                    result.Data = query;
                }
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ ThaiNh";

            }
            return result;
        }


        /// <summary>
        /// Tìm kiếm bài viết theo tagname
        /// </summary>
        /// <param name="tag">tên tag cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("search-tag/{tag}")]
        public Result SearchTag(string tag)
        {
            Result result = new Result();
            try
            {
                var query = db.Tag.Where(x => x.TagName.Contains(tag));
                result.Data = query.ToList();
                result.Success = true;
                result.Message = "Lấy dữ liệu thành công";
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Đã có lỗi xảy ra, vui lòng liên hệ Thai Nh";

                throw;
            }
            return result;
        }


    }
}