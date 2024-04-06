using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.DynamicData;

namespace MovieTicketBooking.Dao
{
    public class ShowtimeDao
    {
        private ShowtimeDao() { }
        private static ShowtimeDao instance;
        public static ShowtimeDao Instance()
        {

            if (instance == null)
            {

                instance = new ShowtimeDao();

            }
            return instance;
        }
        public bool InsertShowtime(Showtime st)
        {
            bool flagInsertShowtime = true;
            try
            {
                int? moid = st.movie_id;
                MovieDao.Instance().UpdateStatusMovie(moid.Value);
                var mv = new MovieTicketBookingEntities2();
                Showtime s = new Showtime();
                s.movie_id = st.movie_id;
                s.room_id = st.room_id;
                s.show_date = st.show_date;
                s.starttime = st.starttime;
                s.endtime = st.endtime;
                s.theater_id = st.theater_id;
                s.status = st.status;
                mv.Showtimes.Add(s);
                mv.SaveChanges();
            }
            catch (Exception ex)
            {
                flagInsertShowtime = false;
                Console.WriteLine(ex.Message);
            }
            return flagInsertShowtime;
        }
        public List<Showtime> getShowtimeOfMovie(int id_movie)
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from t in mv.Showtimes where t.movie_id == id_movie select t).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public List<Showtime> getAllShowtime()
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = mv.Showtimes.ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public bool CheckShowtime(Showtime st)
        {
            bool flagcheck = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from t in mv.Showtimes where (t.room_id == st.room_id && t.starttime <= st.endtime && t.endtime >= st.starttime) select t).FirstOrDefault();
                if (result != null)
                {
                    flagcheck = false;
                }
                else
                {
                    flagcheck = true;
                }

            }
            catch
            {
                flagcheck = false;
            }
            return flagcheck;
        }
        public void AutoUpdateShowtime()
        {

            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from st in mv.Showtimes where st.starttime <= DateTime.Now select st).ToList();
                if (result != null)
                {
                    foreach (var r in result)
                    {
                        r.status = 1;
                        mv.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }

        }
        public List<Showtime> GetShowtimeofTheater(int theater_id, DateTime showtimedate)
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from st in mv.Showtimes where st.theater_id == theater_id && st.status == 0 && DbFunctions.TruncateTime(st.show_date) == showtimedate.Date select st).ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public Showtime ShowtimeBooking(int showtime_id)
        {
            try
            {
                using (var mv = new MovieTicketBookingEntities2())
                {
                    var result = (from st in mv.Showtimes
                                  join movie in mv.Movies on st.movie_id equals movie.movie_id
                                  join theater in mv.Theaters on st.theater_id equals theater.theater_id
                                  join room in mv.Rooms on st.room_id equals room.room_id // Assuming the foreign key relationship between Showtimes and Rooms
                                  where st.showtime_id == showtime_id
                                  select new
                                  {
                                      Showtime = st,
                                      Movie = movie,
                                      Theater = theater,
                                      Room = room
                                  }).FirstOrDefault();
                    if (result != null)
                    {
                        Showtime showtime = result.Showtime;
                        showtime.Movie = result.Movie;
                        showtime.Theater = result.Theater;
                        showtime.Room = result.Room; // Assigning room details to Showtime object
                        return showtime;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }



    }
}