using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Dao
{
    public class FoodDrinkDao
    {
        private FoodDrinkDao() { }
        private static FoodDrinkDao instance;
        public static FoodDrinkDao Instance()
        {

            if (instance == null)
            {

                instance = new FoodDrinkDao();

            }
            return instance;
        }
        public List<FoodDrink> GetFoodDrink()
        {
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var result = (from fd in mv.FoodDrinks where fd.fooddrink_status == 1 select fd).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
public bool AddCombo(FoodDrink foodDrink)
        {
            bool flagInsert = true;
            try
            {
                var mv = new MovieTicketBookingEntities2();
                var fd = new FoodDrink();
                fd.fooddrink_name = foodDrink.fooddrink_name;
                fd.fooddrink_description = foodDrink.fooddrink_description;
                fd.fooddrink_status = 1;
                fd.price = foodDrink.price;
                mv.FoodDrinks.Add(fd);
                mv.SaveChanges();
                return flagInsert;
            }
            catch
            {
                flagInsert =false;
                return flagInsert;
            }
        }
    }
}