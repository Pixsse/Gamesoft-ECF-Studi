namespace Gamesoft.Services
{
    public interface IEmailMgt
    {
        bool SendEmail(string to, string subject, string body);

    }
}
