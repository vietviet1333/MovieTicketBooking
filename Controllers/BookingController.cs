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


    }
}