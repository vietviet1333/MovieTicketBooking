using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Dao
{
    public class AdminDao
    {
        private AdminDao() { }
        private static AdminDao instance;
        public static AdminDao Instance()
        {
            if (instance == null)
            {
                instance = new AdminDao();
            }
            return instance;
        }
        public bool AdminLogin(string username, string password)
        {
            bool flagLogin = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var admin = (from ad in mv.Admins where ad.username == username select ad).First();
                if (admin != null)
                {
                    admin.verificationCode = 0;// reset verificationCode to 0 
                    mv.SaveChanges();
                    bool verifyPassword = PasswordHashingService.Instance().VerifyPassword(password, admin.password);
                    if (verifyPassword)
                    {
                        flagLogin = true;
                    }
                    else
                    {
                        flagLogin = false;
                    }
                }
                else
                {
                    flagLogin = false;
                }

            }
            catch (Exception ex)
            {
                flagLogin = false;
                Console.WriteLine(ex.Message);
            }
            return flagLogin;
        }
        public bool AdminChangePassword(string email, int verificationCode, string newpassword)
        {
            bool flagChangePassword = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var ad = mv.Admins.SingleOrDefault(x => x.email == email && x.verificationCode.HasValue);
                if (ad != null && ad.verificationCode == verificationCode)
                {
                    string hashedPassword = PasswordHashingService.Instance().HashPassword(newpassword);
                    ad.password = hashedPassword;
                    ad.verificationCode = 0;
                    mv.SaveChanges();
                    flagChangePassword = true;

                }

            }
            catch (Exception ex)
            {
                flagChangePassword = false;
                Console.WriteLine(ex.Message);
            }
            return flagChangePassword;
        }
        public bool InsertCode(int verificationCode, string email)
        {
            bool flagInsert = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = mv.Admins.SingleOrDefault(x => x.email == email);
                if (result != null)
                {
                    result.verificationCode = verificationCode;
                    mv.SaveChanges();
                    flagInsert = true;
                }
                else
                {
                    flagInsert = false;
                }
            }
            catch (Exception ex)
            {
                flagInsert = false;
                Console.WriteLine(ex.Message);
            }
            return flagInsert;
        }
    }
}