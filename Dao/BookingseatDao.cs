using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Dao
{
    public class BookingseatDao
    {
        private BookingseatDao() { }
        private static BookingseatDao instance;
        public static BookingseatDao Instance()
        {
            if (instance == null)
            {
                instance = new BookingseatDao();
            }
            return instance;
        }
        public bool InsertBookingseat(string nameseat, int room_id, int booking_id)
        {
            bool flagInsert = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                Bookingseat b = new Bookingseat();
                b.booking_id = booking_id;
                b.room_id = room_id;
                b.nameseat = nameseat;
                mv.Bookingseats.Add(b);
                mv.SaveChanges();
                return flagInsert;
            }
            catch
            {
                flagInsert = false;
                return flagInsert;
            }
        }
        public String GetSeatsOfBooking(int showtime_id)
        {
            try
            {
                var mv=  new MovieTicketBookingEntities2();
                var result = (from bs in mv.Bookingseats
                              join b in mv.Bookings on bs.booking_id equals b.booking_id
                              where b.showtime_id == showtime_id
                              select bs.nameseat).ToList();
                string resultString = string.Join(",", result);
                return resultString;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}