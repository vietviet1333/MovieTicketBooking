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
            }catch
            {
                return View();
            }
        }
        public ActionResult Detailmovie( int movie_id)
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
        public ActionResult ViewLogin() {
        return View();
        }
     }
}