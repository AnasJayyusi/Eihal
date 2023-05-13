using System.Net.Mail;
using System.Net;

namespace Eihal.Helper
{
    public class EmailSender
    {
        public static async Task SendEmailAsync(string recipient, string subject, string message, bool IsBodyHtml = false)
        {
            string hostAddress = "smtp-mail.outlook.com";
            string address = "anas.jayyusi@outlook.com";
            string password = "Newthink_Backup2020";
            int Port = 587;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(address);
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            mailMessage.IsBodyHtml = IsBodyHtml;
            mailMessage.To.Add(new MailAddress(recipient));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = hostAddress;

            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential();
            networkCredential.UserName = mailMessage.From.Address;
            networkCredential.Password = password;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = Convert.ToInt32(Port);
            await smtp.SendMailAsync(mailMessage);

        }
    }
}
