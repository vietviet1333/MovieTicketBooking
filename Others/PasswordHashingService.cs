using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTicketBooking.Bcrypt
{
    public class PasswordHashingService
    {
       private PasswordHashingService() { }
        private static PasswordHashingService instance;
        public static PasswordHashingService Instance()
        {
            if(instance == null)
            {
                instance = new PasswordHashingService();
            }
            return instance;
        }
        public string HashPassword(string password)
        {
            // Hash a password with a random salt
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Verify that a password matches the hashed password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}