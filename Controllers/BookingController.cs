using MovieTicketBooking.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTicketBooking.Controllers
{
    public class BookingController : Controller
    {
        [HttpPost]
        public bool CheckBooking(string seats,int showtime)
        {
            bool check = BookingDao.Instance().CheckBooking(showtime,seats);
            return check;
        }
        [HttpPost]
        public ActionResult HistoryTransaction( int id_booking)
        {
            try
            {
                var result = BookingDao.Instance().GetHistorySeatCombo(id_booking);
                return PartialView("Historytransaction", result);
            }catch
            {
                return PartialView("Errors");
            }
        }

    }
}