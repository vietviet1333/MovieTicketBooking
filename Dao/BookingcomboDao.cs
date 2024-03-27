using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Dao
{
    public class BookingcomboDao
    {
        private BookingcomboDao() { }
        private static BookingcomboDao instance;
        public static BookingcomboDao Instance()
        {
            if(instance == null)
            {
                instance = new BookingcomboDao();
            }
            return instance;
        }
        public bool InsertBookingcombo(int combo_id, int quantity_combo,int booking_id )
        {
          bool  flagInsert = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                Bookingcombo bc= new Bookingcombo();
                bc.quantity_combo = quantity_combo;
                bc.combo_id = combo_id;
                bc.booking_id = booking_id;
                mv.Bookingcomboes.Add(bc);
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