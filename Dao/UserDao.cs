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
                    user.verificationCode = 0;
                    mv.SaveChanges();
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
                var result = (from u in mv.Users select u).ToList();
                return result;
            }catch(Exception ex)
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
                var u = mv.Users.SingleOrDefault(x=> x.user_id == user.user_id);
                u.user_name= user.user_name;
                u.email= user.email;
                u.user_phone= user.user_phone;
                u.birthday= user.birthday;
                u.gender= user.gender;
                mv.SaveChanges();
                return flagEdit;
            }catch(Exception ex)
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
                var u = mv.Users.SingleOrDefault(x=>x.user_id == user_id);
                bool checkOldPass = PasswordHashingService.Instance().VerifyPassword(oldpassword, u.password);
                if (checkOldPass)
                {
                    var newp = PasswordHashingService.Instance().HashPassword(newpassword);
                    u.password = newp;
                    mv.SaveChanges();
                    
                }
                else
                {
                    flagChangePass= false;
                   
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagChangePass= false;
            }
            return flagChangePass;
        }
        public bool UserChangePasswordByEmail(string email, int verificationCode, string newpassword)
        {
            bool flagChangePassword = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var ad = mv.Users.SingleOrDefault(x => x.email == email && x.verificationCode.HasValue);
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
                var result = mv.Users.SingleOrDefault(x => x.email == email);
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