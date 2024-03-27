using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Security.Cryptography;
using System.Web;

namespace MovieTicketBooking.Dao
{
    public class RoomDao
    {
        public RoomDao() { }
        private static RoomDao instance;

        public static RoomDao Instance()
        {

            if (instance == null)
            {

                instance = new RoomDao();

            }
            return instance;
        }
        public bool Insertnewroomandchair(string room_name, int theater_id, int row, int column)
        {
            bool flagInsertroom = true;
            try
            {
               
                using (var mv = new MovieTicketBookingEntities2())
                {
                    Room r = new Room();
                    r.room_name = room_name;
                    r.theater_id = theater_id;
                    r.number_row = row;
                    r.number_column = column;
                    mv.Rooms.Add(r);
                    mv.SaveChanges();
                    for (int i = 0; i < row * column; i++)
                    {
                       
                        Chair c = new Chair();
                        c.name_chair = "A" + i;
                        c.room_id = r.room_id;
                        mv.Chairs.Add(c);
                        mv.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                flagInsertroom = false;
                Console.WriteLine(ex.ToString());
            }
            return flagInsertroom;
        }
        public List<Room> GetAllRoomsOfTheater( int id_theater)
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var r = (from rr in mv.Rooms where rr.theater_id == id_theater select rr ).ToList();
                return r;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
       
    }
}