using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace MovieTicketBooking.Dao
{
    public class MovieDao
    {
        private MovieDao() { }
        private static MovieDao instance;
        public static MovieDao Instance()
        {

            if (instance == null)
            {

                instance = new MovieDao();

            }
            return instance;
        }

        public bool Addnewmovie(string namemovi, string directo, string languag, DateTime release_dat, int duratio, string genr, string descriptio, string urlimag, string cas)
        {
            bool flagAddnewmovie = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                Movie movie = new Movie();
                movie.title = namemovi;
                movie.director = directo;
                movie.release_date = release_dat;
                movie.language = languag;
                movie.duration = duratio;
                movie.description = descriptio;
                movie.genre = genr;
                movie.poster_image = urlimag;
                movie.cast = cas;
                movie.status = 1;
                mv.Movies.Add(movie);
                mv.SaveChanges();

            }
            catch (Exception ex)
            {
                flagAddnewmovie = false;

                Console.WriteLine(ex.Message);

            }
            return flagAddnewmovie;
        }
        public List<Movie> GetAllMovies()
        {
            try
            {

                var mv = new MovieTicketBookingEntities2();
                var result = mv.Movies.OrderByDescending((s) => s.movie_id).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;

        }
        public Movie GetMovieById(int id_movie)
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from movie in mv.Movies where movie.movie_id  == id_movie select movie).FirstOrDefault();
                return result;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public List<Movie> GetMovieNowPlaying()
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from mo in mv.Movies where mo.status==1 && mo.release_date <DateTime.Now select mo).ToList();
                return result;
            }catch(Exception ex) { 
            Console.WriteLine(ex.Message);
            
            return null;
            }
        }
        public List<Movie> GetMovieCommingSoon()
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from mo in mv.Movies where mo.status==1 && mo.release_date>DateTime.Now select mo).ToList();
                return result;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }

}