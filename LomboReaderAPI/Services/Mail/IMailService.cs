namespace LimboReaderAPI.Services.Mail
{
    public interface IMailService
    {
        void SendMail(string mail,string code);
    }
}
