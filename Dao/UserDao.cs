using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Dao
{
    public class UserDao
    {
        private UserDao() { }
        private static UserDao instance;
        public static UserDao Instance()
        {
            if (instance == null)
            {
                instance = new UserDao();

            }
            return instance;
        }
        public bool InsertUser(User user)
        {
            bool flagInsert = true;
            try
            {
                string hashedPassword = PasswordHashingService.Instance().HashPassword(user.password);
                var mv = new MovieTicketBookingEntities2();
                User u = new User();
                u.user_name = user.user_name;
                u.user_phone = user.user_phone;
                u.birthday = user.birthday;
                u.email = user.email;
                u.gender = user.gender;
                u.password = hashedPassword;
                mv.Users.Add(u);
                mv.SaveChanges();
                return flagInsert;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagInsert = false;
                return flagInsert;
            }
        }
        public bool GetEmailExists(string email)
        {
            bool flagGet = true;  //false is exit
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from u in mv.Users where u.email == email select u).FirstOrDefault();
                if (result != null)
                {
                    flagGet = false;
                    return flagGet;
                }
                else
                {
                    return flagGet;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagGet = false;
                return flagGet;
            }
        }
        public bool GetPhoneExists(string phone)
        {
            bool flagGet = true;  //false is exit
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from u in mv.Users where u.user_phone == phone select u).FirstOrDefault();
                if (result != null)
                {
                    flagGet = false;
                    return flagGet;
                }
                else
                {
                    return flagGet;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagGet = false;
                return flagGet;
            }
        }
        public bool CheckLogin(string email, string password)
        {
            bool flagCheck = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var user = (from u in mv.Users where u.email.Equals(email) select u).FirstOrDefault();
                if (user != null)
                {
                    bool verifyPassword = PasswordHashingService.Instance().VerifyPassword(password, user.password);
                    if (verifyPassword)
                    {
                        flagCheck = true;
                    }
                    else
                    {
                        flagCheck = false;
                    }
                }
                else
                {
                    flagCheck = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagCheck = false;
            }
            return flagCheck;
        }
        public User GetUserByEmail(string email)
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from us in mv.Users where us.email.Equals(email) select us).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}