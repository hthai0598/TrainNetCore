using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BLL.Helpers
{
    public class EmailTemplates : IEmailTemplates
    {
        static IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        //public static void Initialize(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //    _configuration = configuration;
        //}

        public EmailTemplates(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }


        public string GetForgotPasswordEmailTemplate(string fullname, string email, string urlforgotemail)
        {
            string emailTemplate = ReadPhysicalFile(string.Format("Resources/Templates/ForgotPassword.template"));
            string emailMessage = emailTemplate
                .Replace("{fullname}", fullname)
                .Replace("{email}", email)
                .Replace("{urlforgotemail}", urlforgotemail)
                .Replace("{hourslive}", _configuration["TokenLifespan:ResetPassword"])
                .Replace("{appname}", _configuration["AppSettings:AppName"])
                .Replace("{appurl}", _configuration["AppSettings:Backend_Url"]);
            return emailMessage;
        }

        public string GetChangePasswordAlertEmailTemplate(string user)
        {
            string emailTemplate = ReadPhysicalFile(string.Format("Resources/Templates/ChangePasswordAlert.template"));
            string emailMessage = emailTemplate
                .Replace("{fullname}", user)
                .Replace("{support}", "")
                .Replace("{appname}", _configuration["AppSettings:AppName"])
                .Replace("{appurl}", _configuration["AppSettings:Backend_Url"]);
            return emailMessage;
        }

        public string GetInviteBuyerEmailTemplate(string fullname, string email, string urlinvitelink, string userinvited_fullname, string companyname)
        {
            string emailTemplate = ReadPhysicalFile(string.Format("Resources/Templates/InviteBuyer.template"));
            string emailMessage = emailTemplate
                .Replace("{fullname}", fullname)
                .Replace("{email}", email)
                .Replace("{urlinvitelink}", urlinvitelink)
                .Replace("{hourslive}", _configuration["TokenLifespan:InviteUser"])
                .Replace("{appname}", _configuration["AppSettings:AppName"])
                .Replace("{appurl}", _configuration["AppSettings:Backend_Url"])
                .Replace("{userinvited_fullname}", userinvited_fullname)
                .Replace("{companyname}", companyname);
            return emailMessage;
        }

        public string GetInviteSupplierEmailTemplate(string fullname, string email, string urlinvitelink, string userinvited_fullname)
        {
            string emailTemplate = ReadPhysicalFile(string.Format("Resources/Templates/InviteSupplier.template"));
            string emailMessage = emailTemplate
                .Replace("{fullname}", fullname)
                .Replace("{email}", email)
                .Replace("{urlinvitelink}", urlinvitelink)
                .Replace("{hourslive}", _configuration["TokenLifespan:InviteUser"])
                .Replace("{appname}", _configuration["AppSettings:AppName"])
                .Replace("{appurl}", _configuration["AppSettings:Backend_Url"])
                .Replace("{userinvited_fullname}", userinvited_fullname);
            return emailMessage;
        }

        public string GetSendFormRequestEmailTemplate(string fullname, string email, string message, string usersend_fullname)
        {
            string emailTemplate = ReadPhysicalFile(string.Format("Resources/Templates/FormRequest.template"));
            string emailMessage = emailTemplate
                .Replace("{fullname}", fullname)
                .Replace("{email}", email)
                .Replace("{message}", message)
                .Replace("{appname}", _configuration["AppSettings:AppName"])
                .Replace("{appurl}", _configuration["AppSettings:Backend_Url"])
                .Replace("{usersend_fullname}", usersend_fullname);
            return emailMessage;
        }

        private string ReadPhysicalFile(string path)
        {
            if (_hostingEnvironment == null)
                throw new InvalidOperationException($"{nameof(EmailTemplates)} is not initialized");

            IFileInfo fileInfo = _hostingEnvironment.ContentRootFileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
                throw new FileNotFoundException($"Template file located at \"{path}\" was not found");

            using (var fs = fileInfo.CreateReadStream())
            {
                using (var sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
