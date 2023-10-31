using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Drawing;
using FladeUp_Api.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using EASendMail;

namespace FladeUp_Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly string Host;
        private readonly string Port;
        private readonly string UserName;
        private readonly string Password;
        public EmailService(IConfiguration configuration)
        {
            Host = configuration.GetValue<string>("EmailHost");
            Port = configuration.GetValue<string>("EmailHostPort");
            UserName = configuration.GetValue<string>("EmailUserName");
            Password = configuration.GetValue<string>("EmailPassword");
        }
        public async Task<string> SendMailAsync(string Title, string Body, string Destination)
        {

            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                oMail.From = "vladixerplay@gmail.com";
                oMail.To = Destination;
 
                oMail.HtmlBody = Body;
                oMail.Subject = Title;
  
                SmtpServer oServer = new SmtpServer("smtp.gmail.com");

                oServer.User = "vladixerplay@gmail.com";
                oServer.Password = "gdxg grvh venl krnt";

                oServer.Port = 465;

                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                SmtpClient oSmtp = new SmtpClient();
          
                oSmtp.SendMailAsync(oServer, oMail);

                return "Ok";

            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
                return ep.Message;
            }
        }
    }
}
