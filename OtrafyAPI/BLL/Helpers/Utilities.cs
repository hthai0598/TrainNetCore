using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BLL.Helpers
{
    public static class Utilities
    {
        public static string GetUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value?.Trim();
        }

        public static string GetUsername(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value?.Trim();
        }

        public static string GetEmail(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value?.Trim();
        }

        public static string GetRoles(ClaimsPrincipal identity)
        {
            return identity.FindFirst(ClaimTypes.Role)?.Value?.Trim();
        }

        public static string GetCompanyId(ClaimsPrincipal user)
        {
            return user.FindFirst("CompanyId")?.Value?.Trim();
        }

        public static string GetBuyerId(ClaimsPrincipal user)
        {
            return user.FindFirst("BuyerId")?.Value?.Trim();
        }

        public static string GetSupplierId(ClaimsPrincipal user)
        {
            return user.FindFirst("SupplierId")?.Value?.Trim();
        }
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                { 
                    if (computedHash[i] != passwordHash[i]) return false; 
                }
            }
            return true; 
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?"                        // non-alphanumeric
            };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public static byte[] ConvertFileToByteArray(IFormFile file)
        {
            byte[] result = null;

            using (var fileStream = file.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                result = memoryStream.ToArray();
            }

            return result;
        }

        public static string GetFileName(IFormFile file)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)
                        .FileName.Trim('"');

            return fileName;
        }

        public static string GenerateFileName(IFormFile file)
        {
            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            var filename = parsedContentDisposition.FileName.Trim('"');

            filename = Guid.NewGuid().ToString() + "-" + filename;
            return filename;
        }

    }
}
