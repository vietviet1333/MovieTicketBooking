using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Dao
{
    public class BookingDao
    {
        private BookingDao() { }
        private static BookingDao instance;
        public static BookingDao Instance()
        {
            if(instance == null)
            {
                instance= new BookingDao();
            }
            return instance;
        }
        public bool InsertBooking(Booking booking)
        {
            bool flagInsert = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var b = new Booking();
                b.showtime_id= booking.showtime_id;
                b.user_id= booking.user_id;
                b.number_of_seats= booking.number_of_seats;
                b.total_price= booking.total_price;
                b.booking_time= booking.booking_time;
                b.combo = booking.combo;
                mv.Bookings.Add(b);
                mv.SaveChanges();
                return flagInsert;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagInsert = false;
                return flagInsert;
            }
        }

    }
}