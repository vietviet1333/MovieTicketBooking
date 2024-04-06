using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Dao;
using MovieTicketBooking.Models;
using MovieTicketBooking.ModelView;
using MovieTicketBooking.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTicketBooking.Controllers
{
    public class ShowtimeController : Controller
    {
        // GET: Showtime
        [CheckAdminLogin]
        public ActionResult Showtimes()
        {
            try
            {
                ShowtimeDao.Instance().AutoUpdateShowtime();
                return View();
            }
            catch
            {
                return Redirect("Dashboard");
            }
        }
        [CheckAdminLogin]
        public ActionResult Addshowtime(int id_movie)
        {
            try
            {
                var mo = MovieDao.Instance().GetMovieById(id_movie);
                return View(mo);
            }
            catch
            {
                return Redirect("Dashboard");
            }
        }
        [HttpGet]
        public ActionResult FetchRooms(int theaterId)
        {
            try
            {
                var r = RoomDao.Instance().GetAllRoomsOfTheater(theaterId);
                return PartialView("_RoomPartial", r);
            }
            catch
            {
                return PartialView("Errors");
            }
        }
        [HttpPost]
        public ActionResult Insertshowtimeofmovie(Showtime st)
        {
            try
            {
                ShowtimeDao.Instance().InsertShowtime(st);
                return RedirectToAction("Addshowtime", new { id_movie = st.movie_id });
            }
            catch
            {
                return Redirect("Dashboard");
            }
        }
        public ActionResult Insertshowtime(Showtime st)
        {
            try
            {
                ShowtimeDao.Instance().InsertShowtime(st);
                return Redirect("Showtimes");
            }
            catch
            {
                return Redirect("Dashboard");
            }
        }
        [HttpPost]
        public bool Checkshowtime(Showtime st)
        {
            bool result = true;
            try
            {

                bool chk = ShowtimeDao.Instance().CheckShowtime(st);
                if (chk == false)
                {
                    return false;
                }
                else
                {
                    return result;
                }
            }
            catch
            {
                return result;
            }
        }
        [HttpPost]
        public ActionResult TheaterShowtime(int id_city, DateTime showtimedate , int movieid)
        {
            try
            {
                var result = TheaterDao.Instance().GetTheatersWithShowtime(id_city, showtimedate, movieid);
                ViewData["ShowtimeDate"] = showtimedate;
                return PartialView("TheaterShowtime", result);
            }
            catch
            {
                return Content("An error occurred while retrieving theater showtimes.");
            }

        }
        [HttpPost]
        public ActionResult ShowtimeofTheater(int theater_id, DateTime showtimedate)
        {
            try
            {
                var result = ShowtimeDao.Instance().GetShowtimeofTheater(theater_id, showtimedate);
                return PartialView("ShowtimeofTheater", result);
            }
            catch
            {
                return null;
            }
        }
        [HttpGet]
        [CheckLogin]
        public ActionResult ShowtimeBooking(int showtimeid)
        {
            try
            {
               
                var st = ShowtimeDao.Instance().ShowtimeBooking(showtimeid);
                return View(st);
            }
            catch {
                return Redirect("Client/Home");
            }
        }
        [HttpPost]
        
        public ActionResult BookingCheckcount(string nameseats , string idcombos,string quantitycombos , int showtime_id, decimal totalofbooking, int room_id,int id_user) {
            try
            {
                
                BookingDao.Instance().InsertBooking(nameseats, idcombos, quantitycombos, showtime_id, totalofbooking, room_id,id_user);
                var emailuser = UserDao.Instance().GetUserById(id_user);
               if(emailuser != null)
                {
                    Sendmail.Instance().SendmailCheckCount(emailuser.email);
                }
                return Redirect("/Client/Home");
            }
            catch
            {
                return Redirect("/Client/Home");
            }
        }
    }
}