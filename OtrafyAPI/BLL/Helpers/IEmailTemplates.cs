namespace BLL.Helpers
{
    public interface IEmailTemplates
    {
        string GetForgotPasswordEmailTemplate(string fullname, string email, string urlforgotemail);
        string GetChangePasswordAlertEmailTemplate(string user);
        string GetInviteBuyerEmailTemplate(string fullname, string email, string urlinvitelink, string userinvited_fullname, string companyname);
        string GetInviteSupplierEmailTemplate(string fullname, string email, string urlinvitelink, string userinvited_fullname);
        string GetSendFormRequestEmailTemplate(string fullname, string email, string message, string usersend_fullname);
    }
}
