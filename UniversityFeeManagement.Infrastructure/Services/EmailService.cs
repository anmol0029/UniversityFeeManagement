using System.Net;
using System.Net.Mail;

namespace UniversityFeeManagement.Infrastructure.Email;

    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("anmolgodara07@gmail.com", "mctt zdoi gwcw tuqi"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("anmolgodara07@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
