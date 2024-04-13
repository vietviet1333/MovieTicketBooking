using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTicketBooking.Controllers
{
    public class TheaterController : Controller
    {
        // GET: Theater
        [CheckAdminLogin]
        public ActionResult Addnewtheater()
        {
            try
            {
                return View(TheaterDao.Instance().getAllCity());
            }
            catch
            {
                return View("Errors");
            }
        }
        [CheckAdminLogin]
        public ActionResult AddnewCity()
        {
            try
            {
                return View(TheaterDao.Instance().getAllCity());
            }
            catch
            {
                return View("Errors");
            }
        }
        [HttpPost]
        public ActionResult Insertcity()
        {
            string name = Request.Form["name"];
            TheaterDao.Instance().Insertcity(name);
            return Redirect("AddnewCity");
        }
        [HttpPost]
        public bool Inserttheate(string theatername, string location, int city, string urlimag)
        {
            bool flagInsert = true;
            try
            {
                bool inserted = TheaterDao.Instance().Inserttheater(theatername, location, city, urlimag);
                if (inserted == true)
                {
                    flagInsert = true;
                }
                else
                {
                    flagInsert = false;
                }
            }
            catch
            {
                flagInsert = false;
            }
            return flagInsert;

        }
        [HttpGet, CheckAdminLogin]
        public ActionResult Theater_detail(int id)
        {

            try
            {

                var thea = TheaterDao.Instance().getTheaterById(id);

                if (thea == null)
                {
                    return Redirect("Addnewtheater");
                }
                else
                {
                    return View(thea);
                }

            }
            catch
            {
                return Redirect("Addnewtheater");
            }
        }
        [HttpPost]
        public ActionResult Addnewroom()
        {
            string room_name = Request.Form["roomname"];
            int theater_id = int.Parse(Request.Form["theater_id"]);
            int row = int.Parse(Request.Form["row"]);
            int column = int.Parse(Request.Form["colummn"]);
            RoomDao.Instance().Insertnewroomandchair(room_name, theater_id, row, column);
            return Redirect("Theater_detail?id=" + theater_id);
        }
        [HttpPost]
        public ActionResult Edittheate(int id_theater, string theatername, string location, int city, string urlimag)
        {
            try
            {
                TheaterDao.Instance().EditTheater(id_theater, theatername, location, city, urlimag);
                return RedirectToAction("Theater_detail", new { id = id_theater });
            }
            catch
            {
                return Redirect("Addnewtheater");
            }
        }

    }
}