using MovieTicketBooking.Models;
using MovieTicketBooking.ModelView;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
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
            if (instance == null)
            {
                instance = new BookingDao();
            }
            return instance;
        }
        public bool InsertBooking(string nameseats, string idcombos, string quantitycombos, int showtime_id, decimal totalofbooking, int room_id, int id_user)
        {
            bool flagInsert = true;
            try
            {

                var mv = new MovieTicketBookingEntities2();
                var b = new Booking();
                b.showtime_id = showtime_id;
                b.user_id = id_user;
                b.total_price = totalofbooking;
                b.booking_time = DateTime.Now;
                mv.Bookings.Add(b);
                mv.SaveChanges();

                string[] nameseatss = nameseats.Split(',');
                foreach (var nameseat in nameseatss)
                {
                    BookingseatDao.Instance().InsertBookingseat(nameseat, room_id, b.booking_id);
                }
                string[] bookingcombos = idcombos.Split(',');
                string[] quantities = quantitycombos.Split(',');
                for (int i = 0; i < bookingcombos.Length; i++)
                {
                    int combo_id = int.Parse(bookingcombos[i]);
                    int quantity = int.Parse(quantities[i]);
                    BookingcomboDao.Instance().InsertBookingcombo(combo_id, quantity, b.booking_id);
                }
                return flagInsert;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagInsert = false;
                return flagInsert;
            }
        }
        public bool CheckBooking(int showtime_id, string nameseats)
        {
            bool flagCheck = true;
            try
            {
                var check = BookingseatDao.Instance().GetSeatsOfBooking(showtime_id);
                string[] seatsselected = check.Split(',');
                string[] seatsnow = nameseats.Split(',');
                foreach (string seat in seatsnow)
                {
                    if (seatsselected.Contains(seat))
                    {
                        // Seat is already booked
                        flagCheck = false;
                        break; // No need to check further
                    }
                }
                return flagCheck;
            }
            catch
            {
                return flagCheck;
            }
        }
        public List<Booking> GetAllBooking()
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from bk in mv.Bookings select bk).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public List<Booking> GetBookingOfShowtime(int showtime_id)
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from bk in mv.Bookings where bk.showtime_id == showtime_id select bk).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public string GetTotalPriceBookingOfUser(int user_id)
        {
            try
            {
                var y = DateTime.Now;
                int year = y.Year;
                var mv = new MovieTicketBookingEntities2();
                var sumTotal = mv.Bookings.Where(booking => booking.user_id == user_id && booking.booking_time.HasValue && booking.booking_time.Value.Year == year).Sum(booking => booking.total_price);
                if (sumTotal > 0)
                {

                    return sumTotal.ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "0";
            }
        }
        public int RevenueByYear()
        {
            try
            {
                var y = DateTime.Now;
                int year = y.Year;
                var mv = new MovieTicketBookingEntities2();
                var sumTotal = mv.Bookings.Where(booking => booking.booking_time.HasValue && booking.booking_time.Value.Year == year).Sum(booking => booking.total_price);
                if (sumTotal > 0)
                {

                    return int.Parse(sumTotal.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public decimal RevenueByYearGrow()
        {
            try
            {
                var yn = RevenueByYear();
                var y = DateTime.Now;
                int year = y.Year - 1;
                var mv = new MovieTicketBookingEntities2();
                var sumTotal = mv.Bookings.Where(booking => booking.booking_time.HasValue && booking.booking_time.Value.Year == year).Sum(booking => booking.total_price);
                if (sumTotal > 0)
                {
                    var percent = (yn - sumTotal) / 100;
                    return decimal.Parse(percent.ToString());
                }
                else
                {
                    return 100;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 100;
            }
        }
        public int RevenueByMonth()
        {
            try
            {
                var y = DateTime.Now;
                int year = y.Year;
                int month = y.Month;
                var mv = new MovieTicketBookingEntities2();
                var sumTotal = mv.Bookings.Where(booking => booking.booking_time.HasValue && booking.booking_time.Value.Month == month && booking.booking_time.Value.Year == year).Sum(booking => booking.total_price);
                if (sumTotal > 0)
                {

                    return int.Parse(sumTotal.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public decimal RevenueByMonthGrow()
        {
            try
            {
                var yn = RevenueByMonth();

                var y = DateTime.Now;
                int year = y.Year;
                int month = y.Month - 1;
                var mv = new MovieTicketBookingEntities2();
                var sumTotal = mv.Bookings.Where(booking => booking.booking_time.HasValue && booking.booking_time.Value.Month == month && booking.booking_time.Value.Year == year).Sum(booking => booking.total_price);
                if (sumTotal > 0)
                {
                    var percent = (yn - sumTotal) / 100;
                    return decimal.Parse(percent.ToString());
                }
                else
                {
                    return 100;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 100;
            }
        }
        public List<BookingView> GetBookingOfUser(int user_id)
        {
            try
            {
                using (var mv = new MovieTicketBookingEntities2())
                {
                    var result = (from bk in mv.Bookings
                                  join st in mv.Showtimes on bk.showtime_id equals st.showtime_id
                                  join movie in mv.Movies on st.movie_id equals movie.movie_id
                                  join th in mv.Theaters on st.theater_id equals th.theater_id
                                  where bk.user_id == user_id
                                  select new BookingView
                                  {
                                      booking = bk,
                                      movie = movie,
                                      showtime = st,
                                      theater = th,
                                  }).OrderByDescending(x=>x.booking.booking_id).ToList();

                    if (result.Count > 0)
                    {
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return null;
            }
        }
        public BookingView GetHistorySeatCombo(int id_booking)
        {
            using (var mv = new MovieTicketBookingEntities2())
            {
                var result = (from bk in mv.Bookings
                              where bk.booking_id == id_booking
                              select new BookingView
                              {
                                  booking = bk,
                                  bookingseats = mv.Bookingseats.Where(b => b.booking_id == bk.booking_id).ToList(),
                                  bookingcombos = mv.Bookingcomboes.Where(bc => bc.booking_id == bk.booking_id).ToList()

                              }).FirstOrDefault();
                return result;
            }
        }
        public int EmptySeatOfShowtime(int showtime_id)
        {
            var countseat = 0;
            try
            {
                using (var mv = new MovieTicketBookingEntities2())
                {
                    var bookingofshowtime = (from bk in mv.Bookings where bk.showtime_id == showtime_id select bk.booking_id).ToList();
                    if (bookingofshowtime != null)
                    {
                        foreach (var bkst in bookingofshowtime)
                        {
                            var count = mv.Bookingseats.Count(bs => bs.booking_id == bkst);
                            countseat += count;
                        }
                    }
                    else
                    {
                        countseat = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                countseat = 0;
            }
            return countseat;
        }
        public List<decimal> ChartByYear(int year)
        {
            List<decimal> monthlyTotals = new List<decimal>();

            try
            {
                using (var mv = new MovieTicketBookingEntities2())
                {
              
                    if (year == DateTime.Now.Year)
                    {
                        for (int i = 1; i < DateTime.Now.Month +1; i++)
                        {
                            var totalOfMonth = mv.Bookings
                                .Where(x => x.booking_time.HasValue && x.booking_time.Value.Year == year && x.booking_time.Value.Month == i)
                                .Sum(x => x.total_price);

                            if (totalOfMonth != null)
                            {
                                monthlyTotals.Add(decimal.Parse(totalOfMonth.ToString()));
                            }
                            else
                            {
                                monthlyTotals.Add(0);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < 13; i++)
                        {
                            var totalOfMonth = mv.Bookings
                                .Where(x => x.booking_time.HasValue && x.booking_time.Value.Year == year && x.booking_time.Value.Month == i)
                                .Sum(x => x.total_price);

                            if (totalOfMonth != null)
                            {
                                monthlyTotals.Add(decimal.Parse(totalOfMonth.ToString()));
                            }
                            else
                            {
                                monthlyTotals.Add(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
            }

            return monthlyTotals;
        }

    }
}