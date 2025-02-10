using System.Net.Mail;
using System.Net;

namespace shop_ASP_CORE_MVC.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("cuong62003@gmail.com", "kddq ttlf edez zwyo")
            };

            return client.SendMailAsync(
                new MailMessage(from: "cuong62003@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
