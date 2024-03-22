using MovieTicketBooking.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTicketBooking.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Addnewmovie()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insertmovie(string namemovi,string directo,string languag, DateTime release_dat,int duratio,string genr,string descriptio, string urlimag,string cas) {

            MovieDao.Instance().Addnewmovie(namemovi,directo,languag,release_dat,duratio,genr,descriptio,urlimag,cas);

            return Redirect("/Admin/Dashboard");
            
        }
        public ActionResult Listmovie()

        {
            var listMovie = MovieDao.Instance().GetAllMovies();
            return View(listMovie);
        }
    }
}