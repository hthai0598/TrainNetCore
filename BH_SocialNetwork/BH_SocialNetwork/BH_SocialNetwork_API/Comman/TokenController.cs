using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;

namespace BH_SocialNetwork_API.Controllers
{
    public class TokenController : Controller
    {
        private const string SECRET_KEY = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        public static readonly SymmetricSecurityKey SINGING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        //[HttpGet]
        //[Route("api/Token/{username}/{password}")]
        //public IActionResult Get(string username, string password)
        //{
        //    if (username == password)
        //    {
        //        return new ObjectResult(GenerateToken(username));
        //    }
        //    else return BadRequest();
        //}

        //private object GenerateToken(string username)
        //{
        //    var token = new JwtSecurityToken(
        //        claims: new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name,username),
        //            new Claim("User",username)
        //        },
        //        issuer: "Ngô Hoàng Thái",
        //        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
        //        expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
        //        signingCredentials: new SigningCredentials(SINGING_KEY, SecurityAlgorithms.HmacSha256)


        //        );
        //    IdentityModelEventSource.ShowPII = true;
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        /// <summary>
        /// Trả về token nếu người dùng đăng nhập đúng
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public object GenerateToken(string username)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,username),
                        new Claim("UserID",username)
                    };
            var token = new JwtSecurityToken(
                issuer: "Ngô Hoàng Thái",
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddMinutes(2)).DateTime,
                signingCredentials: new SigningCredentials(SINGING_KEY, SecurityAlgorithms.HmacSha256),
                claims: claims
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var tokenDescription = new SecurityTokenDescriptor
            //    {
            //        Subject = new ClaimsIdentity(new[]
            //        {
            //            new Claim(ClaimTypes.Name,username),
            //            new Claim("sub",username)
            //        }),
            //        Expires = DateTime.UtcNow.AddDays(7),
            //    };
            //    tokenDescription.Issuer = "Admin";
            //    tokenDescription.SigningCredentials= new SigningCredentials(SINGING_KEY, SecurityAlgorithms.HmacSha256);
            //    var token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescription);
            //    var tokenString = tokenHandler.WriteToken(token);
            //    return tokenString;
        }
    }
}
