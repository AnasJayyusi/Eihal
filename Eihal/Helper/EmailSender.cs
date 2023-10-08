﻿using System.Net.Mail;
using System.Net;

namespace Eihal.Helper
{
    public class EmailSender
    {
        public static async Task SendEmailAsync(string recipient, string subject, string message, bool IsBodyHtml = false)
        {
       
            int port = 587;
            string hostAddress = "mail.eihal.com";
            string address = "Eihal@eihal.com";
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(address);
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            mailMessage.IsBodyHtml = IsBodyHtml;
            mailMessage.To.Add(new MailAddress(recipient));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = hostAddress;

            smtp.EnableSsl = false;
            smtp.Credentials = new System.Net.NetworkCredential("Info@eihal.com", "Geno@2001");
            smtp.Port = Convert.ToInt32(port);
            await smtp.SendMailAsync(mailMessage);

        }
    }
}
