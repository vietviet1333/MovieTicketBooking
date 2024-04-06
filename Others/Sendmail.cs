using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

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
            <p>Your verification code is: <strong>" + verificationCode + @"</strong></p>
            <p>Please enter the verification code in the field below to change your password:</p>
            <form action='https://localhost:44351/Admin/Changepass' method='post'>
                <input type='hidden' name='email' value='" + usermail + @"' />
                <input type='text' name='verificationCode' placeholder='Verification Code' required /><br /><br />
                <input type='password' name='newPassword' placeholder='New Password' required /><br /><br />
                <input type='submit' style='background-color:white;border:1px solid green;border-radius:8px;color:green' value='Submit' />
            </form>
        </body>
        </html>";

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

        public void SendmailChangePassUser(string usermail, int verificationCode)
        {
            string fromEmail = "ntviet1333@gmail.com";
            string password = "zkgq lmor buji xotb";
            string htmlBody = @"
        <html>
        <body>
            <h2>Password Change Verification</h2>
            <p>Your verification code is: <strong>" + verificationCode + @"</strong></p>
            <p>Please enter the verification code in the field below to change your password:</p>
            <form action='https://localhost:44351/Client/Changepass' method='post'>
                <input type='hidden' name='email' value='" + usermail + @"' />
                <input type='text' name='verificationCode' placeholder='Verification Code' required /><br /><br />
                <input type='password' name='newPassword' placeholder='New Password' required /><br /><br />
                <input type='submit' style='background-color:white;border:1px solid green;border-radius:8px;color:green' value='Submit' />
            </form>
        </body>
        </html>";

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