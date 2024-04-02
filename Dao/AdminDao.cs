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
    }
}