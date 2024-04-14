using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Dao;

namespace MovieTicketBooking.Others
{
    public class Sendmail
    {
        private Sendmail() { }
        private static Sendmail instance;
        public static Sendmail Instance()
        {
            if (instance == null)
            {
                instance = new Sendmail();
            }
            return instance;
        }
        public void Sendmaill(string usermail, int verificationCode)
        {
            string fromEmail = "ntviet1333@gmail.com";
            string password = "zkgq lmor buji xotb";
            string htmlBody = @"
        <html>
        <body>
            <h2>Password Change Verification</h2>
            <p>Your verification code is: <strong>"" + verificationCode + @""</strong></p>
            <div>Change pass<a href='https://localhost:44351/Admin/Changepass'>here</a></div>
        </body>
        </html>
       ";

            // Recipient's email address
            string toEmail = usermail;

            // SMTP client configuration
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(fromEmail, password);

            // Email message
            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
            mailMessage.From = new MailAddress(fromEmail, "VIET CINEMAS");
            mailMessage.Subject = "Password Change Verification";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = htmlBody;


            try
            {
                // Send the email
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }

        }

        public void SendmailChangePassUser(string usermail)
        {   
            string token = PasswordHashingService.Instance().HashPassword(usermail);
            UserDao.Instance().InsertToken(usermail, token);
            string changepassurl = $"https://localhost:44351/Client/Changepassword?token={HttpUtility.UrlEncode(token)}&&email={HttpUtility.UrlEncode(usermail)}";
            string htmlBody = @"
        <html>
        <body>
             <div style=""font-weight: bold;color: rgb(216, 216, 0);"">Note: link is only valid for 5 minutes</div>
            <div>Change pass <a href='" + changepassurl + @"'>here</a></div>
        </body>
        </html>";
            string fromEmail = "ntviet1333@gmail.com";
            string password = "zkgq lmor buji xotb";
            // Recipient's email address
            string toEmail = usermail;

            // SMTP client configuration
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(fromEmail, password);

            // Email message
            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
            mailMessage.From = new MailAddress(fromEmail, "VIET CINEMAS");
            mailMessage.Subject = "Password Change Verification";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = htmlBody;


            try
            {
                // Send the email
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }

        }
        public void SendmailCheckCount(string usermail)
        {
            string fromEmail = "ntviet1333@gmail.com";
            string password = "zkgq lmor buji xotb";
            string htmlBody = "Checkcount Success, Thank you for choosing Viet Cinemas ";

            // Recipient's email address
            string toEmail = usermail;

            // SMTP client configuration
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(fromEmail, password);

            // Email message
            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
            mailMessage.From = new MailAddress(fromEmail, "VIET CINEMAS");
            mailMessage.Subject = "Booking Ticket";
            mailMessage.Body = htmlBody;


            try
            {
                // Send the email
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }

        }

    }
}