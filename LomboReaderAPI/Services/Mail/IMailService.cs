namespace LomboReaderAPI.Services.Mail
{
    public interface IMailService
    {
        void SendMail(string mail,string code);
    }
}
