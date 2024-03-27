using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.ModelView
{
    public class BookingView
    {
        public List<Bookingseat> bookingseats { get; set; }
        public List<Bookingcombo> bookingcombos { get; set; }
        public Booking booking { get; set; }
    }
}