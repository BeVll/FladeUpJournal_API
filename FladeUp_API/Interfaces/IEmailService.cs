namespace FladeUp_Api.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendMailAsync(string Title, string Body, string Destination);
    }
}
