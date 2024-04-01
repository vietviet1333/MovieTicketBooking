using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Dao;
using MovieTicketBooking.Models;
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
                List<Movie> list = MovieDao.Instance().GetAllMovies();
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
            return View();
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
    }
}