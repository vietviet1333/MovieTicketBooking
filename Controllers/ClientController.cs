using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Dao;
using MovieTicketBooking.Models;
using MovieTicketBooking.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTicketBooking.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Home()
        {
            try
            {
                List<Movie> list = MovieDao.Instance().GetMovieNowPlaying();
                List<Movie> listMovie = new List<Movie>();
                if (list.Count < 6)
                {
                    listMovie = list;
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        listMovie.Add(list[i]);
                    }
                }



                return View(listMovie);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Detailmovie(int movie_id)
        {
            try
            {
                ShowtimeDao.Instance().AutoUpdateShowtime();
                var mo = MovieDao.Instance().GetMovieById(movie_id);
                return View(mo);
            }
            catch
            {
                return Redirect("Home");
            }
        }
        public ActionResult ViewLogin()
        {
            try
            {
                if (Session["LoggedInUserID"] == null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("AccountUser", new { userid = Session["LoggedInUserID"] });
                }

            }
            catch
            {
                return Redirect("/Client/Home");
            }
        }
        [HttpPost]
        public int Register(User us)
        {
            try
            {
                bool emailExists = UserDao.Instance().GetEmailExists(us.email);
                bool phoneExists = UserDao.Instance().GetPhoneExists(us.user_phone);

                if (emailExists == true && phoneExists == true)
                {
                    UserDao.Instance().InsertUser(us);
                    return 0; // Successful registration
                }
                else if (emailExists == false)
                {
                    return 2; // email number exists
                }
                else if (phoneExists == false)
                {
                    return 3; // phone exists
                }
                else
                {
                    return 1; // Both email and phone number exist
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 4; // Error during registration
            }
        }
        [HttpPost]
        public bool Login(string email, string password)
        {
            try
            {
                bool checkUser = UserDao.Instance().CheckLogin(email, password);
                if (checkUser == true)
                {
                    var user = UserDao.Instance().GetUserByEmail(email);
                    Session["LoggedInUserID"] = user.user_id;
                    Session["UserName"] = user.user_name;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        [HttpGet]
        [CheckLogin]

        public ActionResult AccountUser()
        {
            try
            {
                int userid = (int)Session["LoggedInUserID"];
                TempData["totalofuser"] = BookingDao.Instance().GetTotalPriceBookingOfUser(userid);
                var u = UserDao.Instance().GetUserById(userid);
                return View(u);
            }
            catch
            {
                return Redirect("/Client/Home");
            }
        }
        [HttpPost]
        public  int  UserSendMail (string email) 
        {//0 is success , 1 is email k tim thay, 2 is error

            int flagChange = 0;
            try
            {
                bool checkemail = UserDao.Instance().GetEmailExists(email);
                if (checkemail == false)
                {
                    
                  
                    Sendmail.Instance().SendmailChangePassUser(email);

                }
                else
                {
                    flagChange = 1;
                }


            }
            catch
            {
                flagChange = 2;
            }
            return flagChange;

        }
        [HttpPost]
        public ActionResult Changepass()
        {
            try
            {
                string email =Request.Form["email"] ;
                string token  = Request.Form["token"];
                string newpass = Request.Form["newPassword"];
                bool c = UserDao.Instance().UserChangePasswordByEmail(email,newpass,token);
                if (c == true)
                {
                    TempData["changepass"] = "Change Password Success";
                    return Redirect("/Client/ViewLogin");
                }
                else
                {
                    TempData["changepass"] = "Change Password Failed";
                    return Redirect("/Client/ViewLogin");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["changepass"] = "Change Password Failed";
                return Redirect("/Client/ViewLogin");
            }

        }
        [HttpPost]
        public bool UserChangePassword(int userid, string newpassword, string oldpassword)
        {
            bool flagUpdate = true;
            try
            {
                bool updatepass = UserDao.Instance().UserChangePassword(userid, oldpassword, newpassword);
                if (updatepass == true)
                {
                    flagUpdate = true;
                }
                else
                {
                    flagUpdate = false;
                }


            }
            catch
            {
                flagUpdate = false;
            }
            return flagUpdate;
        }
        [HttpGet]
        public ActionResult Changepassword(string token, string email)
        {
            try
            {
                bool check = UserDao.Instance().CheckTokenViewchangepass(token, DateTime.Now);
                if (check == true)
                {
                    TempData["token"] = token;
                    TempData["email"] = email;
                    return View();
                }
                else
                {
                    return View("Errors");
                }
            }
            catch
            {
                return View("Errors");
            }
        }
        [HttpPost]
        public bool UserEditProfile(User user)
        {
            bool flagUpdate = true;
            try
            {
                bool edit = UserDao.Instance().EditUser(user);
                if (edit == true)
                {
                    flagUpdate = true;
                }
                else
                {
                    flagUpdate = false;
                }
            }
            catch
            {
                flagUpdate = false;
            }
            return flagUpdate;
        }
        public ActionResult NowPlaying()
        {
            try
            {
                var movie = MovieDao.Instance().GetMovieNowPlaying();
                return View(movie);
            }
            catch
            {
                return View("Errors");
            }
        }
        public ActionResult CommingSoon()
        {
            try
            {
                var movie = MovieDao.Instance().GetMovieCommingSoon();
                return View(movie);
            }
            catch
            {
                return View("Errors");
            }
        }
    }
}