using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public User GetUserById(int id)
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var user = (from u in mv.Users where u.user_id.Equals(id) select u).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public List<User> GetAllUser()
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from u in mv.Users select u).OrderByDescending(x => x.user_id).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public bool EditUser(User user)
        {
            bool flagEdit = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                int userid = (int)HttpContext.Current.Session["LoggedInUserID"];
                var u = mv.Users.SingleOrDefault(x => x.user_id == userid);
                u.user_name = user.user_name;
                u.email = user.email;
                u.user_phone = user.user_phone;
                u.birthday = user.birthday;
                u.gender = user.gender;
                mv.SaveChanges();
                return flagEdit;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagEdit = false;
                return flagEdit;
            }
        }
        public bool UserChangePassword(int user_id, string oldpassword, string newpassword)
        {
            bool flagChangePass = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var u = mv.Users.SingleOrDefault(x => x.user_id == user_id);
                bool checkOldPass = PasswordHashingService.Instance().VerifyPassword(oldpassword, u.password);
                if (checkOldPass)
                {
                    var newp = PasswordHashingService.Instance().HashPassword(newpassword);
                    u.password = newp;
                    mv.SaveChanges();

                }
                else
                {
                    flagChangePass = false;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagChangePass = false;
            }
            return flagChangePass;
        }
        public bool UserChangePasswordByEmail(string email, string newpassword , string token)
        {
            bool flagChangePassword = true;
            try
            {
               using(var mv = new MovieTicketBookingEntities2())
                {
                    bool check = CheckToken(email, token, DateTime.Now);
                    if(check == true)
                    {
                        var u = (from us in mv.Users where us.email== email select us).First();
                        if (u != null)
                        {
                            u.password=PasswordHashingService.Instance().HashPassword(newpassword);
                            mv.SaveChanges();
                        }
                        else
                        {
                            flagChangePassword = false;
                        }
                    }
                    else
                    {
                        flagChangePassword = false;
                    }
                }

            }
            catch (Exception ex)
            {
                flagChangePassword = false;
                Console.WriteLine(ex.Message);
            }
            return flagChangePassword;
        }
        public bool InsertToken(string email, string token)
        {
            bool flagInsert = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                Token tk = new Token();
                tk.create_date = DateTime.Now.AddMinutes(5);
                tk.user_email = email;
                tk.token1 =token;
                mv.Tokens.Add(tk);
                mv.SaveChanges();
            }
            catch (Exception ex)
            {
                flagInsert = false;
                Console.WriteLine(ex.Message);
            }
            return flagInsert;
        }
        public bool CheckToken(string email , string token, DateTime date)
        {
            bool check = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from t in mv.Tokens where t.user_email == email && t.token1 == token && t.create_date >date select t).First();
                if (result != null)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                check = false;
            }
            return check;
        }
        public bool CheckTokenViewchangepass(string token, DateTime dat)
        {
            bool check = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from t in mv.Tokens where t.token1 == token && t.create_date > dat select t).First();
                if (result != null)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            }
            catch
            {
               check= false;
            }
            return check;
        }
    }
}