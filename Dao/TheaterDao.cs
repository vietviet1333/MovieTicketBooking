using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Dao
{
    public class TheaterDao
    {
        private TheaterDao() { }
        private static TheaterDao instance;

        public static TheaterDao Instance()
        {

            if (instance == null)
            {

                instance = new TheaterDao();

            }
            return instance;
        }

        public bool Insertcity(string name)
        {

            bool flagInsertcity = true;
            try
            {
                MovieTicketBookingEntities2 mv = new MovieTicketBookingEntities2();
                City city = new City();
                city.title_city = name;
                mv.Cities.Add(city);
                mv.SaveChanges();
            }
            catch (Exception e)
            {
                flagInsertcity = false;
                Console.WriteLine(e.Message);
            }
            return flagInsertcity;
        }
        public List<City> getAllCity()
        {
            try
            {
                MovieTicketBookingEntities2 mv = new MovieTicketBookingEntities2();
                var result = mv.Cities.OrderByDescending(x=>x.id_city).ToList();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public bool Inserttheater(string name, string location, int idcity, string img)
        {
            bool flagInserttheater = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                Theater t = new Theater();
                t.name = name;
                t.location = location;
                t.city_id = idcity;
                t.Theater_image = img;
                mv.Theaters.Add(t);
                mv.SaveChanges();
            }
            catch (Exception e)
            {
                flagInserttheater = false;
                Console.WriteLine(e.Message);
            }
            return flagInserttheater;

        }
        public List<Theater> getAllTheater()
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                return mv.Theaters.OrderByDescending(x=>x.theater_id).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public Theater getTheaterById(int id)
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();

                var t = (from tt in mv.Theaters where tt.theater_id == id select tt).FirstOrDefault();
                return t;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public bool EditTheater(int id_theater, string name, string location, int idcity, string img)
        {

            bool flagEditTheater = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var r = mv.Theaters.SingleOrDefault(x => x.theater_id == id_theater);
                r.name = name;
                r.location = location;
                r.city_id = idcity;
                r.Theater_image = img;
                mv.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                flagEditTheater = false;

            }
            return flagEditTheater;
        }
        public List<Theater> GetTheatersWithShowtime(int cityId, DateTime showDate,int movie_id)
        {
            try
            {
                using (var mv = new MovieTicketBookingEntities2())
                {
                    var result = (from th in mv.Theaters
                                  join st in mv.Showtimes on th.theater_id equals st.theater_id
                                  join mo in mv.Movies on st.movie_id equals mo.movie_id
                                  where th.city_id == cityId && DbFunctions.TruncateTime(st.show_date) == showDate.Date && st.movie_id== movie_id && st.status== 0
                                  select th).Distinct().ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
                // Log the error or rethrow the exception here.
                return new List<Theater>(); // Return an empty list in case of error.
            }
        }
        public List<City> GetCityShowtime()
        {
            try
            {
                var mv = new MovieTicketBookingEntities2(); 
                var selectedCities = mv.Cities.ToList();
                return selectedCities;
            }
            catch
            {
                return null;
            }
        }
    }
}